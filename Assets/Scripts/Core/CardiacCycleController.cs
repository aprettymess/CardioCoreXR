using System;
using UnityEngine;

namespace Core
{
    public enum CyclePhase { Diastole, AtrialKick, Systole }

    public sealed class CardiacCycleController : MonoBehaviour
    {
        public event Action OnBeat;   // R-peak (electrical)
        public event Action OnS1;     // AV closure, systole onset
        public event Action OnS2;     // semilunar closure, diastole onset

        [Header("Fallback rate (until first Sync)")]
        [SerializeField, Range(40f, 180f)] private float fallbackBpm = 68f;
        [SerializeField, Range(30f, 70f)] private float fallbackRs1Ms = 45f;
        [SerializeField, Range(270f, 330f)] private float fallbackS1S2Ms = 300f;

        [Header("Sync")]
        [SerializeField, Range(0f, 1f)] private float phaseCorrectGain = 0.15f;
        [SerializeField, Range(0.05f, 0.5f)] private float resyncSnapThreshold = 0.2f;

        [Header("Demo-tuned timing (NOT truth sheet — confirm with Umer)")]
        [SerializeField, Range(0f, 80f)] private float isovolumicMs = 40f;
        [SerializeField, Range(2f, 40f)] private float s1SnapMs = 18f;
        [SerializeField, Range(2f, 40f)] private float s2SnapMs = 18f;
        [SerializeField, Range(40f, 160f)] private float atrialLeadMs = 90f;
        [SerializeField, Range(20f, 120f)] private float atrialWidthMs = 70f;

        private float bpm;
        private float rs1Ms;
        private float s1s2Ms;
        private float phase;      // 0..1, 0 = R-peak
        private float prevPhase;

        public float Phase => phase;
        public float Bpm => bpm;
        public float RS1Ms => rs1Ms;
        public CyclePhase CurrentPhase { get; private set; }

        public float VentricularContraction { get; private set; }
        public float AtrialContraction { get; private set; }
        public float MitralOpen { get; private set; }   // 1 = open, drives AV pair
        public float AorticOpen { get; private set; }   // 1 = open, drives semilunar pair

        private void Awake()
        {
            bpm = fallbackBpm;
            rs1Ms = fallbackRs1Ms;
            s1s2Ms = fallbackS1S2Ms;
        }

        // phase01 = fraction since R-peak (see Kashif flag below)
        public void Sync(float newBpm, float phase01, float newRs1Ms, float newS1S2Ms)
        {
            bpm = Mathf.Clamp(newBpm, 20f, 220f);
            rs1Ms = newRs1Ms;
            s1s2Ms = newS1S2Ms;

            float target = Mathf.Repeat(phase01, 1f);
            float err = Mathf.Repeat(target - phase + 0.5f, 1f) - 0.5f;
            if (Mathf.Abs(err) > resyncSnapThreshold) phase = target;
            else phase = Mathf.Repeat(phase + err * phaseCorrectGain, 1f);
        }

        private void Update()
        {
            float T = 60f / Mathf.Max(1f, bpm);
            prevPhase = phase;
            phase = Mathf.Repeat(phase + Time.deltaTime / T, 1f);

            float s1P = Ph(rs1Ms, T);
            float s2P = Mathf.Min(Ph(rs1Ms + s1s2Ms, T), 0.95f);
            float openP = Mathf.Min(Ph(rs1Ms + isovolumicMs, T), s2P - 0.02f);
            float snap1 = Ph(s1SnapMs, T);
            float snap2 = Ph(s2SnapMs, T);

            if (Crossed(0f)) OnBeat?.Invoke();
            if (Crossed(s1P)) OnS1?.Invoke();
            if (Crossed(s2P)) OnS2?.Invoke();

            MitralOpen = AvOpen(phase, s1P, s2P, snap1, snap2);
            AorticOpen = SlOpen(phase, openP, s2P, snap1, snap2);
            VentricularContraction = Contract(phase, s1P, s2P);
            AtrialContraction = Atrial(phase, T);

            if (phase >= s1P && phase < s2P) CurrentPhase = CyclePhase.Systole;
            else if (AtrialContraction > 0.05f) CurrentPhase = CyclePhase.AtrialKick;
            else CurrentPhase = CyclePhase.Diastole;
        }

        private static float Ph(float ms, float T) => (ms / 1000f) / T;

        private bool Crossed(float mark)
        {
            if (prevPhase <= phase) return prevPhase < mark && mark <= phase;
            return prevPhase < mark || mark <= phase;
        }

        private static bool In(float x, float a, float b) => x >= a && x < b;
        private static float Smooth(float t) { t = Mathf.Clamp01(t); return t * t * (3f - 2f * t); }

        private static float AvOpen(float p, float s1P, float s2P, float snap1, float snap2)
        {
            if (In(p, s1P, s1P + snap1)) return 1f - Smooth((p - s1P) / snap1);
            if (In(p, s1P + snap1, s2P)) return 0f;
            if (In(p, s2P, s2P + snap2)) return Smooth((p - s2P) / snap2);
            return 1f;
        }

        private static float SlOpen(float p, float openP, float s2P, float snapOpen, float snap2)
        {
            if (In(p, openP, openP + snapOpen)) return Smooth((p - openP) / snapOpen);
            if (In(p, openP + snapOpen, s2P)) return 1f;
            if (In(p, s2P, s2P + snap2)) return 1f - Smooth((p - s2P) / snap2);
            return 0f;
        }

        private static float Contract(float p, float s1P, float s2P)
        {
            float span = Mathf.Max(0.0001f, s2P - s1P);
            float attack = span * 0.35f;
            float release = span * 0.5f;
            if (In(p, s1P, s1P + attack)) return Smooth((p - s1P) / attack);
            if (In(p, s1P + attack, s2P)) return 1f;
            if (In(p, s2P, s2P + release)) return 1f - Smooth((p - s2P) / release);
            return 0f;
        }

        private float Atrial(float p, float T)
        {
            float center = Mathf.Repeat(1f - (atrialLeadMs / 1000f) / T, 1f);
            float half = ((atrialWidthMs / 1000f) / T) * 0.5f;
            float d = Mathf.Repeat(p - center + 0.5f, 1f) - 0.5f;
            float x = d / Mathf.Max(0.0001f, half);
            if (Mathf.Abs(x) >= 1f) return 0f;
            return 0.5f * (1f + Mathf.Cos(Mathf.PI * x));
        }
    }
}