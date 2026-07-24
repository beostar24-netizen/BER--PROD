using UnityEngine;
using BerProdMix.Utils;

namespace BerProdMix.Managers
{
    public class SettingsManager : MonoBehaviour
    {
        [System.Serializable]
        public class AudioSettings
        {
            public int sampleRate = Constants.SAMPLE_RATE;
            public int bufferSize = Constants.BUFFER_SIZE;
            public float masterVolume = 1f;
        }

        [System.Serializable]
        public class UISettings
        {
            public bool enableAnimations = true;
            public float animationSpeed = 1f;
            public bool enableGlowEffects = true;
        }

        [SerializeField] private AudioSettings _audioSettings;
        [SerializeField] private UISettings _uiSettings;
        private static SettingsManager _instance;

        public static SettingsManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<SettingsManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("SettingsManager");
                        _instance = go.AddComponent<SettingsManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); LoadSettings(); }
            else if (_instance != this) Destroy(gameObject);
        }

        private void LoadSettings()
        {
            _audioSettings = new AudioSettings
            {
                sampleRate = PlayerPrefs.GetInt("AudioSettings.SampleRate", Constants.SAMPLE_RATE),
                bufferSize = PlayerPrefs.GetInt("AudioSettings.BufferSize", Constants.BUFFER_SIZE),
                masterVolume = PlayerPrefs.GetFloat("AudioSettings.MasterVolume", 1f)
            };

            _uiSettings = new UISettings
            {
                enableAnimations = PlayerPrefs.GetInt("UISettings.EnableAnimations", 1) == 1,
                animationSpeed = PlayerPrefs.GetFloat("UISettings.AnimationSpeed", 1f),
                enableGlowEffects = PlayerPrefs.GetInt("UISettings.EnableGlowEffects", 1) == 1
            };
        }

        public void SaveSettings()
        {
            PlayerPrefs.SetInt("AudioSettings.SampleRate", _audioSettings.sampleRate);
            PlayerPrefs.SetInt("AudioSettings.BufferSize", _audioSettings.bufferSize);
            PlayerPrefs.SetFloat("AudioSettings.MasterVolume", _audioSettings.masterVolume);
            PlayerPrefs.SetInt("UISettings.EnableAnimations", _uiSettings.enableAnimations ? 1 : 0);
            PlayerPrefs.SetFloat("UISettings.AnimationSpeed", _uiSettings.animationSpeed);
            PlayerPrefs.SetInt("UISettings.EnableGlowEffects", _uiSettings.enableGlowEffects ? 1 : 0);
            PlayerPrefs.Save();
        }

        public void ResetToDefaults() { _audioSettings = new AudioSettings(); _uiSettings = new UISettings(); PlayerPrefs.DeleteAll(); SaveSettings(); }
        public AudioSettings Audio => _audioSettings;
        public UISettings UI => _uiSettings;
    }
}
