using System.Diagnostics;
using System.IO;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class QualitySwitcher : MonoBehaviour
{
    public string[] profileFiles =
 {
    Path.Combine(UnityEngine.Application.streamingAssetsPath, "Config/GraphicsLow.json"),
    Path.Combine(UnityEngine.Application.streamingAssetsPath, "Config/GraphicsMedium.json"),
    Path.Combine(UnityEngine.Application.streamingAssetsPath, "Config/GraphicsHigh.json"),
    Path.Combine(UnityEngine.Application.streamingAssetsPath, "Config/GraphicsUltra.json")
};


    public int CurrentProfileIndex { get; private set; }

    private GraphicsProfile[] profiles;
    private GraphicsProfile currentProfile;

    void Awake()
    {
        LoadProfiles();
        ApplyBestProfile();
    }

    void LoadProfiles()
    {
        profiles = new GraphicsProfile[profileFiles.Length];

        for (int i = 0; i < profileFiles.Length; i++)
        {
            if (File.Exists(profileFiles[i]))
            {
                string json = File.ReadAllText(profileFiles[i]);
                profiles[i] = JsonUtility.FromJson<GraphicsProfile>(json);
            }
            else
            {
                UnityEngine.Debug.LogError("JSON Profile Not Found: " + profileFiles[i]);
            }
        }
    }

    void ApplyBestProfile()
    {
        int targetFPS = 30;
        int bestIndex = 0;
        GraphicsProfile bestProfile = profiles[0];

        for (int i = 0; i < profiles.Length; i++)
        {
            ApplySettings(profiles[i]);
            float fps = SimulatedFPS(profiles[i]);

            UnityEngine.Debug.Log("Profile: " + profiles[i].profileName + " | FPS: " + fps);

            if (fps >= targetFPS)
            {
                bestProfile = profiles[i];
                bestIndex = i;
            }
            else
                break;
        }


        
        currentProfile = bestProfile;
        CurrentProfileIndex = bestIndex;
        ApplySettings(currentProfile);

        UnityEngine.Debug.Log("Optimal Profile Applied: " + currentProfile.profileName);
    }

    float SimulatedFPS(GraphicsProfile p)
    {
        float baseFPS = 120f;
        return baseFPS - (p.shadowDistance * 0.1f);
    }

    void ApplySettings(GraphicsProfile p)
    {
        p.ApplyProfile();
    }

    public void ApplyProfile(string fileName)
    {
        string fullPath = Path.Combine(UnityEngine.Application.streamingAssetsPath, "Config", fileName);


        if (!File.Exists(fullPath))
        {
            UnityEngine.Debug.LogError("Profile JSON not found: " + fullPath);
            return;
        }

        string json = File.ReadAllText(fullPath);
        currentProfile = JsonUtility.FromJson<GraphicsProfile>(json);

        ApplySettings(currentProfile);
        UnityEngine.Debug.Log("Applied profile from: " + fileName);
    }
}
