using System;
using System.Collections;
using CardioCore;
using UnityEngine;
using UnityEngine.Networking;

namespace Core
{
    public sealed class DataClient : MonoBehaviour
    {
        public event Action<TwinState> OnTwinState;

        [Header("Source")]
        [SerializeField] private bool useMock = true;
        [SerializeField] private string host = "127.0.0.1";
        [SerializeField] private int port = 5000;
        [SerializeField] private string route = "/data";

        [Header("Polling")]
        [SerializeField, Range(10f, 60f)] private float pollHz = 30f;
        [SerializeField] private float requestTimeout = 1f;

        [Header("Mock")]
        [SerializeField, Range(40f, 180f)] private float mockBpm = 68f;
        [SerializeField, Range(30f, 70f)] private float mockRS1Ms = 45f;
        [SerializeField, Range(270f, 330f)] private float mockS1S2Ms = 300f;
        [SerializeField] private int mockWindow = 256;
        private string mockRhythm = "normal";
        private string mockMurmur = "none";


        private readonly TwinState state = new TwinState();
        private readonly TwinState.Wire wire = new TwinState.Wire();
        private float[] ecgBuf;
        private float[] pcgBuf;
        private WaitForSeconds pollWait;
        private float mockPhase;
        private bool connected;

        public bool IsConnected => connected;
        public bool UsingMock => useMock;
        public TwinState Latest => state;

        private void OnEnable()
        {
            ecgBuf = new float[mockWindow];
            pcgBuf = new float[mockWindow];
            pollWait = new WaitForSeconds(1f / Mathf.Max(1f, pollHz));
            StartCoroutine(Loop());
        }

        private void OnDisable() => StopAllCoroutines();

        private IEnumerator Loop()
        {
            while (true)
            {
                if (useMock) StepMock();
                else yield return Fetch();
                yield return pollWait;
            }
        }

        private IEnumerator Fetch()
        {
            using UnityWebRequest req = UnityWebRequest.Get($"http://{host}:{port}{route}");
            req.timeout = Mathf.CeilToInt(requestTimeout);
            yield return req.SendWebRequest();

            if (req.result != UnityWebRequest.Result.Success)
            {
                connected = false;
                yield break;
            }

            try
            {
                JsonUtility.FromJsonOverwrite(req.downloadHandler.text, wire);
                state.CopyFrom(wire);
                connected = true;
            }
            catch
            {
                connected = false;
                yield break;
            }

            OnTwinState?.Invoke(state);
        }

        private void StepMock()
        {
            float cycle = 60f / Mathf.Max(1f, mockBpm);
            mockPhase += (1f / Mathf.Max(1f, pollHz)) / cycle;
            if (mockPhase >= 1f) mockPhase -= 1f;

            FillMock(cycle);

            state.Bpm = mockBpm;
            state.Phase = mockPhase;
            state.RPeakT = Time.realtimeSinceStartupAsDouble;
            state.RS1Ms = mockRS1Ms;
            state.S1S2Ms = mockS1S2Ms;
            state.RhythmLabel = mockRhythm;
            state.RhythmConf = 0.95f;
            state.MurmurLabel = mockMurmur;
            state.MurmurConf = 0.9f;
            state.LeadOff = false;
            state.Ecg = ecgBuf;
            state.PcgEnv = pcgBuf;

            connected = true;
            OnTwinState?.Invoke(state);
        }

        public void SetMockRhythm(string l) => mockRhythm = l;
        public void SetMockMurmur(string l) => mockMurmur = l;

        
// placeholder morphology only; the AI team owns real ecg[] / pcg_env[]
        private void FillMock(float cycle)
        {
            float now = Time.realtimeSinceStartup;
            float dt = (2f * cycle) / mockWindow;
            float s1At = mockRS1Ms / 1000f / cycle;
            float s2At = (mockRS1Ms + mockS1S2Ms) / 1000f / cycle;

            for (int i = 0; i < mockWindow; i++)
            {
                float ph = Mathf.Repeat((now - (mockWindow - 1 - i) * dt) / cycle, 1f);
                ecgBuf[i] = Bump(ph, 0.0f, 0.010f, 1.0f) - Bump(ph, 0.02f, 0.012f, 0.25f)
                            + Bump(ph, 0.35f, 0.05f, 0.2f);
                pcgBuf[i] = Bump(ph, s1At, 0.03f, 1.0f) + Bump(ph, s2At, 0.03f, 0.7f);
            }
        }

        private static float Bump(float x, float c, float w, float a)
        {
            float d = Mathf.Repeat(x - c + 0.5f, 1f) - 0.5f;
            return a * Mathf.Exp(-(d * d) / (2f * w * w));
        }
    }
}