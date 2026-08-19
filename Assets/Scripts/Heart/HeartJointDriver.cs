using System.Collections.Generic;
using Core;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Heart
{
    public sealed class HeartJointDriver : MonoBehaviour
    {
        private enum Channel { Position, Rotation, Scale }
        private enum Driver { AvClose, SlOpen, Ventricle, Atria }

        [Header("Bindings")]
        [SerializeField] private CardiacCycleController cycle;
        [SerializeField] private Transform jointRoot;   // heart_jnt.5

        [Header("Motion")]
        [SerializeField, Range(0f, 1f)] private float globalGain = 1f;
        [SerializeField] private bool driveEnabled = true;

        [Title("Manual Inspection (Play Mode)")]
        [SerializeField, ToggleLeft] private bool manualOverride;
        [SerializeField, PropertyRange(0f, 1f), ShowIf("manualOverride")] private float mAvClose;
        [SerializeField, PropertyRange(0f, 1f), ShowIf("manualOverride")] private float mSlOpen;
        [SerializeField, PropertyRange(0f, 1f), ShowIf("manualOverride")] private float mVentricle;
        [SerializeField, PropertyRange(0f, 1f), ShowIf("manualOverride")] private float mAtria;

        private sealed class Bound
        {
            public Transform t;
            public Channel channel;
            public Driver driver;
            public Vector3 restPos, peakPos, restScale, peakScale;
            public Quaternion restRot, peakRot;
        }

        private readonly List<Bound> bound = new List<Bound>();

        private struct Spec
        {
            public string path; public Channel ch; public Driver dv;
            public Vector3 peakV; public Vector3 peakEuler;
            public Spec(string p, Channel c, Driver d, Vector3 pv, Vector3 pe)
            { path = p; ch = c; dv = d; peakV = pv; peakEuler = pe; }
        }

        // peaks straight from beat_pose_dump.txt
        private static readonly Spec[] specs =
        {
            new Spec("right_atrium_jnt.6",        Channel.Position, Driver.Atria,     new Vector3(5.175f, 0.968f, 3.432f),  default),
            new Spec("left_atrium_jnt.13",        Channel.Position, Driver.Atria,     new Vector3(-3.324f, 2.538f, 3.252f), default),
            new Spec("left_atrium_storage_jnt.14",Channel.Position, Driver.Atria,     new Vector3(-1.251f, 2.212f, -2.264f),default),

            new Spec("cardiac_muscle_jnt.7",      Channel.Scale,    Driver.Ventricle, new Vector3(0.974f, 1.343f, 1.049f),  default),

            new Spec("left_mitral_valve_jnt.15",  Channel.Position, Driver.AvClose,   new Vector3(-2.642f, -0.964f, 2.525f),default),
            new Spec("right_mitral_valve_jnt.16", Channel.Position, Driver.AvClose,   new Vector3(-2.75f, -0.977f, 2.406f), default),
            new Spec("left_tricuspid_valve_jnt.23",Channel.Position,Driver.AvClose,   new Vector3(2.636f, -2.702f, 1.957f), default),
            new Spec("right_tricuspid_valve_jnt.24",Channel.Position,Driver.AvClose,  new Vector3(2.691f, -2.641f, 2.275f), default),

            new Spec("aortic_valve_01_jnt.21",    Channel.Rotation, Driver.SlOpen,    default, new Vector3(45.231f, 177.429f, 222.245f)),
            new Spec("aortic_valve_02_jnt.17",    Channel.Rotation, Driver.SlOpen,    default, new Vector3(7.633f, 43.124f, 119.626f)),
            new Spec("aortic_valve_03_jnt.19",    Channel.Rotation, Driver.SlOpen,    default, new Vector3(314.069f, 261.163f, 220.656f)),
            new Spec("left_pulmonary_valve_jnt.11",Channel.Rotation,Driver.SlOpen,    default, new Vector3(2.524f, 184.68f, 327.837f)),
            new Spec("right_pulmonary_valve_jnt.9",Channel.Rotation,Driver.SlOpen,    default, new Vector3(2.334f, 0f, 5.363f)),
        };

        private void Awake()
        {
            if (jointRoot == null) { Debug.LogError("HeartJointDriver: jointRoot unassigned."); enabled = false; return; }

            foreach (Spec s in specs)
            {
                Transform t = FindByLeaf(jointRoot, s.path);
                if (t == null) { Debug.LogWarning($"HeartJointDriver: joint not found: {s.path}"); continue; }

                var b = new Bound { t = t, channel = s.ch, driver = s.dv };
                b.restPos = t.localPosition;
                b.restRot = t.localRotation;
                b.restScale = t.localScale;
                b.peakPos = s.peakV;
                b.peakRot = Quaternion.Euler(s.peakEuler);
                b.peakScale = s.peakV;
                bound.Add(b);
            }
        }

        private void LateUpdate()
        {
            if (!driveEnabled) return;

            float av, sl, ven, atr;
            if (manualOverride)
            {
                av = mAvClose; sl = mSlOpen; ven = mVentricle; atr = mAtria;
            }
            else
            {
                if (cycle == null) return;
                av = 1f - cycle.MitralOpen;
                sl = cycle.AorticOpen;
                ven = cycle.VentricularContraction;
                atr = cycle.AtrialContraction;
            }

            av *= globalGain; sl *= globalGain; ven *= globalGain; atr *= globalGain;

            for (int i = 0; i < bound.Count; i++)
            {
                Bound b = bound[i];
                float f = b.driver switch
                {
                    Driver.AvClose => av,
                    Driver.SlOpen => sl,
                    Driver.Ventricle => ven,
                    Driver.Atria => atr,
                    _ => 0f
                };

                switch (b.channel)
                {
                    case Channel.Position:
                        b.t.localPosition = Vector3.LerpUnclamped(b.restPos, b.peakPos, f);
                        break;
                    case Channel.Rotation:
                        b.t.localRotation = Quaternion.SlerpUnclamped(b.restRot, b.peakRot, f);
                        break;
                    case Channel.Scale:
                        b.t.localScale = Vector3.LerpUnclamped(b.restScale, b.peakScale, f);
                        break;
                }
            }
        }

        [ButtonGroup("av"), ShowIf("manualOverride")]
        private void AvClosed() { manualOverride = true; mAvClose = 1f; }
        [ButtonGroup("av"), ShowIf("manualOverride")]
        private void AvOpen() { manualOverride = true; mAvClose = 0f; }

        [ButtonGroup("sl"), ShowIf("manualOverride")]
        private void SlOpened() { manualOverride = true; mSlOpen = 1f; }
        [ButtonGroup("sl"), ShowIf("manualOverride")]
        private void SlClosed() { manualOverride = true; mSlOpen = 0f; }

        [ButtonGroup("chambers"), ShowIf("manualOverride")]
        private void VentricleContracted() { manualOverride = true; mVentricle = 1f; }
        [ButtonGroup("chambers"), ShowIf("manualOverride")]
        private void AtriaContracted() { manualOverride = true; mAtria = 1f; }

        [ButtonGroup("whole"), ShowIf("manualOverride")]
        private void PoseDiastole() { manualOverride = true; mAvClose = 0f; mSlOpen = 0f; mVentricle = 0f; mAtria = 0f; }
        [ButtonGroup("whole"), ShowIf("manualOverride")]
        private void PoseSystole() { manualOverride = true; mAvClose = 1f; mSlOpen = 1f; mVentricle = 1f; mAtria = 0f; }
        [ButtonGroup("whole"), ShowIf("manualOverride")]
        private void ResetAll() { mAvClose = 0f; mSlOpen = 0f; mVentricle = 0f; mAtria = 0f; }

        [Button, ShowIf("manualOverride")]
        private void ReturnToLiveCycle() => manualOverride = false;

        private static Transform FindByLeaf(Transform root, string leaf)
        {
            if (root.name == leaf) return root;
            for (int i = 0; i < root.childCount; i++)
            {
                Transform found = FindByLeaf(root.GetChild(i), leaf);
                if (found != null) return found;
            }
            return null;
        }
    }
}