using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SfxManager : MonoBehaviour
{
    private static SfxManager _instance;
    [Range(0f, 1f)]
    public float sfxVolume; 
    [Range(0f, 1f)]
    public float sfxVolumeMultiplier;
    public AudioClip[] playCard;
    public AudioClip[] chipClick;
    public AudioClip cheer;
    public AudioClip aww;
    public AudioClip gasp;
    public static SfxManager Instance 
    { 
        get { return _instance; } 
    }

    public void SetVolume(float master, float sfx)
    {
        sfxVolume = sfx * master;
    }

    private void Awake() 
    { 
        thisFrame = new HashSet<AudioClip>();
        if (_instance != null && _instance != this) 
        { 
            Destroy(gameObject);
            return;
        }

        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        sources = GetComponentsInChildren<AudioSource>();
    }
    private AudioSource[] sources;
    private int nextSource;
    private HashSet<AudioClip> thisFrame;

    // Start is called before the first frame update
    private void PlayClip(AudioClip clip)
    {
        if(thisFrame.Contains(clip)) return;
        thisFrame.Add(clip);
        var source = sources[nextSource];
        source.PlayOneShot(clip, sfxVolume * sfxVolumeMultiplier);
        nextSource = nextSource < sources.Length - 1 ? nextSource + 1 : 0;
    }
    public static void Play(AudioClip clip)
    {
        Instance.PlayClip(clip);
    }
    public static void PlayCard(int index)
    {
        var sounds = Instance.playCard;
        var clip = index >= sounds.Length ? sounds[0] : sounds[index];
        Instance.PlayClip(clip);
    }

    public static void ChipClick(int index)
    {
        var clip = Instance.chipClick[Random.Range(0, Instance.chipClick.Length)];
        Instance.PlayClip(clip);
    }

    public static void Cheer()
    {
        Instance.PlayClip(Instance.cheer);
    }

    public static void Aww()
    {
        Instance.PlayClip(Instance.aww);
    }

    public void LateUpdate()
    {
        thisFrame.Clear();
    }

    internal static void Gasp()
    {
        Instance.PlayClip(Instance.gasp);
    }
}
