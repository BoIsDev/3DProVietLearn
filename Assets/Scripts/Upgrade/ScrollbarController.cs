using UnityEngine;
using UnityEngine.UI;

public class ScrollbarController : MonoBehaviour
{
    private static ScrollbarController instance;
    public static ScrollbarController Instance => instance;
    public Scrollbar Scrollbar;


    public void Awake()
    {
        if(instance == null)
            instance = this;
    }
    public void updateHealthBar(float currentValue, float maxValue)
    {
        Scrollbar.size = currentValue / maxValue;
    }
}
