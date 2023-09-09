using UnityEngine;

public class MainMenuSettings : GameSettings<MainMenuSettings>
{
    private const string VolumeKey = "Volume";
    public float Volume { get; set; }


    public override void SaveSettings()
    {
        PlayerPrefs.SetFloat(VolumeKey, Volume);
    }

    public override void LoadSettings()
    {
        Volume = PlayerPrefs.GetFloat(VolumeKey, 0);
    }
}