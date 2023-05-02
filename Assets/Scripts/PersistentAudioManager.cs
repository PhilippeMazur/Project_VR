using UnityEngine;


public class PersistentAudioManager : MonoBehaviour
{
    public static PersistentAudioManager Instance;

    public AudioClip audioClip;
    public AudioClip buttonClip;
    public float startTime;

    private AudioSource audioSource;


    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PlayAudioClip();
    }

    private void PlayAudioClip()
    {
        if (audioClip != null)
        {
            audioSource.clip = audioClip;
            audioSource.time = startTime;
            audioSource.Play();
        }
    }

    public void PlayButtonSound()
    {
        audioSource.PlayOneShot(buttonClip);
    }
}