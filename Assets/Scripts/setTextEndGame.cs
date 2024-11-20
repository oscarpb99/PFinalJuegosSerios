using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class setTextEndGame : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        if (GameManager.Instance.getWinCondition())
            GetComponent<TextMeshProUGUI>().text = "¡¡Has conseguido acabar la carrera!!";

        else
            GetComponent<TextMeshProUGUI>().text = "La carrera ha podido contigo";



    }

    
}
