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

        [Header("Manual Toggle")]
        [SerializeField] private Toggle manualToggle;

        [Header("Channel Sliders")]
        [SerializeField] private Slider avCloseSlider;
        [SerializeField] private Slider slOpenSlider;
        [SerializeField] private Slider ventricleSlider;
        [SerializeField] private Slider atriaSlider;

        [Header("Preset Buttons")]
        [SerializeField] private Button diastoleButton;
        [SerializeField] private Button systoleButton;
        [SerializeField] private Button resumeLiveButton;

        [Header("Summon")]
        [SerializeField] private OVRInput.Button toggleButton = OVRInput.Button.Two;
        [SerializeField] private bool startHidden = true;

        private bool visible;

        private void Start()
        {
            visible = !startHidden;
            if (panelRoot != null) panelRoot.SetActive(visible);
            if (manualToggle != null) manualToggle.onValueChanged.AddListener(OnManual);
            Bind(avCloseSlider, v => { if (driver != null) driver.SetManualAvClose(v); });
            Bind(slOpenSlider, v => { if (driver != null) driver.SetManualSlOpen(v); });
            Bind(ventricleSlider, v => { if (driver != null) driver.SetManualVentricle(v); });
            Bind(atriaSlider, v => { if (driver != null) driver.SetManualAtria(v); });
            if (diastoleButton != null) diastoleButton.onClick.AddListener(OnDiastole);
            if (systoleButton != null) systoleButton.onClick.AddListener(OnSystole);
            if (resumeLiveButton != null) resumeLiveButton.onClick.AddListener(OnResume);
        }

        private void Bind(Slider s, UnityEngine.Events.UnityAction<float> a)
        {
            if (s != null) s.onValueChanged.AddListener(a);
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

        private void OnManual(bool on)
        {
            if (driver == null) return;
            driver.SetManualMode(on);
            if (on) PushSliders();
        }

        private void PushSliders()
        {
            if (avCloseSlider != null) driver.SetManualAvClose(avCloseSlider.value);
            if (slOpenSlider != null) driver.SetManualSlOpen(slOpenSlider.value);
            if (ventricleSlider != null) driver.SetManualVentricle(ventricleSlider.value);
            if (atriaSlider != null) driver.SetManualAtria(atriaSlider.value);
        }

        private void OnDiastole() => SetSliders(0f, 0f, 0f, 0f);
        private void OnSystole() => SetSliders(1f, 1f, 1f, 0f);

        private void SetSliders(float av, float sl, float ven, float atr)
        {
            if (manualToggle != null) manualToggle.isOn = true;
            if (avCloseSlider != null) avCloseSlider.value = av;
            if (slOpenSlider != null) slOpenSlider.value = sl;
            if (ventricleSlider != null) ventricleSlider.value = ven;
            if (atriaSlider != null) atriaSlider.value = atr;
        }

        private void OnResume()
        {
            if (manualToggle != null) manualToggle.isOn = false;
            if (driver != null) driver.SetManualMode(false);
        }
    }
}
