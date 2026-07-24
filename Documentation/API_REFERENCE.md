# BER PROD MIX - API Reference

## Core Managers

### AudioEngine
Central audio processing hub for playback and recording.

```csharp
// Singleton instance
AudioEngine engine = AudioEngine.Instance;

// Play audio
engine.PlayAudio(audioClip, volume: 1f);

// Recording
engine.StartRecording("filename.wav");
engine.StopRecording();

// Tempo management
engine.UpdateTempo(120f);
float tempo = engine.GetTempo();
float beatDuration = engine.GetBeatDuration();
```

### ProjectManager
Handles project lifecycle and file management.

```csharp
ProjectManager pm = ProjectManager.Instance;

// Create new project
pm.CreateProject("My Project", tempo: 120f);

// Save project
pm.SaveProject("path/to/project.berproj");

// Load project
pm.LoadProject("path/to/project.berproj");

// Track changes
pm.MarkDirty(); // Mark as modified
```

### SettingsManager
Global application settings.

```csharp
SettingsManager sm = SettingsManager.Instance;

// Audio settings
int sampleRate = sm.Audio.sampleRate;
float masterVolume = sm.Audio.masterVolume;

// UI settings
bool animations = sm.UI.enableAnimations;

// Save/Load
sm.SaveSettings();
sm.ResetToDefaults();
```

### FileManager
File I/O operations for projects and audio.

```csharp
// Get all projects
List<string> projects = FileManager.GetProjectFiles();

// Audio files
FileManager.SaveAudioFile(audioClip, "recording.wav");
AudioClip loaded = FileManager.LoadAudioFile("recording.wav");

// File management
FileManager.DeleteFile(filePath);
FileManager.ClearTempFiles();
```

## Audio Systems

### BeatMaker
Drum sequencing and playback engine.

```csharp
BeatMaker beatMaker = GetComponent<BeatMaker>();

// Playback control
beatMaker.Play();
beatMaker.Stop();

// Sequencing
beatMaker.ToggleStep(stepIndex);
beatMaker.TriggerSound(stepIndex);

// Tempo
beatMaker.SetTempo(120f);
beatMaker.SetDrumKit(kitIndex);

// Events
beatMaker.OnBeatStep += (step) => { /* Handle beat */ };
beatMaker.OnSoundTriggered += (clip) => { /* Handle sound */ };
```

### VocalStudio
Multi-track recording and playback.

```csharp
VocalStudio studio = GetComponent<VocalStudio>();

// Recording
studio.StartRecording();
studio.StopRecording();
studio.SaveRecording("path/to/recording.wav");

// Playback
studio.PlayRecording();
studio.StopPlayback();

// Monitoring
studio.SetInputMonitoring(true);
studio.SetMonitoringVolume(0.5f);

// Events
studio.OnRecordingStarted += () => { /* Handle */ };
studio.OnRecordingStopped += () => { /* Handle */ };
```

### MasterMixer
32-track mixing with routing and metering.

```csharp
MasterMixer mixer = GetComponent<MasterMixer>();

// Track control
mixer.SetTrackVolume(trackIndex, 0.8f);
mixer.SetTrackPan(trackIndex, -0.5f); // -1 to 1
mixer.MuteTrack(trackIndex, true);
mixer.SoloTrack(trackIndex, true);

// Master control
mixer.SetMasterVolume(0.9f);

// Metering
float[] meters = mixer.GetMeterReadings();

// Events
mixer.OnTrackVolumeChanged += (track, volume) => { /* Handle */ };
mixer.OnMetersUpdated += (meters) => { /* Update UI */ };
```

## Effects

### EQEffect
Parametric 4-band equalizer.

```csharp
EQEffect eq = GetComponent<EQEffect>();

// Band adjustment
eq.SetBandGain(bandIndex, gainDb);
eq.SetBandFrequency(bandIndex, frequency);
eq.SetBandQ(bandIndex, q);

// Control
eq.SetEnabled(true);
eq.Reset(); // Return to neutral
```

### CompressorEffect
Dynamic range compression.

```csharp
CompressorEffect comp = GetComponent<CompressorEffect>();

// Parameters
comp.SetThreshold(-20f);
comp.SetRatio(4f);
comp.SetAttack(10f);
comp.SetRelease(100f);
comp.SetMakeupGain(5f);

comp.SetEnabled(true);
```

### ReverbEffect
Convolution-based spatial reverb.

```csharp
ReverbEffect reverb = GetComponent<ReverbEffect>();

comp.SetMix(0.3f); // 30% wet
comp.SetDecayTime(2f);
comp.SetEnabled(true);
```

### DelayEffect
Configurable time-based delay.

```csharp
DelayEffect delay = GetComponent<DelayEffect>();

delay.SetDelayTime(500f); // 500ms
delay.SetFeedback(0.5f);
delay.SetMix(0.3f);
delay.Clear(); // Reset buffer
```

## UI Components

### ChamberController
Chamber/tab navigation system.

```csharp
ChamberController controller = GetComponent<ChamberController>();

// Switch chambers
controller.SetActiveChamber(0); // Beat Maker
controller.SetActiveChamber(1); // Vocal Studio
controller.SetActiveChamber(2); // Master Mixer

int active = controller.ActiveChamber;
```

### BeatPadUI
Interactive beat pad with animations.

```csharp
BeatPadUI pad = GetComponent<BeatPadUI>();

// Setup
pad.Initialize(padIndex);

// State
pad.SetIdle();
pad.SetActive(Constants.COLOR_NEON_GREEN);

// Animation
pad.PlayAnimation();
pad.PlayPulse();

// Events
pad.OnPadPressed += (index) => { /* Handle press */ };
```

### FaderUI
Volume/parameter fader control.

```csharp
FaderUI fader = GetComponent<FaderUI>();

// Set value
fader.SetValue(0.75f);
fader.AnimateTo(0.5f, duration: 0.5f);

// Styling
fader.SetGlowColor(Constants.COLOR_NEON_BLUE);

// Query
float value = fader.Value;
float normalized = fader.NormalizedValue;

// Events
fader.OnValueChanged += (value) => { /* Handle change */ };
```

### VUMeterUI
Real-time audio level metering.

```csharp
VUMeterUI meter = GetComponent<VUMeterUI>();

// Update display
meter.SetLevel(0.7f); // 0-1 normalized

// Query state
float level = meter.CurrentLevel;
float peak = meter.PeakLevel;
```

### WaveformVisualizer
Real-time frequency spectrum display.

```csharp
WaveformVisualizer viz = GetComponent<WaveformVisualizer>();

// Styling
viz.SetFrequencyColors(lowColor, midColor, highColor);

// Query
float[] spectrum = viz.GetSpectrum();
```

## Constants

All color and parameter constants are defined in `Constants.cs`:

```csharp
// Colors
Constants.COLOR_BACKGROUND_DARK
Constants.COLOR_ACCENT_GOLD
Constants.COLOR_NEON_GREEN
Constants.COLOR_NEON_BLUE
Constants.COLOR_NEON_RED
Constants.COLOR_NEON_YELLOW

// Audio
Constants.SAMPLE_RATE = 48000
Constants.BUFFER_SIZE = 256
Constants.MAX_TRACKS = 32
Constants.DEFAULT_TEMPO = 120f

// UI
Constants.BEAT_PAD_SIZE = 80
Constants.BEAT_PAD_GRID_SIZE = 4
Constants.TRANSITION_DURATION = 0.3f
```

---

**Last Updated**: 2026-07-24
**Version**: 1.0
