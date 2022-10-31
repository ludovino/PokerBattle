using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VolumeControl : MonoBehaviour
{
    [SerializeField]
    private Slider _master;
    [SerializeField]
    private Slider _music;
    [SerializeField]
    private Slider _sfx;

    MusicManager _musicManager;
    SfxManager _sfxManager;

    public void Start()
    {
        _musicManager = GameObject.FindGameObjectWithTag("Music").GetComponent<MusicManager>();
        _sfxManager = GameObject.FindGameObjectWithTag("Sfx").GetComponent<SfxManager>();
        _master.value = _musicManager.MasterVolume;
        _music.value = _musicManager.MusicVolume;
        _sfx.value = _musicManager.SfxVolume;
        _sfxManager.SetVolume(_musicManager.MasterVolume, _musicManager.SfxVolume);
        _musicManager.SetVolume(_master.value, _music.value, _sfx.value);
    }

    public void OnChange()
    {
        _musicManager.SetVolume(_master.value, _music.value, _sfx.value);
        _sfxManager.SetVolume(_musicManager.MasterVolume, _musicManager.SfxVolume);
    }
}
