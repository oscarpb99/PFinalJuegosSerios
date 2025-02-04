using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIFill : MonoBehaviour
{
    [SerializeField] int maxValue;
    [SerializeField] Image fill;
    [SerializeField] GameObject circle;
    [SerializeField] int valGameManager;
    float currentValue;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.Instance.imagesStats[valGameManager] = this;
        currentValue = 0.5f;
        fill.fillAmount = 0.5f;
    }

    public void addOrDeduct(float i)
    { 
        currentValue = i;
        fill.fillAmount = currentValue / maxValue;
    }

    public void showModifiedStat(bool show)
    {
        circle.SetActive(show);
    }

}
