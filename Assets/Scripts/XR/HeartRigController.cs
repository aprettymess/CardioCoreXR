using Oculus.Interaction;
using UnityEngine;

namespace XR
{
    public sealed class HeartRigController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform headAnchor;     
        [SerializeField] private Grabbable grabbable;        

        [Header("Spawn / Follow Pose")]
        [SerializeField] private float distance = 0.55f;
        [SerializeField] private float height = -0.1f;
        [SerializeField] private float spawnYaw = 180f;
        [SerializeField] private float defaultScale = 1f;

        [Header("Follow Feel")]
        [SerializeField] private bool followEnabled = true;
        [SerializeField, Range(0f, 5f)] private float followLerp = 1.5f;
        [SerializeField, Range(0f, 0.6f)] private float deadZone = 0.25f;

        [Header("Reset Input")]
        [SerializeField] private OVRInput.Button resetButton = OVRInput.Button.One;

        private bool isGrabbed;

        private void OnEnable()
        {
            if (grabbable != null) grabbable.WhenPointerEventRaised += OnPointer;
        }

        private void OnDisable()
        {
            if (grabbable != null) grabbable.WhenPointerEventRaised -= OnPointer;
        }

        private void Start()
        {
            ResolveHead();
            PlaceInFront();
        }

        private void OnPointer(PointerEvent evt)
        {
            if (evt.Type == PointerEventType.Select) isGrabbed = true;
            else if (evt.Type == PointerEventType.Unselect)
                isGrabbed = grabbable != null && grabbable.SelectingPointsCount > 0;
        }

        private void Update()
        {
            if (OVRInput.GetDown(resetButton)) PlaceInFront();

            if (!followEnabled || isGrabbed || headAnchor == null) return;

            Vector3 target = TargetPosition();
            if (Vector3.Distance(transform.position, target) > deadZone)
                transform.position = Vector3.Lerp(transform.position, target, followLerp * Time.deltaTime);
        }

        public void PlaceInFront()
        {
            ResolveHead();
            if (headAnchor == null) { Debug.LogWarning("HeartRigController: no head anchor."); return; }

            transform.position = TargetPosition();

            Vector3 flat = headAnchor.forward; flat.y = 0f;
            if (flat.sqrMagnitude < 1e-4f) flat = Vector3.forward;
            transform.rotation = Quaternion.LookRotation(flat.normalized) * Quaternion.Euler(0f, spawnYaw, 0f);
            transform.localScale = Vector3.one * defaultScale;
        }

        private Vector3 TargetPosition()
        {
            Vector3 flat = headAnchor.forward; flat.y = 0f;
            if (flat.sqrMagnitude < 1e-4f) flat = Vector3.forward;
            flat.Normalize();
            return headAnchor.position + flat * distance + Vector3.up * height;
        }

        private void ResolveHead()
        {
            if (headAnchor != null) return;
            GameObject go = GameObject.Find("CenterEyeAnchor");
            if (go != null) headAnchor = go.transform;
            else if (Camera.main != null) headAnchor = Camera.main.transform;
        }
    }
}