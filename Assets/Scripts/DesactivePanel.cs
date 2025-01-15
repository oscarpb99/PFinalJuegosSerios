using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DesactivePanel : MonoBehaviour
{
    public Button creditsButton;
    public Button playButton;
    public TextMeshProUGUI title;

    public TMP_InputField inputField;
    public TextMeshProUGUI textPanel;
    public Button acceptButton;
    // Start is called before the first frame update
    void Start()
    {
        if (!string.IsNullOrEmpty(NameData.PlayerName))
        {
            creditsButton.gameObject.SetActive(true);
            playButton.gameObject.SetActive(true);
            title.gameObject.SetActive(true);

            inputField.gameObject.SetActive(false);
            textPanel.gameObject.SetActive(false);
            acceptButton.gameObject.SetActive(false);
            gameObject.SetActive(false);
        }
    }

    
    
}
