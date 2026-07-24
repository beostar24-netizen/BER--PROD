using UnityEngine;
using System.IO;
using System.Collections.Generic;
using BerProdMix.Utils;

namespace BerProdMix.Managers
{
    public class FileManager : MonoBehaviour
    {
        private static string _projectsPath;
        private static string _audioPath;
        private static string _tempPath;
        private static FileManager _instance;

        public static FileManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<FileManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("FileManager");
                        _instance = go.AddComponent<FileManager>();
                        DontDestroyOnLoad(go);
                    }
                }
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null) { _instance = this; DontDestroyOnLoad(gameObject); InitializePaths(); }
            else if (_instance != this) Destroy(gameObject);
        }

        private static void InitializePaths()
        {
            _projectsPath = Path.Combine(Application.persistentDataPath, "Projects");
            _audioPath = Path.Combine(Application.persistentDataPath, "Audio");
            _tempPath = Path.Combine(Application.persistentDataPath, "Temp");
            Directory.CreateDirectory(_projectsPath);
            Directory.CreateDirectory(_audioPath);
            Directory.CreateDirectory(_tempPath);
        }

        public static List<string> GetProjectFiles()
        {
            List<string> projects = new List<string>();
            try
            {
                DirectoryInfo dir = new DirectoryInfo(_projectsPath);
                FileInfo[] files = dir.GetFiles($"*{Constants.PROJECT_EXTENSION}");
                foreach (FileInfo file in files) projects.Add(file.FullName);
            }
            catch { }
            return projects;
        }

        public static bool SaveAudioFile(AudioClip clip, string filename)
        {
            if (clip == null) return false;
            try { string path = Path.Combine(_audioPath, filename); return true; }
            catch { return false; }
        }

        public static bool DeleteFile(string filePath)
        {
            try { if (File.Exists(filePath)) { File.Delete(filePath); return true; } }
            catch { }
            return false;
        }

        public static void ClearTempFiles()
        {
            try { DirectoryInfo dir = new DirectoryInfo(_tempPath); foreach (FileInfo f in dir.GetFiles()) f.Delete(); }
            catch { }
        }

        public static string ProjectsPath => _projectsPath;
        public static string AudioPath => _audioPath;
        public static string TempPath => _tempPath;
    }
}
