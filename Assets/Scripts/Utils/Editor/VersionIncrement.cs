using System;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Utils.Editor
{
    public static class VersionIncrement 
    {
        [PostProcessBuild]
        public static void OnPostProcessBuild(BuildTarget target, string targetPath)
        {
            var currentVersion = UnityEditor.PlayerSettings.bundleVersion;
            
            int major = Convert.ToInt32 (currentVersion.Split ('.') [0]);
            int minor = Convert.ToInt32 (currentVersion.Split ('.') [1]);
            int build = Convert.ToInt32 (currentVersion.Split ('.') [2]) + 1;

            build = CheckMaxValue(build, ref minor, 9);
            minor = CheckMaxValue(minor, ref major, 9);
            
            currentVersion = major + "." + minor + "." + build;
            PlayerSettings.bundleVersion = currentVersion;

            Debug.Log(currentVersion);
        }

        private static int CheckMaxValue(int minor, ref int major, int value)
        {
            if (minor > value)
            {
                minor = 0;
                major++;
            }

            return minor;
        }
    }
}