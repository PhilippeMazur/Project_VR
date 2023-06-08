using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public string menuScene = "MenuScene";
    public string gameScene = "GameScene";
    public string settingsScene = "SettingsScene";
    private AudioClip backgroundMusic;

    private void Start()
    {
        backgroundMusic = Resources.Load<AudioClip>("Sounds/MenuMusic");
        if (SceneManager.GetActiveScene().name != "MenuScene" || SceneManager.GetActiveScene().name != "SettingsScene")
        {
            backgroundMusic = null;
        }
        PersistentAudioManager.Instance.audioClip = backgroundMusic;
    }

    public void PlayButtonClicked()
    {
        
        SceneManager.LoadSceneAsync(gameScene);
    }

    public void MenuButtonClicked()
    {

        SceneManager.LoadSceneAsync(menuScene);
    }

    public void SettingsButtonClicked()
    {
        
        SceneManager.LoadSceneAsync(settingsScene);
    }

    public void SaveButtonClicked()
    {
        // Implement code to save chosen difficulty



        SceneManager.LoadSceneAsync(menuScene);
    }
}