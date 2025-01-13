using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ProcessSituationData : MonoBehaviour
{
    StadisticsOfSelections[] dataToAnalyze;
    bool[] dataToAnalyzeBool;

    [SerializeField] TextMeshProUGUI situationText;
    [SerializeField] TextMeshProUGUI conditionWinText;
    [SerializeField] TextMeshProUGUI elec1Text;
    [SerializeField] TextMeshProUGUI elec2Text;
    [SerializeField] TextMeshProUGUI elec3Text;
    [SerializeField] GameObject buttonReplay;
    [SerializeField] GameObject buttonMainMenu;

    int i = 0; // Indice para saber en que situacion estamos

    private void Awake()
    {
        dataToAnalyze = GameManager.Instance.lastGameDataSaved;
        dataToAnalyzeBool = new bool[dataToAnalyze.Length];

        // Especificamos que situaciones nos interesa analizar
        dataToAnalyzeBool[0] = true;
        dataToAnalyzeBool[1] = true;
        dataToAnalyzeBool[3] = true;

        dataToAnalyzeBool[10] = true;
        dataToAnalyzeBool[14] = true;

        if (GameManager.Instance.getWinCondition())
            conditionWinText.text = "¡¡Has conseguido acabar la carrera!!";
        else if (GameManager.Instance.getStat(0) <= 0)
            conditionWinText.text = "Estas demasiado solo y te está afectando por lo que decides abandonar la carrera";
        else if (GameManager.Instance.getStat(1) <= 0)
            conditionWinText.text = "El estrés y el cansancio generado por la carrera hacen que no puedas más y decides abandonarla";
        else if (GameManager.Instance.getStat(2) <= 0)
            conditionWinText.text = "No tienes ganas de estudiar y decides abandonar la carrera";
        else if (GameManager.Instance.getStat(3) <= 0)
            conditionWinText.text = "Te has quedado sin dinero y no puedes continuar la carrera";
    }

    // Start is called before the first frame update
    void Start()
    {
        


    }

    public void ProcessData()
    {
        elec1Text.gameObject.SetActive(false);
        elec2Text.gameObject.SetActive(false);
        elec3Text.gameObject.SetActive(false);

        // Si una situacion tiene un posible analisis y se ha activado, se activa esta condicion
        bool conditionActivated = false;
        int nVeces = dataToAnalyze[i].nSelectedLeft + dataToAnalyze[i].nSelectedRight;

        // Process data
        switch (GameManager.Instance.indexGameDataSaved)
        {
            case 0:
                // Situacion Salir de Fiesta
                // Si ha salido (1/3 * nVeces que ha salido la carta) veces de fiesta y su responsabilidad academica es menor que 25
                // y ha habido al menos 3 fiestas
                situationText.text = "Salir de Fiesta";
                if (dataToAnalyze[i].nSelectedLeft > nVeces / 3 && GameManager.Instance.stats[2] <= 25 &&
                    nVeces >= 3)
                {
                    // Conclusion: El jugador no es responsable
                    elec1Text.gameObject.SetActive(true);
                    elec1Text.text = "Bro, relajate que la idea es terminar la carrera, no hacer speedrun de coma etilico";
                    conditionActivated = true;
                }
                break;

            case 1:
                // Situacion Adelantas trabajo por la noche o descansas
                situationText.text = "Adelantas trabajo por la noche o descansas";
                // Si el jugador ha trasnochado al menos 2 veces mas que de lo que ha descansado

                if (dataToAnalyze[i].nSelectedLeft > dataToAnalyze[i].nSelectedRight + 2)
                {
                    elec1Text.gameObject.SetActive(true);
                    elec1Text.text = "La de organizarte bien te la sabes?";
                    conditionActivated = true;
                }

                // Si el jugador lleva 3 dias sin dormir
                if (dataToAnalyze[i].maxStreakLeft >= 3)
                {
                    elec2Text.gameObject.SetActive(true);
                    elec2Text.text = "Tienes unas ojeras del tamaño de la cueva de Batman. Necesitas descansar";
                    conditionActivated = true;
                }
                break;
            case 3:
                // Situacion 4
                situationText.text = "Muchas cosas que hacer, lo dejo para mañana o empiezo ahora con ello";

                // Si el jugador ha pospuesto 4 veces seguidas sus tareas
                if (dataToAnalyze[i].maxStreakRight >= 4)
                {
                    elec1Text.gameObject.SetActive(true);
                    elec1Text.text = "No dejes para mañana lo que puedes hacer hoy";
                    conditionActivated = true;
                }

                // Si el jugador ha dejado para el finde sus tareas 4 veces seguidas
                if (dataToAnalyze[i].maxStreakDown >= 4)
                {
                    elec2Text.gameObject.SetActive(true);
                    elec2Text.text = "Tus amigos suelen estar libre en mitad de semana, adelanta lo que puedas en el resto de la semana";
                    conditionActivated = true;
                }

                // Si el jugador hace sus tareas durante 4 dias seguidos
                if (dataToAnalyze[i].maxStreakLeft >= 4)
                { 
                    elec3Text.gameObject.SetActive(true);
                    elec3Text.text = "A menos que tu tarea sea urgente, no pasa nada por descansar un poco";
                    conditionActivated = true;
                }

                break;


            // SITUACIONES ESPECIALES (Actualmente)

            case 8:
                // SpecificSituacion 0
                // Inicio de las situaciones especiales [CUIDADO, si se añade mas situaciones normales, el inicio de las situaciones especiales cambiará]
                break;

            case 10:
                // SpecificSituacion 2
                // El profe me cae mal, voy o no a tutoria
                situationText.text = "No te va muy bien en los estudios, deberias pedir tutoria al profesor que te cae mal.";

                // Si el jugador ha evitado la tutoria 5 veces mas de lo que ha ido
                if (dataToAnalyze[i].nSelectedLeft - dataToAnalyze[i].nSelectedRight <= -5)
                {
                    elec1Text.gameObject.SetActive(true);
                    elec1Text.text = "Asi como dato: El profesor es el que sabe como se debe realizar la asignatura";
                    conditionActivated = true;
                }
                // Si el jugador ha ido 5 veces mas a tutoria que las que ha evitado
                else if (dataToAnalyze[i].nSelectedLeft - dataToAnalyze[i].nSelectedRight >= 5)
                {
                    elec1Text.gameObject.SetActive(true);
                    elec1Text.text = "Además de la tutoria del profesor, puedes preguntar a amigos, ir a la biblioteca o mirar apuntes pasados";
                    conditionActivated = true;
                }

                // Si el jugador ha evitado la tutoria al menos 4 veces seguidas
                if (dataToAnalyze[i].maxStreakRight >= 4)
                { 
                    elec2Text.gameObject.SetActive(true);
                    // Github copilot ha sugerido esta respuesta y yo no lo voy a cambiar
                    elec2Text.text = "No te va a morder, y si lo hace, denuncialo";
                    conditionActivated = true;
                }
                break;

            case 14:
                // SpecificSituation 6
                // Ultima Temporada de examenes te deja cansao, vas al psicoloco o con quien lo hablas
                situationText.text = "La última temporada de exámenes te ha dejado bastante agotado y estas algo deprimido.";

                // Si el jugador piensa que ha sido una mala racha 3 veces
                if (dataToAnalyze[i].nSelectedDown >= 3) 
                {
                    elec1Text.gameObject.SetActive(true);
                    elec1Text.text = "No te lo guardes todo, intenta hablarlo con alguien que sino luego explotas";
                    conditionActivated = true;
                }

                // Si la situacion ha salido al menos 4 veces y el jugador ha ido al psicologo menos de 2 veces y el stat de bienestar es menor que 25
                if (nVeces >= 4 && dataToAnalyze[i].nSelectedRight < 2 && GameManager.Instance.stats[1] <= 25)
                {
                    elec2Text.gameObject.SetActive(true);
                    elec2Text.text = "No pasa nada por hablarlo con un profesional. Psicall es una buena opcion";
                    conditionActivated = true;
                }
                break;
        }

        // Buscamos la siguiente situacion a analizar
        GameManager.Instance.indexGameDataSaved++;
        while (GameManager.Instance.indexGameDataSaved < dataToAnalyzeBool.Length && !dataToAnalyzeBool[GameManager.Instance.indexGameDataSaved])
        {
            GameManager.Instance.indexGameDataSaved++;
        }

        if(GameManager.Instance.indexGameDataSaved >= dataToAnalyzeBool.Length)
        {
            situationText.text = "Quieres jugar otra partida? Haz click en el boton de jugar otra vez, abajo a la izquierda";
            buttonReplay.SetActive(true);
            buttonMainMenu.SetActive(true);
            conditionActivated = true; // Lo activamos para evitar errores
        }

        if(!conditionActivated)
        {
            // Si no se ha activado ninguna condicion, volvemos a hacer el analisis con la siguiente situacion que nos interese
            ProcessData();
        }
    }
}
