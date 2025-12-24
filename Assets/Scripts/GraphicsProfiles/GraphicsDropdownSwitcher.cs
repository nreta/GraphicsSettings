using System.Diagnostics;
using TMPro;
using UnityEngine;

public class GraphicsDropdownSwitcher : MonoBehaviour
{
    public TMP_Dropdown graphicsDropdown;
    public QualitySwitcher qualitySwitcher;

    void Start()
    {
        // Listen for changes
        graphicsDropdown.onValueChanged.AddListener(OnDropdownChanged);
        Invoke(nameof(SyncDropdownWithAutoProfile), 0.2f);
    
    }
    void SyncDropdownWithAutoProfile()
    {
        if (qualitySwitcher == null || graphicsDropdown == null)
            return;

        // Set dropdown to match auto-selected profile WITHOUT triggering event
        graphicsDropdown.SetValueWithoutNotify(qualitySwitcher.CurrentProfileIndex);
    }

    void OnDropdownChanged(int index)
    {
        switch (index)
        {
            case 0:
                qualitySwitcher.ApplyProfile("GraphicsLow.json");
                break;
            case 1:
                qualitySwitcher.ApplyProfile("GraphicsMedium.json");
                break;
            case 2:
                qualitySwitcher.ApplyProfile("GraphicsHigh.json");
                break;
            case 3:
                qualitySwitcher.ApplyProfile("GraphicsUltra.json");
                break;
        }

        UnityEngine.Debug.Log("User Graphics profile set to: " + graphicsDropdown.options[index].text);
    }
}
