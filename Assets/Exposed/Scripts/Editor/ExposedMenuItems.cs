using System.IO;
using Exposed.Utils;
using UnityEditor;
using UnityEngine;

namespace Exposed.Editor
{
    public class ExposedMenuItems
    {
        [MenuItem("Assets/Create/Exposed/Configuration", false, 100)]
        public static void NewConfig()
        {
            ExposedLogger.Info("Creating new nest configuration");
            CreateAsset<ExposedConfiguration>("ExposedConfiguration.asset");
        }

        private static void CreateAsset<T>(string name) where T : ScriptableObject
        {
            var asset = ScriptableObject.CreateInstance<T>();
            //AssetDatabase.CreateAsset(asset, path);
            string path = string.Format("{0}/{1}", GetDirectoryPathForCurrentSelection(), name);
            ProjectWindowUtil.CreateAsset(asset, path);
            AssetDatabase.SaveAssets();
            //Selection.activeInstanceID = asset.GetInstanceID();
        }

        public static string GetDirectoryPathForCurrentSelection()
        {
            string path = "Assets";
            foreach (var obj in Selection.GetFiltered(typeof(Object), SelectionMode.Assets))
            {
                path = AssetDatabase.GetAssetPath(obj);
                if (File.Exists(path))
                {
                    path = Path.GetDirectoryName(path);
                }
                break;
            }
            return path;
        }
         
    }
}