using UnityEngine;

namespace XR
{
    public sealed class PanelSummoner : MonoBehaviour
    {
        [Header("Panels")]
        [SerializeField] private GameObject manualPanel;
        [SerializeField] private GameObject injectPanel;

        [Header("Summon Buttons")]
        [SerializeField] private OVRInput.Button manualButton = OVRInput.Button.Two;
        [SerializeField] private OVRInput.Button injectButton = OVRInput.Button.Four;

        [Header("Startup")]
        [SerializeField] private bool hideOnStart = true;

        private void Start()
        {
            if (!hideOnStart) return;
            if (manualPanel != null) manualPanel.SetActive(false);
            if (injectPanel != null) injectPanel.SetActive(false);
        }

        private void Update()
        {
            if (OVRInput.GetDown(manualButton) && manualPanel != null)
                manualPanel.SetActive(!manualPanel.activeSelf);
            if (OVRInput.GetDown(injectButton) && injectPanel != null)
                injectPanel.SetActive(!injectPanel.activeSelf);
        }
    }
}
