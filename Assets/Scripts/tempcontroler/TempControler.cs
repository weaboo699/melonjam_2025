using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.Collections; 
using System;
public class TempControler : MonoBehaviour
{


    public GameObject panel;

    public CanvasGroup canvasGroup;

    public SpriteRenderer rend;
    public Sprite normal; 
    public Sprite sad;  
    public Sprite happy;

    private AudioSource audioSource;
    public AudioClip warningSound;
    public AudioSource bgmSource;
    public AudioClip backgroundMusic;


    public Slider temperatureSlider;
    public Slider HealthSlider;
    public Slider progressSlider;

    public Image fillImage;
    private float temperature = 50f;
    private float heatingSpeed = 20f;
    private float coolingSpeed = 15f;

    private float idealMin = 40f;
    private float idealMax = 60f;
    private float dangerMin = 30f;
    private float dangerMax = 70f;
    

    private float successTimer = 0f;
    private float successRequired = 10f;
    

    private float mistakeCount = 0f;
    private float maxMistakes = 5f;

    enum GameState { Ready, Playing, Win, Lose }
    private GameState currentState = GameState.Ready;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
        bgmSource.clip = backgroundMusic;
        bgmSource.loop = true;         
        bgmSource.volume = 0.1f;        
        bgmSource.Play();

        audioSource = GetComponent<AudioSource>();

        panel.SetActive(false);
        temperatureSlider.minValue = 0f;
        temperatureSlider.maxValue = 100f;
        temperatureSlider.value = temperature;
    }

    // Update is called once per frame
    void Update()
    {
        if(currentState == GameState.Ready)
            if(Keyboard.current.anyKey.wasPressedThisFrame)
                currentState = GameState.Playing;
        if(currentState != GameState.Playing) return;
        
        UpdateTemperature();
        UpdateUI();
        CheckTemperatureZone();
        UpdateDifficulty();
        CheckWinLose();
    }
    void UpdateDifficulty()
    {
        if(successTimer < 2f)
        {
            idealMin = 40f;
            idealMax = 60f;
        }
        else if(successTimer < 4f)
        {
            idealMin = 42f;
            idealMax = 58f;
        }
        else if(successTimer < 6f)
        {
            idealMin = 45f;
            idealMax = 55f;
        }
        else
        {
            idealMin = 48f;
            idealMax = 52f;
        }
    }
    void UpdateTemperature()
    {
        if(Keyboard.current.spaceKey.isPressed)
        {
            temperature += heatingSpeed * Time.deltaTime;
        }
        else
        {
            temperature -= coolingSpeed * Time.deltaTime;
        }
        
        temperature = Mathf.Clamp(temperature, 0f, 100f);
    }
    void CheckTemperatureZone()
    {
        if(temperature >= idealMin && temperature <= idealMax)
        {
            successTimer += Time.deltaTime;
            progressSlider.value = successTimer;
        }
        
        if(temperature < dangerMin || temperature > dangerMax)
        { 
            mistakeCount += Time.deltaTime;
            if(!audioSource.isPlaying)
            audioSource.PlayOneShot(warningSound);
        }
    }
    void CheckWinLose()
    {
        if(successTimer >= successRequired)
        {
            Debug.Log("Win!");
            currentState = GameState.Win;
            ShowPanel();
            StartCoroutine(GoNextAfterDelay(2.5f));
        }
        
        if(mistakeCount >= maxMistakes)
        {
            Debug.Log("Lose!");
            currentState = GameState.Lose;
            ShowPanel();
            StartCoroutine(ReloadSceneAfterDelay(2.5f));
        }
    }
    IEnumerator ReloadSceneAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
    IEnumerator GoNextAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ch3");
    }
    

    void UpdateUI()
    {
        temperatureSlider.value = temperature;

            if(temperature < dangerMin)
            {
                fillImage.color = Color.blue;
                rend.sprite = sad;
            } 
            else if(temperature >= idealMin && temperature <= idealMax)
            {
                fillImage.color = Color.green;
                rend.sprite = happy;
            }
                
            else if(temperature > dangerMax)
            {
                fillImage.color = Color.red;
                rend.sprite = sad;
            }
                
            else if(temperature >= 50f)
            {
                fillImage.color = new Color(1f, 0.5f, 0f);  // 橙色
                rend.sprite = normal;
            }
                
            else
            {
                fillImage.color = Color.cyan;
                rend.sprite = normal;
            }
                
        HealthSlider.value = maxMistakes - mistakeCount;
    }
    void ShowPanel()
    {
        panel.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 2f);
    }
}
