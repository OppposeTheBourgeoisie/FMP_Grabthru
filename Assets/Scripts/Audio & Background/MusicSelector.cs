using UnityEngine;
using TMPro;

public class MusicSelector : MonoBehaviour
{
    public TMP_Dropdown dropdown;

    void Start()
    {
        //Load last selected song
        int savedSong = PlayerPrefs.GetInt("SelectedSong", 0);
        dropdown.value = savedSong;

        //Change the song picked in the dropdown changes
        dropdown.onValueChanged.AddListener(delegate { OnSongSelected(dropdown.value); });
    }

    void OnSongSelected(int index)
    {
        //Save the selected song
        PlayerPrefs.SetInt("SelectedSong", index);
        PlayerPrefs.Save();

        if (AudioManager.Instance != null)
        {
            AudioManager.Instance.PlayMusic(index);
        }
    }
}
