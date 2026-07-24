using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BerProdMix.Utils;

namespace BerProdMix.UI
{
    public class FaderUI : MonoBehaviour
    {
        [SerializeField] private Slider _faderSlider;
        [SerializeField] private Image _faderHandle;
        [SerializeField] private Text _valueLabel;
        [SerializeField] private Color _glowColor = Constants.COLOR_NEON_BLUE;
        [SerializeField] private float _minValue = 0f;
        [SerializeField] private float _maxValue = 100f;

        private float _currentValue;
        public System.Action<float> OnValueChanged;

        private void Awake()
        {
            if (_faderSlider == null) _faderSlider = GetComponent<Slider>();
            _faderHandle = _faderSlider.handleRect.GetComponent<Image>();
            _faderSlider.minValue = _minValue;
            _faderSlider.maxValue = _maxValue;
            _faderSlider.onValueChanged.AddListener(OnFaderValueChanged);
            UpdateVisuals();
        }

        private void OnFaderValueChanged(float value) { _currentValue = value; UpdateVisuals(); OnValueChanged?.Invoke(value); }

        private void UpdateVisuals()
        {
            if (_valueLabel != null) _valueLabel.text = _currentValue.ToString("F1");
            if (_faderHandle != null)
            {
                float normalizedValue = (_currentValue - _minValue) / (_maxValue - _minValue);
                Color glowColor = Color.Lerp(new Color(0.3f, 0.3f, 0.3f), _glowColor, normalizedValue);
                _faderHandle.color = glowColor;
            }
        }

        public void SetValue(float value) => _faderSlider.value = Mathf.Clamp(value, _minValue, _maxValue);
        public void AnimateTo(float targetValue, float duration = 0.5f) { StopAllCoroutines(); StartCoroutine(AnimateFaderCoroutine(targetValue, duration)); }

        private IEnumerator AnimateFaderCoroutine(float targetValue, float duration)
        {
            float startValue = _currentValue;
            float elapsedTime = 0f;
            while (elapsedTime < duration) { elapsedTime += Time.deltaTime; float progress = Mathf.Clamp01(elapsedTime / duration); float easeValue = Mathf.SmoothStep(0f, 1f, progress); float value = Mathf.Lerp(startValue, targetValue, easeValue); SetValue(value); yield return null; }
            SetValue(targetValue);
        }

        public void SetGlowColor(Color color) { _glowColor = color; UpdateVisuals(); }
        public float Value => _currentValue;
        public float NormalizedValue => (_currentValue - _minValue) / (_maxValue - _minValue);
    }
}
