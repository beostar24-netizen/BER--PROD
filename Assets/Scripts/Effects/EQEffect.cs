using UnityEngine;
using BerProdMix.Utils;

namespace BerProdMix.Effects
{
    public class EQEffect : MonoBehaviour
    {
        [System.Serializable]
        public class EQBand
        {
            public string bandName;
            public float frequency = 1000f;
            public float gain = 0f;
            public float q = 1f;
        }

        [SerializeField] private EQBand[] _bands = new EQBand[4];
        [SerializeField] private bool _enabled = true;

        private void Awake()
        {
            InitializeEQBands();
        }

        private void InitializeEQBands()
        {
            if (_bands.Length == 0)
            {
                _bands = new EQBand[4];
                _bands[0] = new EQBand { bandName = "Low", frequency = 100f };
                _bands[1] = new EQBand { bandName = "Low-Mid", frequency = 400f };
                _bands[2] = new EQBand { bandName = "High-Mid", frequency = 2500f };
                _bands[3] = new EQBand { bandName = "High", frequency = 10000f };
            }
        }

        public void SetBandGain(int bandIndex, float gainDb)
        {
            if (bandIndex < 0 || bandIndex >= _bands.Length) return;
            _bands[bandIndex].gain = Mathf.Clamp(gainDb, Constants.MIN_GAIN_DB, Constants.MAX_GAIN_DB);
        }

        public void SetBandFrequency(int bandIndex, float frequency)
        {
            if (bandIndex < 0 || bandIndex >= _bands.Length) return;
            _bands[bandIndex].frequency = Mathf.Clamp(frequency, Constants.MIN_FREQUENCY, Constants.MAX_FREQUENCY);
        }

        public void SetBandQ(int bandIndex, float q)
        {
            if (bandIndex < 0 || bandIndex >= _bands.Length) return;
            _bands[bandIndex].q = Mathf.Clamp(q, 0.5f, 8f);
        }

        public void SetEnabled(bool enabled) => _enabled = enabled;
        public void Reset() { foreach (var b in _bands) b.gain = 0f; }

        public EQBand[] Bands => _bands;
        public bool IsEnabled => _enabled;
    }
}
