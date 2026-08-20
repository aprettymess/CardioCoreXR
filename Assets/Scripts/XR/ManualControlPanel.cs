using Heart;
using UnityEngine;
using UnityEngine.UI;

namespace XR
{
    public sealed class ManualControlPanel : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private HeartJointDriver driver;
        [SerializeField] private GameObject panelRoot;

        [Header("Controls")]
        [SerializeField] private Toggle manualToggle;
        [SerializeField] private Slider avCloseSlider;
        [SerializeField] private Slider slOpenSlider;
        [SerializeField] private Slider ventricleSlider;
        [SerializeField] private Slider atriaSlider;

        [Header("Summon")]
        [SerializeField] private OVRInput.Button toggleButton = OVRInput.Button.Two;
        [SerializeField] private bool startHidden = true;

        private bool visible;

        private void Start()
        {
            visible = !startHidden;
            if (panelRoot != null) panelRoot.SetActive(visible);
            WireEvents();
        }

        private void WireEvents()
        {
            if (manualToggle != null)
                manualToggle.onValueChanged.AddListener(OnManualToggled);
            if (avCloseSlider != null)
                avCloseSlider.onValueChanged.AddListener(v => { if (driver != null) driver.SetManualAvClose(v); });
            if (slOpenSlider != null)
                slOpenSlider.onValueChanged.AddListener(v => { if (driver != null) driver.SetManualSlOpen(v); });
            if (ventricleSlider != null)
                ventricleSlider.onValueChanged.AddListener(v => { if (driver != null) driver.SetManualVentricle(v); });
            if (atriaSlider != null)
                atriaSlider.onValueChanged.AddListener(v => { if (driver != null) driver.SetManualAtria(v); });
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

        private void OnManualToggled(bool on)
        {
            if (driver == null) return;
            driver.SetManualMode(on);
            if (on) PushAllSliders();
        }

        private void PushAllSliders()
        {
            if (avCloseSlider != null) driver.SetManualAvClose(avCloseSlider.value);
            if (slOpenSlider != null) driver.SetManualSlOpen(slOpenSlider.value);
            if (ventricleSlider != null) driver.SetManualVentricle(ventricleSlider.value);
            if (atriaSlider != null) driver.SetManualAtria(atriaSlider.value);
        }
    }
}