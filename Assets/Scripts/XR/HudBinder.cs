using CardioCore;
using Core;
using TMPro;
using UnityEngine;

namespace XR
{
    public sealed class HudBinder : MonoBehaviour
    {
        [Header("Source")]
        [SerializeField] private DataClient client;

        [Header("Fields")]
        [SerializeField] private TMP_Text sourceText;
        [SerializeField] private TMP_Text bpmText;
        [SerializeField] private TMP_Text rhythmText;
        [SerializeField] private TMP_Text murmurText;
        [SerializeField] private GameObject leadOffBanner;

        [Header("Colors")]
        [SerializeField] private Color normalColor = new Color(0.85f, 0.9f, 0.95f);
        [SerializeField] private Color alertColor = new Color(0.95f, 0.35f, 0.3f);

        private void OnEnable()
        {
            if (client != null) client.OnTwinState += OnState;
        }

        private void OnDisable()
        {
            if (client != null) client.OnTwinState -= OnState;
        }

        private void OnState(TwinState s)
        {
            if (bpmText != null) bpmText.text = Mathf.RoundToInt(s.Bpm).ToString();

            if (rhythmText != null)
            {
                rhythmText.text = string.IsNullOrEmpty(s.RhythmLabel) ? "--" : s.RhythmLabel;
                rhythmText.color = (string.IsNullOrEmpty(s.RhythmLabel) || s.RhythmLabel == "normal")
                    ? normalColor : alertColor;
            }

            if (murmurText != null)
            {
                murmurText.text = string.IsNullOrEmpty(s.MurmurLabel) ? "--" : s.MurmurLabel;
                murmurText.color = (string.IsNullOrEmpty(s.MurmurLabel) || s.MurmurLabel == "none")
                    ? normalColor : alertColor;
            }

            if (sourceText != null)
                sourceText.text = client.UsingMock ? "MOCK" : (client.IsConnected ? "LIVE" : "NO SIGNAL");

            if (leadOffBanner != null) leadOffBanner.SetActive(s.LeadOff);
        }
    }
}