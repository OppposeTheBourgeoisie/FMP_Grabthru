using UnityEngine;
using TMPro;

public class MusicSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        // Load last selected song
        int savedSong = PlayerPrefs.GetInt("SelectedSong", 0);
        dropdown.value = savedSong; // Set dropdown to match last choice

        // Listen for dropdown changes
        dropdown.onValueChanged.AddListener(delegate { OnSongSelected(dropdown.value); });
    }

    void OnSongSelected(int index)
    {
        PlayerPrefs.SetInt("SelectedSong", index);
        PlayerPrefs.Save(); // Ensure it's saved

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(index); // Play selected song immediately
        }
    }
}
