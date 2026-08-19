using Core;
using UnityEngine;

namespace CardioCore
{
    public sealed class CycleProbe : MonoBehaviour
    {
        [SerializeField] private CardiacCycleController cycle;
        private float s1Flash, s2Flash, beatFlash;
        private int s1Count, s2Count;

        private void OnEnable()
        {
            if (cycle == null) return;
            cycle.OnBeat += () => beatFlash = Time.time;
            cycle.OnS1 += () => { s1Flash = Time.time; s1Count++; };
            cycle.OnS2 += () => { s2Flash = Time.time; s2Count++; };
        }

        private void OnGUI()
        {
            if (cycle == null) return;
            GUI.skin.label.fontSize = 16;
            var r = new Rect(14, 12, 460, 24);
            GUI.Label(r, $"phase {cycle.Phase:0.00}   bpm {cycle.Bpm:0}   {cycle.CurrentPhase}"); r.y += 26;
            Bar(ref r, "AV open  ", cycle.MitralOpen);
            Bar(ref r, "SL open  ", cycle.AorticOpen);
            Bar(ref r, "Ventricle", cycle.VentricularContraction);
            Bar(ref r, "Atria    ", cycle.AtrialContraction);
            r.y += 8;
            GUI.Label(r, $"{Flash("R", beatFlash)}  {Flash("S1", s1Flash)} x{s1Count}  {Flash("S2", s2Flash)} x{s2Count}");
        }

        private void Bar(ref Rect r, string label, float v)
        {
            GUI.Label(new Rect(r.x, r.y, 90, 22), label);
            var bg = new Rect(r.x + 96, r.y + 3, 300, 16);
            GUI.Box(bg, GUIContent.none);
            GUI.Box(new Rect(bg.x, bg.y, bg.width * Mathf.Clamp01(v), bg.height), GUIContent.none);
            r.y += 24;
        }

        private string Flash(string tag, float t) => Time.time - t < 0.15f ? $"[{tag}!]" : $" {tag} ";
    }
}