#if UNITY_CLOUD_BUILD
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using UnityEditor;

public class CloudBuildProcessor : MonoBehaviour
{
    public static void PreExport(UnityEngine.CloudBuild.BuildManifestObject manifest)
    {
        string buildNum = manifest.GetValue<string>("buildNumber");
        Debug.LogError("PREBUILD Script launched, build number is " + buildNum);

        string[] versionSplit = PlayerSettings.bundleVersion.Split('.');
        int smallVersion = Int32.Parse(versionSplit[2]) + 1;

        PlayerSettings.Android.bundleVersionCode = Convert.ToInt32(Environment.GetEnvironmentVariable("MINIMUM_VERSION")) + Convert.ToInt32(manifest.GetValue<string>("buildNumber"));

        PlayerSettings.iOS.buildNumber = Convert.ToInt32(Environment.GetEnvironmentVariable("MINIMUM_VERSION")) + manifest.GetValue<string>("buildNumber");

        string buildEnv = "";
        buildEnv += "BUILD_ENVIRONMENT:" +Environment.GetEnvironmentVariable("BUILD_ENVIRONMENT") + "\n";
        Debug.LogError("Current enviroment " + buildEnv);
        buildEnv += "BUNDLE_VERSION_ANDROID:" + PlayerSettings.Android.bundleVersionCode;
        buildEnv += "BUNDLE_VERSION_IOS:" + PlayerSettings.iOS.buildNumber;
        File.WriteAllText("Assets/Resources/PreBuildConfig.txt", buildEnv);
    }
}
#endif