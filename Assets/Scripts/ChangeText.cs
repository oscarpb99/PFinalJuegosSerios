using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ChangeText : MonoBehaviour
{
    // Start is called before the first frame update
    private TextMeshProUGUI text;
    public GameManager gameManager;
    public int index;
    public bool isStat = false;
    private void Awake()
    {

        text= GetComponent<TextMeshProUGUI>();
    }
    void Start()
    {
        if(isStat)
            text.text=gameManager.getStat(index).ToString();
        
    }

    // Update is called once per frame
    void Update()
    {
        if(isStat)
            text.text = gameManager.getStat(index).ToString();
       
    }
}
