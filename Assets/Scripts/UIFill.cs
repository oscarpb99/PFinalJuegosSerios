using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFill : MonoBehaviour
{
    [SerializeField] int maxValue;
    [SerializeField] Image fill;
    float currentValue;
    // Start is called before the first frame update
    void Start()
    {
        currentValue = 0.5f;
        fill.fillAmount = 0.5f;
    }

    public void addOrDeduct(float i)
    { 
        currentValue = i;
        fill.fillAmount = currentValue / maxValue;
    }

}
