using UnityEngine;

public class ScAudioManager : MonoBehaviour {
    public static ScAudioManager Instance;

    [Header("~~~~~~~~ Audio Source ~~~~~~~~")]
    [SerializeField] AudioSource _musicSource;
    [SerializeField] AudioSource _SFXSource;
    
    [Header("~~~~~~~~ Audio Song ~~~~~~~~")]
    public AudioClip _mainMenuSong;
    
    private void Awake() {
        if (Instance == null) { Instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(this); }
    }
    
    
    private void Start() {
        PlayMainMusic(_mainMenuSong);
    }
    
    public void PlayMainMusic(AudioClip clip) { 
        _musicSource.Stop();
        _musicSource.clip = clip;
        _musicSource.loop = true;
        _musicSource.Play();
    }
}
