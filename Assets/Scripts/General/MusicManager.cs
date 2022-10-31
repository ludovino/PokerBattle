using DG.Tweening;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    [SerializeField]
    [Range(0f, 1f)]
    private float sfxVolume;
    public float SfxVolume => sfxVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float musicVolume;
    public float MusicVolume => musicVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float masterVolume;
    public float MasterVolume => masterVolume;
    [SerializeField]
    [Range(0f, 1f)]
    private float ambienceVolumeMultiplier;
    [SerializeField]
    private AudioSource ambience;
    [SerializeField]
    private AudioSource menu;
    [SerializeField]
    private AudioSource game;
    [SerializeField]
    private AudioSource boss;

    public void SetVolume(float master, float music, float sfx)
    {
        musicVolume = music;
        SetMusicVolume();
        sfxVolume = sfx;
        SetSfxVolume();
        masterVolume = master;
    }

    private AudioSource _current;
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Music");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    private void Start()
    {
        _current = menu;
        if(!_current.isPlaying)
        {
            _current.Play();
            SetMusicVolume();
        }
    }
    public void SetMusicVolume()
    {
        menu.volume = musicVolume * masterVolume;
        game.volume = musicVolume * masterVolume;
        boss.volume = musicVolume * masterVolume;
    }

    public void SetSfxVolume()
    {
        ambience.volume = sfxVolume * ambienceVolumeMultiplier * masterVolume;
    }
    public void AmbienceOn()
    {
        if(!ambience.isPlaying) ambience.Play();
    }
    public void AmbienceOff()
    {
        if(ambience.isPlaying) ambience.Stop();
    }
    public void Menu()
    {
        SwapMusicTo(menu);
    }

    public void Game()
    {
        SwapMusicTo(game);
    }

    public void Boss()
    {
        SwapMusicTo(boss);
    }

    public void StopMusic()
    {
        var playing = _current;
        _current.DOFade(0f, 1f).OnComplete(() => _current.Stop());
    }
    private void SwapMusicTo(AudioSource audioSource)
    {
        if (audioSource == _current) return;
        var ending = _current;
        _current = audioSource;
        if(ending != null && ending.isPlaying) ending.DOFade(0f, 1f).OnComplete(() => ending.Stop());
        audioSource.Play();
        audioSource.DOFade(musicVolume, 1.5f);
    }
}
