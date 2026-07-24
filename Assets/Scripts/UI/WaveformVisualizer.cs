using UnityEngine;
using UnityEngine.UI;
using BerProdMix.Utils;

namespace BerProdMix.UI
{
    public class WaveformVisualizer : MonoBehaviour
    {
        [SerializeField] private Image _visualizerImage;
        [SerializeField] private int _barCount = 64;
        [SerializeField] private Color _minColor = Constants.COLOR_NEON_GREEN;
        [SerializeField] private Color _midColor = Constants.COLOR_NEON_BLUE;
        [SerializeField] private Color _maxColor = Constants.COLOR_NEON_RED;
        [SerializeField] private float _smoothing = 0.1f;
        [SerializeField] private float _multiplier = 2f;

        private float[] _spectrum;
        private float[] _smoothedSpectrum;
        private float[] _displaySpectrum;
        private Texture2D _visualizerTexture;
        private Color[] _textureData;

        private void Awake() { InitializeVisualizer(); }

        private void InitializeVisualizer()
        {
            _spectrum = new float[_barCount];
            _smoothedSpectrum = new float[_barCount];
            _displaySpectrum = new float[_barCount];
            _visualizerTexture = new Texture2D(_barCount, 128, TextureFormat.RGB24, false);
            _textureData = new Color[_barCount * 128];
            if (_visualizerImage != null) _visualizerImage.texture = _visualizerTexture;
        }

        private void Update() { UpdateSpectrum(); RenderWaveform(); }

        private void UpdateSpectrum()
        {
            int totalSources = 0;
            float[] tempSpectrum = new float[_barCount];
            AudioSource[] audioSources = FindObjectsOfType<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                if (source.isPlaying)
                {
                    AudioListener.GetSpectrumData(tempSpectrum, 0, FFTWindow.BlackmanHarris);
                    for (int i = 0; i < _barCount; i++) _spectrum[i] += tempSpectrum[i];
                    totalSources++;
                }
            }
            if (totalSources > 0)
            {
                for (int i = 0; i < _barCount; i++)
                {
                    _spectrum[i] /= totalSources;
                    _smoothedSpectrum[i] = Mathf.Lerp(_smoothedSpectrum[i], _spectrum[i], _smoothing);
                    _displaySpectrum[i] = _smoothedSpectrum[i] * _multiplier;
                }
            }
        }

        private void RenderWaveform()
        {
            for (int i = 0; i < _textureData.Length; i++) _textureData[i] = Constants.COLOR_BACKGROUND_DARK;
            for (int x = 0; x < _barCount; x++)
            {
                int barHeight = Mathf.RoundToInt(_displaySpectrum[x] * 128f);
                Color barColor = GetColorForFrequency(x, _barCount);
                for (int y = 0; y < barHeight && y < 128; y++)
                {
                    int pixelIndex = y * _barCount + x;
                    if (pixelIndex < _textureData.Length) _textureData[pixelIndex] = barColor;
                }
            }
            _visualizerTexture.SetPixels(_textureData);
            _visualizerTexture.Apply();
        }

        private Color GetColorForFrequency(int index, int total)
        {
            float normalizedFreq = (float)index / total;
            if (normalizedFreq < 0.33f) return Color.Lerp(_minColor, _midColor, normalizedFreq * 3f);
            else if (normalizedFreq < 0.66f) return Color.Lerp(_midColor, _maxColor, (normalizedFreq - 0.33f) * 3f);
            else return _maxColor;
        }

        public void SetFrequencyColors(Color low, Color mid, Color high) { _minColor = low; _midColor = mid; _maxColor = high; }
        public float[] GetSpectrum() => _displaySpectrum;
    }
}
