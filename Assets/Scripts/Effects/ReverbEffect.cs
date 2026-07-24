using UnityEngine;
using BerProdMix.Utils;

namespace BerProdMix.Effects
{
    public class ReverbEffect : MonoBehaviour
    {
        [SerializeField] private float _wetDryMix = 0.3f;
        [SerializeField] private float _decayTime = 2f;
        [SerializeField] private bool _enabled = true;

        public void SetMix(float wetDryMix) => _wetDryMix = Mathf.Clamp01(wetDryMix);
        public void SetDecayTime(float decaySeconds) => _decayTime = Mathf.Clamp(decaySeconds, 0.5f, 8f);
        public void SetEnabled(bool enabled) => _enabled = enabled;

        public float ProcessSample(float sample)
        {
            if (!_enabled) return sample;
            float wetSignal = sample * Mathf.Exp(-Time.deltaTime / _decayTime);
            return Mathf.Lerp(sample, wetSignal, _wetDryMix);
        }

        public float WetDryMix => _wetDryMix;
        public float DecayTime => _decayTime;
        public bool IsEnabled => _enabled;
    }
}
