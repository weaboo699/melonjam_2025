using UnityEngine;

public class Stone : MonoBehaviour
{
    public bool isTarget;        
    public bool isCollected;        
    
    private Renderer rend;
    public Sprite targetMaterial; 
    public Sprite normalMaterial;  
    public Sprite hiddenMaterial;  

    private SpriteRenderer spriteRenderer;    
    private Gamemanager gameManager;
    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        gameManager = FindObjectOfType<Gamemanager>();

        ShowAppearance();
    }
    
    public void ShowAppearance()
    {
        if(isTarget)
            spriteRenderer.sprite = targetMaterial;
        else
            spriteRenderer.sprite = normalMaterial;
    }
    
    public void Hide()
    {
        spriteRenderer.sprite = hiddenMaterial;
    }
    
    void OnMouseDown()
    {
        gameManager.OnStoneClicked(this);
    }
}
