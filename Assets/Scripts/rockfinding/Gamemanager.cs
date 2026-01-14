using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.InputSystem;
using DG.Tweening;
using UnityEngine.UI;

public class Gamemanager : MonoBehaviour
{
    public GameObject panel;

    public CanvasGroup canvasGroup;
    public Slider HealthSlider;
    public Slider progressSlider;

    private AudioSource audioSource;
    public AudioClip flipSound;
    public AudioClip correctSound;
    public AudioClip wrongSound;
    private AudioSource bgmSource;
    public AudioClip backgroundMusic;


    private int totalStones = 12;
    private int targetCount = 4;
    private int maxMistakes = 3;
    private float showDuration = 5f;

    private int mistakeCount = 0;
    private int collectedCount = 0;

    private float timer = 0f;

    enum GameState { Ready, ShowPhase, SelectPhase, GameOver }
    private GameState currentState;

    public GameObject stonePrefab;
    public Transform gridCenter; 
    public float spacing = 1.5f;

    List<GameObject> Rocks = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        bgmSource = GetComponent<AudioSource>();
        
        bgmSource.clip = backgroundMusic;
        bgmSource.loop = true;         
        bgmSource.volume = 0.1f;        
        bgmSource.Play();

        audioSource = GetComponent<AudioSource>();
        panel.SetActive(false);
        currentState = GameState.Ready;
        timer = showDuration;

        progressSlider.value = 4;
    }

    // Update is called once per frame
    void Update()
    {
        switch(currentState)
        {
            case GameState.ShowPhase:
                UpdateShowPhase();
                break;
                
            case GameState.SelectPhase:
                UpdateSelectPhase();
                break;
            case GameState.Ready:
                if (Keyboard.current.anyKey.wasPressedThisFrame)
                {
                    currentState = GameState.ShowPhase;
                    GenerateGrid();
                    AssignTargets();
                }
                break;
            case GameState.GameOver:
                break;
        }
    }
    void GenerateGrid()
    {
        int rows = 3;
        int cols = 4;
        for(int row = 0; row < rows; row++)
        {
            for(int col = 0; col < cols; col++)
            {
                Vector3 position = CalculateGridPosition(row, col);
                GameObject stone = Instantiate(stonePrefab,position,Quaternion.identity);
                Rocks.Add(stone);
            }
        }
    }
    Vector3 CalculateGridPosition(int row,int col)
    {
        float x = gridCenter.position.x + (col-2)*spacing;
        float y = gridCenter.position.y + (-row+1)*spacing;
        return new Vector3(x,y,0);
    }

    void AssignTargets()
    {
        int assigned = 0;
        
        while(assigned < targetCount)
        {
            int randomIndex = Random.Range(0, totalStones);
            
            Stone stoneScript = Rocks[randomIndex].GetComponent<Stone>();
            if(!stoneScript.isTarget)
            {
                stoneScript.isTarget = true;
                assigned++;
            }
        }
    }
    void UpdateShowPhase()
    {
        timer -= Time.deltaTime;
        progressSlider.value = timer*0.8f;
        if(timer <= 0)
        {
            HideAllStones();
            currentState = GameState.SelectPhase;
        }
    }

    void UpdateSelectPhase()
    {

    }

    void HideAllStones()
    {
        foreach(GameObject rock in Rocks)
        {
            Stone stoneScript = rock.GetComponent<Stone>();
            stoneScript.Hide();
        }
        audioSource.PlayOneShot(flipSound);
    }
    public void OnStoneClicked(Stone clickedStone)
    {
        if(currentState != GameState.SelectPhase) return;
        if(clickedStone.isCollected) return; 
        
        if(clickedStone.isTarget)
        {
            Debug.Log("Correct!");
            audioSource.PlayOneShot(correctSound);
            clickedStone.isCollected = true;
            collectedCount++;
            progressSlider.value= collectedCount;
            Destroy(clickedStone.gameObject);
            
            if(collectedCount >= targetCount)
            {
                Debug.Log("Win!");
                currentState = GameState.GameOver;
                ShowPanel();
                StartCoroutine(GoNextAfterDelay(2.5f));
            }
        }
        else
        {
            Debug.Log("Wrong!");
            mistakeCount++;
            audioSource.PlayOneShot(wrongSound);
            HealthSlider.value= maxMistakes-mistakeCount;
            
            clickedStone.ShowAppearance();  
            
            if(mistakeCount >= maxMistakes)
            {
                Debug.Log("Lose!");
                currentState = GameState.GameOver;
                ShowPanel();
                StartCoroutine(ReloadSceneAfterDelay(2.5f));
            }
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
        SceneManager.LoadScene("ch2");
    }
        void ShowPanel()
    {
        panel.SetActive(true);
        
        canvasGroup.alpha = 0f;
        canvasGroup.DOFade(1f, 2f);
    }
}
