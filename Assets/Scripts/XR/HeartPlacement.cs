using UnityEngine;

namespace XR
{
    public sealed class HeartPlacement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform headCamera;

        [Header("Spawn Pose")]
        [SerializeField] private float distance = 0.6f;
        [SerializeField] private float verticalOffset = -0.1f;
        [SerializeField] private float facingYawOffset = 0f;
        [SerializeField] private float defaultScale = 1f;

        [Header("Reset Input")]
        [SerializeField] private OVRInput.Button resetButton = OVRInput.Button.One;
        [SerializeField] private bool editorKeyboardReset = true;

        private void Start()
        {
            ResolveCamera();
            PlaceInFront();
        }

        private void Update()
        {
            if (OVRInput.GetDown(resetButton)) PlaceInFront();
#if UNITY_EDITOR
            if (editorKeyboardReset && Input.GetKeyDown(KeyCode.R)) PlaceInFront();
#endif
        }

        public void PlaceInFront()
        {
            ResolveCamera();
            if (headCamera == null) { Debug.LogWarning("HeartPlacement: no head camera found."); return; }

            Vector3 flatForward = headCamera.forward;
            flatForward.y = 0f;
            if (flatForward.sqrMagnitude < 1e-4f) flatForward = Vector3.forward;
            flatForward.Normalize();

            transform.position = headCamera.position + flatForward * distance + Vector3.up * verticalOffset;
            transform.rotation = Quaternion.LookRotation(flatForward) * Quaternion.Euler(0f, facingYawOffset, 0f);
            transform.localScale = Vector3.one * defaultScale;
        }

        private void ResolveCamera()
        {
            if (headCamera == null && Camera.main != null) headCamera = Camera.main.transform;
        }
    }
}