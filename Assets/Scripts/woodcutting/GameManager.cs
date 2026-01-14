using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using System.Collections;
using UnityEngine.SceneManagement;
using DG.Tweening;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public GameObject panel;

    public CanvasGroup canvasGroup;

    public Slider healthSlider;
    public Slider progressSlider;


    private AudioSource audioSource;
    public AudioClip cutSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    public AudioClip placeItem;
    private AudioSource bgmSource;
    public AudioClip backgroundMusic;

    public int MaxMistake = 5;
    private float SpawnInterval;
    private int CurrentIndex;
    private int MistakeCount;
    private int TotalItems;
    
    private bool CurrentItem;
    private bool IsGameRunning = true;
    private float timer = 0f;

    private bool canInteract;

    public Transform spawnPoint;
    public GameObject woodPrefab;
    public GameObject animalPrefab;
    private GameObject currentSpawnedObject;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        bgmSource = GetComponent<AudioSource>();
        
        bgmSource.clip = backgroundMusic;
        bgmSource.loop = true;         
        bgmSource.volume = 0.5f;        
        bgmSource.Play();

        audioSource = GetComponent<AudioSource>();
        IsGameRunning = false;
        panel.SetActive(false);
        TotalItems = 20;
        canInteract = true;
    }

    // Update is called once per frame
    void Update()
    {   
        if(!IsGameRunning || !canInteract)
        {
            if(Keyboard.current.anyKey.wasPressedThisFrame && canInteract)
            {
                IsGameRunning=true;
                return;
            }
            else
                return;  
        } 
        UpdateSpawnInterval();
        progressSlider.value = TotalItems - CurrentIndex;
        timer += Time.deltaTime;
        if (timer >= SpawnInterval) 
        {
            if(CurrentItem)
                MistakeCount++;
            timer = 0f;
            healthSlider.value = MaxMistake - MistakeCount;
            NextItem();
        }
        else if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            audioSource.PlayOneShot(cutSound);
            CheckItem();
            timer = 0f;
            NextItem();
        }
        if(MistakeCount >= MaxMistake)
        {
            Debug.Log("lose");
            IsGameRunning = false;
            canInteract = false;
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
        SceneManager.LoadScene("ch1");
    }
    void UpdateSpawnInterval()
    {
        if(CurrentIndex <= 4)
        {
            SpawnInterval = 2.0f;
        }
        else if(CurrentIndex <= 9)
        {
            SpawnInterval = 1.5f;
        }
        else if(CurrentIndex <= 14)
        {
            SpawnInterval = 1.2f;
        }
        else if(CurrentIndex <= 19)
        {
            SpawnInterval = 1.0f;
        }
    }
    void NextItem()
    {
        if(currentSpawnedObject != null)
        {
            Destroy(currentSpawnedObject);
        }
        if(CurrentIndex < TotalItems)
        {
            StartCoroutine(SpawnItemWithDelay(CurrentIndex));
            CurrentIndex++;
        }
        else
        {
            Debug.Log("Win");
            IsGameRunning = false;
            canInteract = false;
            ShowPanel();
            StartCoroutine(GoNextAfterDelay(2.5f));
        }
    }
    IEnumerator SpawnItemWithDelay(int index)
    {
        canInteract = false;
        yield return new WaitForSeconds(0.5f); 
        SpawnItem(index);
        canInteract = true;
    }
    void CheckItem()
    {
        if (CurrentItem)
        {
            Debug.Log("nice");
            audioSource.PlayOneShot(correctSound);
        }
        else
        {
            Debug.Log("bad");
            audioSource.PlayOneShot(wrongSound);
            MistakeCount ++;
            healthSlider.value = MaxMistake - MistakeCount;
        }
    }
    void SpawnItem(int x)
    {
        GameObject prefabToSpawn;
        switch (x)
        {
            case 3: case 7: case 10: case 12: case 16: case 15: case 18:
                Debug.Log("Not Wood");
                prefabToSpawn = animalPrefab;
                CurrentItem = false;
                break;
            default:
                Debug.Log("wood");
                prefabToSpawn = woodPrefab;
                CurrentItem = true;
                break;
        }
        audioSource.PlayOneShot(placeItem);
        currentSpawnedObject = Instantiate(prefabToSpawn, spawnPoint.position, Quaternion.identity);
    }
    void ShowPanel()
    {
        panel.SetActive(true);
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 2f);
    }
}
