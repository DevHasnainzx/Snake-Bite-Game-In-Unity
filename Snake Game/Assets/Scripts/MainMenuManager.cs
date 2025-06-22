using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Required for button sound

public class MainMenuManager : MonoBehaviour
{
    [Header("Sound Settings")]
    public AudioClip buttonClickSound;
    public float clickVolume = 1f;

    private AudioSource audioSource;

    private void Start()
    {
        // Set up audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Add sound to all UI buttons automatically
        Button[] buttons = FindObjectsOfType<Button>();
        foreach (Button button in buttons)
        {
            button.onClick.AddListener(PlayButtonClickSound);
        }
    }

    public void OnStartButton()
    {
        PlayButtonClickSound();
        SceneManager.LoadScene("GameScene");
    }

    public void OnQuitButton()
    {
        PlayButtonClickSound();
        Application.Quit();
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void PlayButtonClickSound()
    {
        if (buttonClickSound != null)
        {
            audioSource.PlayOneShot(buttonClickSound, clickVolume);
        }
    }
}