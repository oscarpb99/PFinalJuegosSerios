using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonPanelName : MonoBehaviour
{
    public Button creditsButton;
    public Button playButton;
    public TextMeshProUGUI title;

    public TMP_InputField inputField;
    public TextMeshProUGUI textPanel;
    public GameObject panel;
   


    public void accept()
    {
        // Obtener el texto ingresado
        string playerName = inputField.text;

        // Validar si el campo no está vacío
        if (!string.IsNullOrEmpty(playerName))
        {
           NameData.PlayerName= playerName + ".json";
           creditsButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(true);
            title.gameObject.SetActive(true);

            inputField.gameObject.SetActive(false);
            textPanel.gameObject.SetActive(false);
            panel.SetActive(false);
            gameObject.SetActive(false);
        }
        else
        {
            Debug.Log("Por favor, introduce un nombre válido.");
        }
    }
}
