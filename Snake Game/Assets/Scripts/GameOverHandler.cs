using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Text gameOverText;
    [SerializeField] private Image fadeImage;

    [Header("Settings")]
    [SerializeField] private float textDisplayTime = 1.5f;
    [SerializeField] private float fadeDuration = 1f;

    [Header("Sound Effects")]
    public AudioClip gameOverSound;
    public float gameOverVolume = 0.7f;
    private AudioSource audioSource;

    private void Start()
    {
        // Initialize audio source
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;

        // Initialize hidden UI
        gameOverText.gameObject.SetActive(false);
        fadeImage.gameObject.SetActive(false);
    }

    public void TriggerGameOver()
    {
        // Play game over sound
        if (gameOverSound != null)
        {
            audioSource.PlayOneShot(gameOverSound, gameOverVolume);
        }

        // Show game over text
        gameOverText.gameObject.SetActive(true);

        // Start fade sequence
        StartCoroutine(GameOverSequence());
    }
    private System.Collections.IEnumerator GameOverSequence()
    {
        // Wait before fading
        yield return new WaitForSeconds(textDisplayTime);

        // Fade out
        fadeImage.gameObject.SetActive(true);
        float timer = 0f;
        Color fadeColor = fadeImage.color;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            fadeColor.a = Mathf.Lerp(0f, 1f, timer / fadeDuration);
            fadeImage.color = fadeColor;
            yield return null;
        }

        // Load main menu
        SceneManager.LoadScene("MainMenuScene");
    }
}