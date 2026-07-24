using UnityEngine;
using BerProdMix.Utils;

namespace BerProdMix.Effects
{
    public class CompressorEffect : MonoBehaviour
    {
        [SerializeField] private float _threshold = -20f;
        [SerializeField] private float _ratio = 4f;
        [SerializeField] private float _attackMs = 10f;
        [SerializeField] private float _releaseMs = 100f;
        [SerializeField] private float _makeupGainDb = 0f;
        [SerializeField] private bool _enabled = true;

        private float _gainReduction = 1f;
        public System.Action<float> OnGainReductionChanged;

        public void SetThreshold(float thresholdDb) => _threshold = Mathf.Clamp(thresholdDb, -80f, 0f);
        public void SetRatio(float ratio) => _ratio = Mathf.Clamp(ratio, Constants.MIN_COMPRESSION_RATIO, Constants.MAX_COMPRESSION_RATIO);
        public void SetAttack(float attackMs) => _attackMs = Mathf.Clamp(attackMs, 0.1f, 100f);
        public void SetRelease(float releaseMs) => _releaseMs = Mathf.Clamp(releaseMs, 10f, 2000f);
        public void SetMakeupGain(float gainDb) => _makeupGainDb = Mathf.Clamp(gainDb, Constants.MIN_GAIN_DB, Constants.MAX_GAIN_DB);
        public void SetEnabled(bool enabled) => _enabled = enabled;

        public float ProcessSample(float sample)
        {
            if (!_enabled) return sample;
            float inputDb = 20f * Mathf.Log10(Mathf.Abs(sample) + 0.0001f);
            if (inputDb > _threshold)
                _gainReduction = Mathf.Pow(10f, -((inputDb - _threshold) * (1f - (1f / _ratio))) / 20f);
            else
                _gainReduction = 1f;
            OnGainReductionChanged?.Invoke(_gainReduction);
            float makeupGainLinear = Mathf.Pow(10f, _makeupGainDb / 20f);
            return sample * _gainReduction * makeupGainLinear;
        }

        public float Threshold => _threshold;
        public float Ratio => _ratio;
        public float GainReduction => _gainReduction;
        public bool IsEnabled => _enabled;
    }
}
