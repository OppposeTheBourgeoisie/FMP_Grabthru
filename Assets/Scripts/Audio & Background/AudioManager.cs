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
            Instance = this;
            DontDestroyOnLoad(gameObject); // Keep music playing across scenes

            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.loop = true; // Keep playing the song

            // 🔹 Load and play selected song immediately
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
        if (trackIndex >= 0 && trackIndex < musicTracks.Length)
        {
            audioSource.clip = musicTracks[trackIndex];
            audioSource.Play();
        }
    }
}
