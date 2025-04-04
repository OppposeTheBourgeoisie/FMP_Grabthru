using UnityEngine;
using UnityEngine.SceneManagement;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;
    private AudioSource audioSource;
    public AudioClip[] musicTracks;

    private void Awake()
    {
        if (Instance == null)
        {
            //Keep the music playing between scenes
            Instance = this;
            DontDestroyOnLoad(gameObject);

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true;

            //Play selected song immediately
            int selectedSong = PlayerPrefs.GetInt("SelectedSong", 0);
            PlayMusic(selectedSong);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlayMusic(int trackIndex)
    {
        //Play the selected music track
        if (trackIndex >= 0 && trackIndex < musicTracks.Length)
        {
            audioSource.clip = musicTracks[trackIndex];
            audioSource.Play();
        }
    }
}
