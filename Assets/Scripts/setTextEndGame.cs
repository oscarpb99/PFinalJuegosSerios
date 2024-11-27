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
        else if(GameManager.Instance.getStat(0)<=0)
            GetComponent<TextMeshProUGUI>().text = "Estas demasiado solo y te está afectando por lo que decides abandonar la carrera";
        else if (GameManager.Instance.getStat(1) <= 0)
            GetComponent<TextMeshProUGUI>().text = "El estrés y el cansancio generado por la carrera hacen que no puedas más y decides abandonarla";
        else if (GameManager.Instance.getStat(2) <= 0)
            GetComponent<TextMeshProUGUI>().text = "No tienes ganas de estudiar y decides abandonar la carrera";
        else if (GameManager.Instance.getStat(3) <= 0)
            GetComponent<TextMeshProUGUI>().text = "Te has quedado sin dinero y no puedes continuar la carrera";



    }

    
}
