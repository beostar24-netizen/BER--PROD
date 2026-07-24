using UnityEngine;
using BerProdMix.Utils;

namespace BerProdMix.Effects
{
    public class DelayEffect : MonoBehaviour
    {
        [SerializeField] private float _delayTimeMs = 500f;
        [SerializeField] private float _feedback = 0.5f;
        [SerializeField] private float _wetDryMix = 0.3f;
        [SerializeField] private bool _enabled = true;

        private float[] _delayBuffer;
        private int _delayBufferIndex = 0;
        private int _delayBufferLength;

        private void Awake()
        {
            float maxDelaySeconds = 5f;
            _delayBufferLength = Mathf.RoundToInt(maxDelaySeconds * Constants.SAMPLE_RATE);
            _delayBuffer = new float[_delayBufferLength];
        }

        public void SetDelayTime(float delayMs) => _delayTimeMs = Mathf.Clamp(delayMs, 1f, 5000f);
        public void SetFeedback(float feedback) => _feedback = Mathf.Clamp01(feedback);
        public void SetMix(float wetDryMix) => _wetDryMix = Mathf.Clamp01(wetDryMix);
        public void SetEnabled(bool enabled) => _enabled = enabled;

        public float ProcessSample(float sample)
        {
            if (!_enabled) return sample;
            int delaySamples = Mathf.RoundToInt((_delayTimeMs / 1000f) * Constants.SAMPLE_RATE);
            int delayIndex = (_delayBufferIndex - delaySamples + _delayBufferLength) % _delayBufferLength;
            float delayedSample = _delayBuffer[delayIndex];
            _delayBuffer[_delayBufferIndex] = sample + (delayedSample * _feedback);
            _delayBufferIndex = (_delayBufferIndex + 1) % _delayBufferLength;
            return Mathf.Lerp(sample, delayedSample, _wetDryMix);
        }

        public void Clear() { for (int i = 0; i < _delayBuffer.Length; i++) _delayBuffer[i] = 0f; _delayBufferIndex = 0; }
        public float DelayTimeMs => _delayTimeMs;
        public float Feedback => _feedback;
        public bool IsEnabled => _enabled;
    }
}
