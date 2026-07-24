using UnityEngine;
using UnityEngine.UI;
using BerProdMix.Utils;

namespace BerProdMix.UI
{
    public class VUMeterUI : MonoBehaviour
    {
        [SerializeField] private Image _meterFill;
        [SerializeField] private Image _peakIndicator;
        [SerializeField] private Text _dbLabel;
        [SerializeField] private Color _normalColor = Constants.COLOR_NEON_GREEN;
        [SerializeField] private Color _warningColor = Constants.COLOR_NEON_YELLOW;
        [SerializeField] private Color _peakColor = Constants.COLOR_NEON_RED;
        [SerializeField] private float _normalThreshold = 0.7f;
        [SerializeField] private float _warningThreshold = 0.85f;
        [SerializeField] private float _peakHoldDuration = 2f;
        [SerializeField] private float _responseTime = 0.05f;

        private float _currentLevel = 0f;
        private float _peakLevel = 0f;
        private float _peakHoldTimer = 0f;

        private void Update() { if (_peakHoldTimer > 0) { _peakHoldTimer -= Time.deltaTime; if (_peakHoldTimer <= 0) _peakLevel = _currentLevel; } }

        public void SetLevel(float level)
        {
            level = Mathf.Clamp01(level);
            _currentLevel = Mathf.Lerp(_currentLevel, level, _responseTime);
            if (level > _peakLevel) { _peakLevel = level; _peakHoldTimer = _peakHoldDuration; }
            UpdateVisuals();
        }

        private void UpdateVisuals()
        {
            if (_meterFill != null) { _meterFill.fillAmount = _currentLevel; _meterFill.color = GetColorForLevel(_currentLevel); }
            if (_peakIndicator != null)
            {
                _peakIndicator.color = GetColorForLevel(_peakLevel);
                RectTransform peakRect = _peakIndicator.GetComponent<RectTransform>();
                if (peakRect != null) peakRect.anchoredPosition = new Vector2(_peakLevel * 100f, peakRect.anchoredPosition.y);
            }
            if (_dbLabel != null) { float db = NormalizedToDb(_currentLevel); _dbLabel.text = $"{db:F1} dB"; }
        }

        private Color GetColorForLevel(float level)
        {
            if (level < _normalThreshold) return _normalColor;
            else if (level < _warningThreshold) return Color.Lerp(_normalColor, _warningColor, (level - _normalThreshold) / (_warningThreshold - _normalThreshold));
            else return Color.Lerp(_warningColor, _peakColor, (level - _warningThreshold) / (1f - _warningThreshold));
        }

        private float NormalizedToDb(float normalized) { if (normalized <= 0) return -80f; return 20f * Mathf.Log10(normalized); }

        public float CurrentLevel => _currentLevel;
        public float PeakLevel => _peakLevel;
    }
}
