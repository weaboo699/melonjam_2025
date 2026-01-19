using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenuManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void start()
    {
        SceneManager.LoadScene("Intro");
    }
    public void quit()
    {
        Application.Quit();
    }
}
