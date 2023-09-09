using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider volumeSlider;

    private const string VolumeMixerParam = "Volume";

    private void Awake()
    {
        volumeSlider.value = MainMenuSettings.Instance.Volume;
        audioMixer.SetFloat(VolumeMixerParam, MainMenuSettings.Instance.Volume);
    }

    public void SetVolume(float volume)
    {
        MainMenuSettings.Instance.Volume = volume;
        audioMixer.SetFloat(VolumeMixerParam, volume);
    }
}