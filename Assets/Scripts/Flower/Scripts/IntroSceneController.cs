using UnityEngine;
using Flower;
using UnityEngine.InputSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
public class IntroSceneController : MonoBehaviour
{
    FlowerSystem fs;
    public string dlialog;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        fs = FlowerManager.Instance.CreateFlowerSystem(dlialog, false);
        fs.SetupDialog();
        fs.ReadTextFromResource(dlialog);
        fs.RegisterCommand("load_scene",(List<string> _params)=>{
            SceneManager.LoadScene(_params[0]);
        });
    }

    // Update is called once per frame
    void Update()
    {
        if (Keyboard.current.anyKey.wasPressedThisFrame)
        {
            fs.Next();
        }
    }
}
