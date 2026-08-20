using UnityEngine;

namespace XR
{
    public sealed class UiPanelAnchor : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform headAnchor;

        [Header("Placement")]
        [SerializeField] private float distance = 0.6f;
        [SerializeField] private float sideOffset = 0.35f;
        [SerializeField] private float height = -0.15f;
        [SerializeField] private float facingYaw = 0f;

        [Header("Follow")]
        [SerializeField, Range(0f, 5f)] private float followLerp = 2f;
        [SerializeField, Range(0f, 0.6f)] private float deadZone = 0.2f;

        private void Start()
        {
            ResolveHead();
            transform.position = TargetPosition();
            transform.rotation = FaceRotation();
        }

        private void Update()
        {
            if (headAnchor == null) return;
            Vector3 target = TargetPosition();
            if (Vector3.Distance(transform.position, target) > deadZone)
                transform.position = Vector3.Lerp(transform.position, target, followLerp * Time.deltaTime);
            transform.rotation = FaceRotation();
        }

        private Vector3 TargetPosition()
        {
            Vector3 flat = headAnchor.forward; flat.y = 0f;
            if (flat.sqrMagnitude < 1e-4f) flat = Vector3.forward;
            flat.Normalize();
            Vector3 right = Vector3.Cross(Vector3.up, flat);
            return headAnchor.position + flat * distance + right * sideOffset + Vector3.up * height;
        }

        private Quaternion FaceRotation()
        {
            Vector3 flat = headAnchor.forward; flat.y = 0f;
            if (flat.sqrMagnitude < 1e-4f) flat = Vector3.forward;
            return Quaternion.LookRotation(flat.normalized) * Quaternion.Euler(0f, facingYaw, 0f);
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