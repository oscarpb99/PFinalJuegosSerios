using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI text;
    public int index;
    //public bool isStat = false;
    private void Awake()
    {

        text= GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        if (index < 5) 
        {
            text.text = GameManager.Instance.getStat(index).ToString();
        }
        else
        {
            text.text = GameManager.Instance.getCredits().ToString();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (index < 5)
        {
            text.text = GameManager.Instance.getStat(index).ToString();
        }
        else
        {
            text.text = GameManager.Instance.getCredits().ToString();
        }
    }
}
