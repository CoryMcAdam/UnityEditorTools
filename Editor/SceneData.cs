using System;
using UnityEngine.SceneManagement;

namespace CMDev.EditorTools.Editor
{
    [Serializable]
    public class SceneData
    {
        public string Name;
        public string Path;

        public SceneData(Scene scene)
        {
            Name = scene.name;
            Path = scene.path;
        }

        public SceneData(string path)
        {
            Name = System.IO.Path.GetFileNameWithoutExtension(path);
            Path = path;
        }
    }
}