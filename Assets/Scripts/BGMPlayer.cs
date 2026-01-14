using UnityEngine;

public class BGMPlayer : MonoBehaviour
{
    private AudioSource bgmSource;
    public AudioClip backgroundMusic;
    public float vol;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        
        bgmSource.clip = backgroundMusic;
        bgmSource.loop = true;       
        bgmSource.volume = vol;       
        bgmSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
