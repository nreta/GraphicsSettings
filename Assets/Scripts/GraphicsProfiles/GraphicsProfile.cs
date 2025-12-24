using System.Diagnostics;
using System.IO;
using UnityEngine;

[System.Serializable]
public class GraphicsProfile
{
    public string profileName;

    public int qualityLevel;
    public int pixelLightCount;
    public int shadowResolution;   // 0 = Low, 1 = Medium, 2 = High, 3 = VeryHigh
    public int antiAliasing;       // 0, 2, 4, 8
    public bool softParticles;
    public float shadowDistance;
    public float lodBias;
    public int textureQuality;

    // Load profile from JSON file
    public static GraphicsProfile LoadFromJson(string filePath)
    {
        if (!File.Exists(filePath))
        {
            UnityEngine.Debug.LogError($"GraphicsProfile JSON not found: {filePath}");
            return null;
        }

        string json = File.ReadAllText(filePath);
        return JsonUtility.FromJson<GraphicsProfile>(json);
    }

    // Apply settings in Unity
    public void ApplyProfile()
    {
        QualitySettings.SetQualityLevel(qualityLevel, true);
        QualitySettings.pixelLightCount = pixelLightCount;

        // Shadow quality (enable / disable)
        QualitySettings.shadows = shadowResolution >= 0 ? ShadowQuality.All : ShadowQuality.Disable;

        // Shadow resolution (use enum)
        if (shadowResolution >= 0 && shadowResolution <= 3)
            QualitySettings.shadowResolution = (ShadowResolution)shadowResolution;
        else
            UnityEngine.Debug.LogWarning("Invalid shadowResolution value in " + profileName);

        QualitySettings.antiAliasing = antiAliasing;
        QualitySettings.softParticles = softParticles;
        QualitySettings.shadowDistance = shadowDistance;
        QualitySettings.lodBias = lodBias;
        QualitySettings.globalTextureMipmapLimit = textureQuality;


        UnityEngine.Debug.Log($"Applied Graphics Profile: {profileName}");
    }
}
