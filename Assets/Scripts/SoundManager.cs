using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    private AudioSource sourceMusic;
    private AudioSource sourceSoundEffects;

    public AudioClip music;
    public AudioClip soundEffect;
    private void Awake()
    {
        if(Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        sourceMusic = this.gameObject.AddComponent<AudioSource>();
        sourceSoundEffects = this.gameObject.AddComponent<AudioSource>();

        sourceMusic.clip = music;
        PlayMusic();
    }

    public void PlayMusic()
    {
        sourceMusic.loop = true;
        sourceMusic.Play();
    }

    public void PlaySoundEffect(AudioClip clip)
    {
        sourceSoundEffects.clip = clip;
        sourceSoundEffects.loop = false;
        sourceSoundEffects.Play();
    }

    public void SetMusicVolume(float volume) => sourceMusic.volume = volume;

    public void SetSoundEffectVolume(float volume) => sourceSoundEffects.volume = volume;
}