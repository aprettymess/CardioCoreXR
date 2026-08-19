using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEditor;
using UnityEngine;

namespace CardioCore.EditorTools
{
    public sealed class BeatPoseDumper : EditorWindow
    {
        private GameObject root;
        private AnimationClip clip;
        private int steps = 90;

        [MenuItem("CardioCore/Beat Pose Dumper")]
        private static void Open() => GetWindow<BeatPoseDumper>("Beat Pose Dumper");

        private void OnGUI()
        {
            EditorGUILayout.HelpBox(
                "Put the Beating heart in a scene. Select it as Heart Root, assign its beat clip, click Dump.",
                MessageType.Info);
            root = (GameObject)EditorGUILayout.ObjectField("Heart Root (scene)", root, typeof(GameObject), true);
            clip = (AnimationClip)EditorGUILayout.ObjectField("Beat Clip", clip, typeof(AnimationClip), false);
            steps = EditorGUILayout.IntSlider("Samples", steps, 8, 240);

            using (new EditorGUI.DisabledScope(root == null || clip == null))
                if (GUILayout.Button("Dump Poses")) Run();
        }

        private void Run()
        {
            Transform[] all = root.GetComponentsInChildren<Transform>(true);
            int n = all.Length;
            var p = new Vector3[n][];
            var r = new Quaternion[n][];
            var s = new Vector3[n][];
            for (int j = 0; j < n; j++) { p[j] = new Vector3[steps]; r[j] = new Quaternion[steps]; s[j] = new Vector3[steps]; }

            for (int i = 0; i < steps; i++)
            {
                float time = steps == 1 ? 0f : (float)i / (steps - 1) * clip.length;
                clip.SampleAnimation(root, time);
                for (int j = 0; j < n; j++)
                {
                    p[j][i] = all[j].localPosition;
                    r[j][i] = all[j].localRotation;
                    s[j][i] = all[j].localScale;
                }
            }

            var sb = new StringBuilder();
            sb.AppendLine($"# BeatPoseDump root={root.name} clip={clip.name} len={clip.length:0.###}s samples={steps}");
            sb.AppendLine("# rest = sample 0. peak = frame of max displacement from rest. tPeak normalized 0..1.");
            sb.AppendLine();

            for (int j = 0; j < n; j++)
            {
                int peak = 0;
                float best = 0f;
                for (int i = 1; i < steps; i++)
                {
                    float score = Quaternion.Angle(r[j][0], r[j][i])
                                + (p[j][i] - p[j][0]).magnitude * 1000f
                                + (s[j][i] - s[j][0]).magnitude * 1000f;
                    if (score > best) { best = score; peak = i; }
                }
                if (best < 0.05f) continue;

                float rotAng = Quaternion.Angle(r[j][0], r[j][peak]);
                float posDlt = (p[j][peak] - p[j][0]).magnitude;
                float sclDlt = (s[j][peak] - s[j][0]).magnitude;
                float tPeak = steps == 1 ? 0f : (float)peak / (steps - 1);

                sb.AppendLine($"[{Path(all[j])}]  tPeak={tPeak:0.00}");
                if (rotAng > 0.05f)
                    sb.AppendLine($"  rot rest={Fmt(r[j][0].eulerAngles)} peak={Fmt(r[j][peak].eulerAngles)} (dAngle={rotAng:0.0})");
                if (posDlt > 0.0001f)
                    sb.AppendLine($"  pos rest={Fmt(p[j][0])} peak={Fmt(p[j][peak])} (d={posDlt:0.####})");
                if (sclDlt > 0.0001f)
                    sb.AppendLine($"  scl rest={Fmt(s[j][0])} peak={Fmt(s[j][peak])} (d={sclDlt:0.####})");
                sb.AppendLine();
            }

            clip.SampleAnimation(root, 0f);

            const string dir = "Assets/CardioCore_Dumps";
            if (!Directory.Exists(dir)) Directory.CreateDirectory(dir);
            string file = $"{dir}/beat_pose_dump.txt";
            File.WriteAllText(file, sb.ToString());
            AssetDatabase.Refresh();
            Debug.Log($"Beat pose dump written to {file}\n\n{sb}");
        }

        private string Path(Transform t)
        {
            if (t == root.transform) return t.name;
            var stack = new List<string>();
            Transform cur = t;
            while (cur != null && cur != root.transform) { stack.Add(cur.name); cur = cur.parent; }
            stack.Reverse();
            return string.Join("/", stack);
        }

        private static string Fmt(Vector3 v) => $"({v.x:0.###}, {v.y:0.###}, {v.z:0.###})";
    }
}