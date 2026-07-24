using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using BerProdMix.Utils;

namespace BerProdMix.UI
{
    public class BeatPadUI : MonoBehaviour
    {
        [SerializeField] private Image _padImage;
        [SerializeField] private Image _glowImage;
        [SerializeField] private Button _padButton;
        [SerializeField] private Color _idleColor = new Color(0.2f, 0.2f, 0.2f);
        [SerializeField] private Color _activeColor = Constants.COLOR_NEON_GREEN;
        [SerializeField] private float _glowIntensity = 2f;
        [SerializeField] private float _animationDuration = 0.1f;

        private int _padIndex;
        private bool _isActive = false;
        private CanvasGroup _canvasGroup;
        private RectTransform _rectTransform;
        private Vector3 _originalScale;

        public System.Action<int> OnPadPressed;

        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
            if (_canvasGroup == null) _canvasGroup = gameObject.AddComponent<CanvasGroup>();
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
            if (_padButton != null) _padButton.onClick.AddListener(OnPadClick);
            SetIdle();
        }

        public void Initialize(int index) => _padIndex = index;

        private void OnPadClick() { PlayAnimation(); OnPadPressed?.Invoke(_padIndex); }

        public void SetIdle()
        {
            _isActive = false;
            if (_padImage != null) _padImage.color = _idleColor;
            if (_glowImage != null) _glowImage.color = new Color(_idleColor.r, _idleColor.g, _idleColor.b, 0.3f);
        }

        public void SetActive(Color glowColor)
        {
            _isActive = true;
            if (_padImage != null) _padImage.color = glowColor;
            if (_glowImage != null) _glowImage.color = new Color(glowColor.r, glowColor.g, glowColor.b, _glowIntensity);
        }

        public void PlayAnimation() { StopAllCoroutines(); StartCoroutine(AnimatePadPress()); }

        private IEnumerator AnimatePadPress()
        {
            float elapsedTime = 0f;
            while (elapsedTime < _animationDuration / 2f) { elapsedTime += Time.deltaTime; float scale = Mathf.Lerp(1f, 1.2f, elapsedTime / (_animationDuration / 2f)); _rectTransform.localScale = _originalScale * scale; yield return null; }
            elapsedTime = 0f;
            while (elapsedTime < _animationDuration / 2f) { elapsedTime += Time.deltaTime; float scale = Mathf.Lerp(1.2f, 1f, elapsedTime / (_animationDuration / 2f)); _rectTransform.localScale = _originalScale * scale; yield return null; }
            _rectTransform.localScale = _originalScale;
        }

        public void PlayPulse() { StopAllCoroutines(); StartCoroutine(PulseEffect()); }

        private IEnumerator PulseEffect()
        {
            Color originalColor = _padImage.color;
            float elapsedTime = 0f;
            float pulseDuration = 0.2f;
            while (elapsedTime < pulseDuration) { elapsedTime += Time.deltaTime; float brightness = Mathf.Lerp(1.5f, 1f, elapsedTime / pulseDuration); _padImage.color = originalColor * brightness; yield return null; }
            _padImage.color = originalColor;
        }

        public int PadIndex => _padIndex;
        public bool IsActive => _isActive;
    }
}
