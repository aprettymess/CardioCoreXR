using Core;
using UnityEngine;
using UnityEngine.UI;

namespace XR
{
    public sealed class InjectPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private DataClient client;
        [SerializeField] private GameObject panelRoot;

        [Header("Buttons")]
        [SerializeField] private Button afibButton;
        [SerializeField] private Button murmurButton;
        [SerializeField] private Button normalButton;

        [Header("Summon")]
        [SerializeField] private OVRInput.Button toggleButton = OVRInput.Button.Four;
        [SerializeField] private bool startHidden = true;

        private bool visible;

        private void Start()
        {
            visible = !startHidden;
            if (panelRoot != null) panelRoot.SetActive(visible);
            if (afibButton != null) afibButton.onClick.AddListener(() => Inject("afib", null));
            if (murmurButton != null) murmurButton.onClick.AddListener(() => Inject(null, "systolic"));
            if (normalButton != null) normalButton.onClick.AddListener(() => Inject("normal", "none"));
        }

        private void Update()
        {
            if (OVRInput.GetDown(toggleButton)) ToggleVisible();
        }

        private void ToggleVisible()
        {
            visible = !visible;
            if (panelRoot != null) panelRoot.SetActive(visible);
        }

        private void Inject(string rhythm, string murmur)
        {
            if (client == null) return;
            if (rhythm != null) client.SetMockRhythm(rhythm);
            if (murmur != null) client.SetMockMurmur(murmur);
        }
    }
}
