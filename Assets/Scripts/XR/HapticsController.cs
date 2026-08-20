using System.Collections;
using Core;
using UnityEngine;

namespace XR
{
    public sealed class HapticsController : MonoBehaviour
    {
        private enum Target { Both, LeftHand, RightHand, Active }

        [Header("References")]
        [SerializeField] private CardiacCycleController cycle;

        [Header("S1 Buzz")]
        [SerializeField, Range(0f, 1f)] private float amplitude = 0.8f;
        [SerializeField, Range(0f, 1f)] private float frequency = 0.6f;
        [SerializeField, Range(10f, 200f)] private float durationMs = 70f;
        [SerializeField] private Target target = Target.Both;

        [Header("S2 Buzz (optional)")]
        [SerializeField] private bool alsoBuzzS2;
        [SerializeField, Range(0f, 1f)] private float s2Amplitude = 0.35f;

        private Coroutine running;

        private void OnEnable()
        {
            if (cycle == null) return;
            cycle.OnS1 += OnS1;
            cycle.OnS2 += OnS2;
        }

        private void OnDisable()
        {
            if (cycle != null)
            {
                cycle.OnS1 -= OnS1;
                cycle.OnS2 -= OnS2;
            }
            StopAll();
        }

        private void OnS1() => Buzz(amplitude);

        private void OnS2()
        {
            if (alsoBuzzS2) Buzz(s2Amplitude);
        }

        private void Buzz(float amp)
        {
            if (running != null) StopCoroutine(running);
            running = StartCoroutine(BuzzRoutine(amp));
        }

        private IEnumerator BuzzRoutine(float amp)
        {
            Set(frequency, amp);
            yield return new WaitForSeconds(durationMs / 1000f);
            Set(0f, 0f);
            running = null;
        }

        private void Set(float freq, float amp)
        {
            switch (target)
            {
                case Target.Both:
                    OVRInput.SetControllerVibration(freq, amp, OVRInput.Controller.LTouch);
                    OVRInput.SetControllerVibration(freq, amp, OVRInput.Controller.RTouch);
                    break;
                case Target.LeftHand:
                    OVRInput.SetControllerVibration(freq, amp, OVRInput.Controller.LTouch);
                    break;
                case Target.RightHand:
                    OVRInput.SetControllerVibration(freq, amp, OVRInput.Controller.RTouch);
                    break;
                case Target.Active:
                    OVRInput.SetControllerVibration(freq, amp, OVRInput.Controller.Active);
                    break;
            }
        }

        private void StopAll()
        {
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.LTouch);
            OVRInput.SetControllerVibration(0f, 0f, OVRInput.Controller.RTouch);
        }
    }
}