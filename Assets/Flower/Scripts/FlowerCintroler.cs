using UnityEngine;
using Flower;
public class FlowerCintroler : MonoBehaviour
{
    FlowerSystem flowerSys;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        flowerSys = FlowerManager.Instance.CreateFlowerSystem("FlowerSample", false);
        flowerSys.ReadTextFromResource("dialog_1");
    }

}
