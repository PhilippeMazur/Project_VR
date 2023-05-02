using UnityEngine;
using UnityEngine.SceneManagement;

public class GameSceneManager : MonoBehaviour
{
    public string menuScene = "MenuScene";
    public string gameScene = "GameScene";
    public string settingsScene = "SettingsScene";
    public AudioClip backgroundMusic;

    private void Start()
    {
        if(SceneManager.GetActiveScene().name != "MenuScene" || SceneManager.GetActiveScene().name != "SettingsScene")
        {
            backgroundMusic = null;
        }
        PersistentAudioManager.Instance.audioClip = backgroundMusic;
    }

    public void PlayButtonClicked()
    {
        
        SceneManager.LoadSceneAsync(gameScene);
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