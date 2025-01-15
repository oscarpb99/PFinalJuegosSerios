using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using TMPro;
using UnityEngine;
using UnityEngine.TestTools;
using Random = UnityEngine.Random;

[Serializable]
public struct StadisticsOfSelections
{
    //Numero de veces que se elige cada opcion en cada situacion
    public int nSelectedLeft;
    public int nSelectedRight;
    public int nSelectedDown;

    //Stats usadas para las situaciones acumulativas( las que van sumando o restando 1)
    public int acumulativeLeft;
    public int acumulativeRight;
    public int acumulativeDown;

    //Para las rachas
    public int streakLeftNow;
    public int streakRightNow;
    public int streakDownNow;

    // Maxima racha conseguida
    public int maxStreakLeft;
    public int maxStreakRight;
    public int maxStreakDown;

    public Sprite image;

}

public class SituationManager : MonoBehaviour
{
    public Situation[] situations;
    public Situation[] specificSituations;
    public TutorialCard[] tutorialCards = new TutorialCard[0];
    public int tutorialCounter;

    //array para guardar cuantas veces tiene que esperar una situacion para volver a salir
    private int[] sleepingSituations;

    //situaciones bloqueadas
    private bool[] lockedSituations;
    private bool[] lockedSpecificSituations;

    public StadisticsOfSelections[] numRepeatedSelections;
    int lastSituation;

    int currentSituation;
    int cartasAño;
    int cardCounter;
    int yearDifficulty;
    int specificCounter;
    int contadorAños;
    bool isSpecific = false;
    public TextMeshProUGUI textSituation;
    public TextMeshProUGUI textElec1, textElec2, textElec3;
    [SerializeField] GameObject backgroundElec1;
    [SerializeField] GameObject backgroundElec2;
    [SerializeField] GameObject backgroundElec3;
    public UnityEngine.UI.Image imagenSituation;
    public enum Type { None,Acumulador};
    public Type type;

    private void Awake()
    {
        GameManager.Instance.situationManager = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        // Para cuando asignamos una situacion, ya se han cargado todas en situations
        numRepeatedSelections=new StadisticsOfSelections[situations.Length + specificSituations.Length - 4];
        for (int i = 0; i < situations.Length + specificSituations.Length - 4; i++)
        {
            numRepeatedSelections[i].nSelectedLeft = 0;
            numRepeatedSelections[i].nSelectedRight = 0;
            numRepeatedSelections[i].nSelectedDown = 0;

            numRepeatedSelections[i].acumulativeLeft = 0;
            numRepeatedSelections[i].acumulativeRight = 0;
            numRepeatedSelections[i].acumulativeDown = 0;

            numRepeatedSelections[i].streakLeftNow = 0;
            numRepeatedSelections[i].streakRightNow = 0;
            numRepeatedSelections[i].streakDownNow = 0;

            numRepeatedSelections[i].maxStreakLeft = 0;
            numRepeatedSelections[i].maxStreakRight = 0;
            numRepeatedSelections[i].maxStreakDown = 0;
        }

        for (int i = 0; i < GameManager.Instance.stats.Length; i++)
        {
            GameManager.Instance.stats[i] = 50;
        }

        lockedSituations = new bool[situations.Length];
        lockedSpecificSituations = new bool[specificSituations.Length];
        sleepingSituations = new int[situations.Length];
        blockSituations();
        blockSpecificSituations();
        for(int  i = 0;i < situations.Length;i++)
        {
            sleepingSituations[i] = 0;
        }
        cartasAño = Random.Range(15, 25);
        //cartasAño = Random.Range(5, 10);

        cardCounter = 0;
        yearDifficulty = 0;
        tutorialCounter = 0;
        specificCounter = 0;

        // Guardamos el sprite de cada situacion
        for (int i = 0; i < situations.Length + specificSituations.Length - 4; i++)
        {
            if(i < situations.Length)
            {
                numRepeatedSelections[i].image = Situations.Instance.GetSprite(situations[i].image);
            }
            else
            {
                numRepeatedSelections[i].image = Situations.Instance.GetSprite(specificSituations[i - situations.Length].image);
            }
            
        }

        SetTutorial();
    }

    private void setSituation()
    {
        int specialCardProbability;
        bool nextIsSpecial = false;
        isSpecific = false;

        //Empiezan con un tipo cualquiera(None) y si luego es Acumulador se cambia
        type = Type.None;



        cardCounter++;
        specificCounter++;

        // Restamos 1 a todas las situaciones que tienen que esperar al menos 1 turno
        for (int i = 0; i < sleepingSituations.Length; i++)
        {
            if (sleepingSituations[i] > 0)
            {
                sleepingSituations[i]--;
            }
        }

        // Activamos las elecciones por defecto
        if (textElec1.gameObject.activeSelf == false)
        {
            backgroundElec1.SetActive(true);
            textElec1.gameObject.SetActive(true);
        }
        if (textElec2.gameObject.activeSelf == false)
        {
            backgroundElec2.SetActive(true);
            textElec2.gameObject.SetActive(true);
        }

        //Si ya se han sacado todas las cartas del año, se resetea el contador y sale la carta de los exámenes
        if (cardCounter >= cartasAño)
        {
            cardCounter = 0;
            cartasAño = Random.Range(15, 25);
            //cartasAño = Random.Range(5, 10);

            contadorAños++;

            // Si es el primer año
            if (contadorAños == 1)
            {
                // Desbloqueamos la situqacion de que te dejan agotado los examenes
                lockedSpecificSituations[6] = false;
            }
            else if (contadorAños == 4) // A partir del cuarto año, queda desbloqueado la situacion de tus compañeros
            {
                // Desbloqueamos la situacion de que tus compañeros se graduan
                lockedSpecificSituations[7] = false;
            }
            isSpecific = true;
            currentSituation = 0;
            textSituation.text = specificSituations[currentSituation].situation;
            int maxDifficulty = 70 + yearDifficulty;
            int midDifficulty = 40 + yearDifficulty;

            if(maxDifficulty > 100)
            {
                maxDifficulty = 100;
            }
            if (midDifficulty > 80)
            {
                midDifficulty = 80;
            }


            if (GameManager.Instance.getStat(2) >= maxDifficulty) 
            {
                // Texto BIEN TODO PASADO activado
                backgroundElec1.SetActive(true);
                textElec1.gameObject.SetActive(true);
                textElec1.text = specificSituations[currentSituation].elec1;

                // Desactivamos los otros 2 textos
                backgroundElec2.SetActive(false);
                textElec2.gameObject.SetActive(false);
                backgroundElec3.SetActive(false);
                textElec3.gameObject.SetActive(false);
            }
            else if(GameManager.Instance.getStat(2) >= midDifficulty)
            {
                // Texto BIEN TODO PASADO desactivado
                backgroundElec1.SetActive(false);
                textElec1.gameObject.SetActive(false);

                // Texto BUENO, ME HAN QUEDADO ALGUNAS activado
                backgroundElec2.SetActive(true);
                textElec2.gameObject.SetActive(true);
                textElec2.text = specificSituations[currentSituation].elec2;

                // Desactivamos el otro texto
                backgroundElec3.SetActive(false);
                textElec3.gameObject.SetActive(false);
                yearDifficulty += 10;
            }
            else
            {
                // Desactivamos los 2 primeros textos
                backgroundElec1.SetActive(false);
                textElec1.gameObject.SetActive(false);

                backgroundElec2.SetActive(false);
                textElec2.gameObject.SetActive(false);

                // Texto JODER, ¿TANTAS? activado
                backgroundElec3.SetActive(true);
                textElec3.gameObject.SetActive(true);
                textElec3.text = specificSituations[currentSituation].elec3;
                
                yearDifficulty += 20;
            }
            GameManager.Instance.examWeek();


            GameManager.Instance.setStatsText(1, specificSituations[currentSituation].stat1Left, specificSituations[currentSituation].stat2Left, specificSituations[currentSituation].stat3Left, specificSituations[currentSituation].stat4Left);
            GameManager.Instance.setStatsText(0, specificSituations[currentSituation].stat1Right, specificSituations[currentSituation].stat2Right, specificSituations[currentSituation].stat3Right, specificSituations[currentSituation].stat4Right);
            GameManager.Instance.setStatsText(2, specificSituations[currentSituation].stat1Down, specificSituations[currentSituation].stat2Down, specificSituations[currentSituation].stat3Down, specificSituations[currentSituation].stat4Down);
            

            imagenSituation.sprite = Situations.Instance.GetSprite(specificSituations[currentSituation].image);
        }
        else
        {
            
            //Comprueba que una stat es baja para tener la posibilidad de que salga una carta especial
            if (GameManager.Instance.getStat(0) <= 20 || GameManager.Instance.getStat(1) <= 20 || GameManager.Instance.getStat(2) <= 20 || GameManager.Instance.getStat(3) <= 20)
            {
                // Probabilidad de que salga una carta especial del 20%
                specialCardProbability = Random.Range(0, 5);
                if (specialCardProbability == 1)
                {
                    nextIsSpecial = true;
                }
            }

            // Solo puede haber una carta especial si hay una stat baja
            if (!nextIsSpecial)
            {
                if (specificCounter >= 6)
                {
                    // Seleccionamos una situacion específica
                    specificCounter = 0;
                    currentSituation = Random.Range(1, specificSituations.Length - 4);
                    while (lockedSpecificSituations[currentSituation])
                    {
                        currentSituation = Random.Range(1, specificSituations.Length - 4);
                    }
                    isSpecific = true;
                }
                else
                {
                    // Seleccionamos una situacion general
                    currentSituation = Random.Range(0, situations.Length);
                    while (lockedSituations[currentSituation] || sleepingSituations[currentSituation] > 0)
                    {
                        currentSituation = Random.Range(0, situations.Length);
                    }
                    
                }
            }
            //Por diseño, metemos las cartas especiales al final
            else
            {
                int index = Random.Range(0, 4);
                while (GameManager.Instance.getStat(index) > 20)
                {
                    index = Random.Range(0, 4);
                }
                currentSituation = specificSituations.Length - 4 + index;
                //type = Type.Probabilidad;
                isSpecific = true;
            }


            if (isSpecific)
            {
                textSituation.text = specificSituations[currentSituation].situation;
                textElec1.text = specificSituations[currentSituation].elec1;
                textElec2.text = specificSituations[currentSituation].elec2;
                if (specificSituations[currentSituation].elec3 == "" || specificSituations[currentSituation].elec3 == "NULL")
                {
                    //type = Type.Desactivar;
                    backgroundElec3.SetActive(false);
                    textElec3.gameObject.SetActive(false);
                }
                else
                {
                    backgroundElec3.SetActive(true);
                    textElec3.gameObject.SetActive(true);
                    textElec3.text = specificSituations[currentSituation].elec3;
                    

                }
                GameManager.Instance.setStatsText(1, specificSituations[currentSituation].stat1Left, specificSituations[currentSituation].stat2Left, specificSituations[currentSituation].stat3Left, specificSituations[currentSituation].stat4Left);
                GameManager.Instance.setStatsText(0, specificSituations[currentSituation].stat1Right, specificSituations[currentSituation].stat2Right, specificSituations[currentSituation].stat3Right, specificSituations[currentSituation].stat4Right);
                GameManager.Instance.setStatsText(2, specificSituations[currentSituation].stat1Down, specificSituations[currentSituation].stat2Down, specificSituations[currentSituation].stat3Down, specificSituations[currentSituation].stat4Down);
                imagenSituation.sprite = Situations.Instance.GetSprite(specificSituations[currentSituation].image);
            }
            else
            {
                textSituation.text = situations[currentSituation].situation;
                textElec1.text = situations[currentSituation].elec1;
                textElec2.text = situations[currentSituation].elec2;
                if (situations[currentSituation].elec3 == "" || situations[currentSituation].elec3 == "NULL")
                {
                    //type = Type.Desactivar;
                    backgroundElec3.SetActive(false);
                    textElec3.gameObject.SetActive(false);
                }
                else
                {
                    backgroundElec3.SetActive(true);
                    textElec3.gameObject.SetActive(true);
                    textElec3.text = situations[currentSituation].elec3;
                    

                }

               

                GameManager.Instance.setStatsText(1, situations[currentSituation].stat1Left, situations[currentSituation].stat2Left, situations[currentSituation].stat3Left, situations[currentSituation].stat4Left);
                GameManager.Instance.setStatsText(0, situations[currentSituation].stat1Right, situations[currentSituation].stat2Right, situations[currentSituation].stat3Right, situations[currentSituation].stat4Right);
                GameManager.Instance.setStatsText(2, situations[currentSituation].stat1Down, situations[currentSituation].stat2Down, situations[currentSituation].stat3Down, situations[currentSituation].stat4Down);
                sleepingSituations[currentSituation] = situations[currentSituation].spawnRate;

                imagenSituation.sprite = Situations.Instance.GetSprite(situations[currentSituation].image);
            }

            //Comprueba si el tipo es acumulador
            if ((!isSpecific && situations[currentSituation].tag == "Acumulador") || (isSpecific && specificSituations[currentSituation].tag == "Acumulador"))
            {
                type = Type.Acumulador;
            }
        }

    }

    private void SetTutorial()
    {
        // Con las cartas de tutorial ya leidas, se leen de manera ordenada
        if (tutorialCounter < tutorialCards.Length)
        {
            textSituation.text = tutorialCards[tutorialCounter].situation;
            textElec1.text = tutorialCards[tutorialCounter].elec1;
            textElec2.text = tutorialCards[tutorialCounter].elec2;

            // Si la tercera opcion pone NULL, desactivar
            if (tutorialCards[tutorialCounter].elec3 == "" || tutorialCards[tutorialCounter].elec3 == "NULL")
            {
                //type = Type.Desactivar;
                backgroundElec3.SetActive(false);
                textElec3.gameObject.SetActive(false);
            }
            else
            {
                backgroundElec3.SetActive(true);
                textElec3.gameObject.SetActive(true);
                textElec3.text = tutorialCards[tutorialCounter].elec3;
            }

            GameManager.Instance.setStatsText(1, tutorialCards[tutorialCounter].stat1Left, tutorialCards[tutorialCounter].stat2Left, tutorialCards[tutorialCounter].stat3Left, tutorialCards[tutorialCounter].stat4Left);
            GameManager.Instance.setStatsText(0, tutorialCards[tutorialCounter].stat1Right, tutorialCards[tutorialCounter].stat2Right, tutorialCards[tutorialCounter].stat3Right, tutorialCards[tutorialCounter].stat4Right);
            GameManager.Instance.setStatsText(2, tutorialCards[tutorialCounter].stat1Down, tutorialCards[tutorialCounter].stat2Down, tutorialCards[tutorialCounter].stat3Down, tutorialCards[tutorialCounter].stat4Down);
            imagenSituation.sprite = Situations.Instance.GetSprite(tutorialCards[tutorialCounter].image);
        }
        else
        {
            setSituation();
        }

    }

    public void applyCardTutorial(int option)
    {
        switch (tutorialCounter)
        {
            default:
                break;
            case 0:
                // Si elige NO continuar el tutorial
                if (option != 2)
                    tutorialCounter = tutorialCards.Length + 1;
                break;
            case 4:
                // Enseñamos que se puede modificar las stats
                if (option == 1)
                {
                    GameManager.Instance.addorloseStats(0, GameManager.Instance.getStatsText(1)[0], GameManager.Instance.getStatsText(1)[1], GameManager.Instance.getStatsText(1)[2], GameManager.Instance.getStatsText(1)[3]);
                }
                else if (option == 2)
                {
                    GameManager.Instance.addorloseStats(1, GameManager.Instance.getStatsText(0)[0], GameManager.Instance.getStatsText(0)[1], GameManager.Instance.getStatsText(0)[2], GameManager.Instance.getStatsText(0)[3]);
                }
                else
                {
                    GameManager.Instance.addorloseStats(2, GameManager.Instance.getStatsText(2)[0], GameManager.Instance.getStatsText(2)[1], GameManager.Instance.getStatsText(2)[2], GameManager.Instance.getStatsText(2)[3]);
                }
                break;
            case 6:
                // Reseteamos los stats
                GameManager.Instance.resetStats();
                break;

        }

        // Pasamos a la siguiente carta del tutorial
        tutorialCounter++;
        SetTutorial();
    }

    
    public int getCurrentSituation()
    {
        return currentSituation;
    }

    public Type getType()
    {
        return type;
    }

    public void manageSituations()
    {
        // Si seguimos con los tutoriales, llamamos a setTutorial
        if (tutorialCounter < tutorialCards.Length)
        {
            SetTutorial();
        }
        else
        {
            // Seleccionamos una situacion. Si la situacion es la misma que la anterior, se vuelve a seleccionar otra
            lastSituation = currentSituation;

            setSituation();
        }
    }

    public void blockSituations()
    {
        //Aqui están las situaciones generales que empiezan bloqueadas
        for(int i = 0; i < situations.Length; i++)
        {
            
            switch (i)
            {
                default:
                    lockedSituations[i] = false;
                    break;
                case 5:
                    lockedSituations[i] = true; // Futbol
                    break;
                case 6:
                    lockedSituations[i] = true; // Trabajo
                    break;
                case 7:
                    lockedSituations[i] = true; // Psicologo
                    break;
            }
        }
    }

    public void blockSpecificSituations()
    {
        //Aqui están las situaciones específicas que empiezan bloqueadas
        for (int i = 0; i < specificSituations.Length; i++)
        {
            switch (i)
            {
                default:
                    lockedSpecificSituations[i] = false;
                    break;
                case 2:
                    lockedSpecificSituations[i] = true; //Tutoria profesor 
                    break;
                case 6:
                    lockedSpecificSituations[i] = true; // Temporada de examenes te deja agotado
                    break;
                case 7:
                    lockedSpecificSituations[i] = true; // Tus compañeros se graduan este año, mientras tu sigues en la carrera (Si situacion de examenes sale al menos 4 veces)
                    break;
                case 8:
                    lockedSpecificSituations[i] = true; // Ultimamente las cosas no han ido bien 
                    break;
            }
        }
    }

    // Desbloqueamos situaciones si se selecciona una opcion en una situacion especifica
    public void unlockAndLockSituation(int option)
    {
        // Nos dan index, que es el id de la situacion que puede desbloquear otras situaciones y specific, que indica si es una situacion general o especifica

        if (!isSpecific)
        {
            switch(currentSituation)
            {
                // Si existe alguna situacion general que desbloquee otras, iria aqui
                case 6: // Situacion de Jornada Laboral
                    if (option == 1) //
                        lockedSituations[6] = true; // Jornada Laboral (Se bloquea a si mismo si se elige "LO DEJO")
                    break;

                default:
                    break;
            }
        }
        else 
        {
            // Si existe alguna situacion especifica que desbloquee otras, iria aqui
            switch (currentSituation)
            { 
                default:
                    break;
                case 3: // SpecificSituations[3] de unirse al club de futbol que desbloquea:
                    if(option == 0) // Si elige SI unirse al club de futbol
                        lockedSpecificSituations[4] = false; // Entrenamiento de futbol
                    break;

                case 4: // SpecificSituations[4] de Conseguir Trabajo que desbloquea:
                    if (option == 0) // Si elige "LO COGEMOS"
                        lockedSpecificSituations[5] = false; // Jornada Laboral
                    break;
                case 6: // SpecificSituations[6] de Temporada de examenes te deja agotado que desbloquea:
                    if (option == 1) // Si elige "Lo hablo con un profesional"
                        lockedSpecificSituations[6] = false; // Visita al psicologo
                    break;
                case 8: // SpecificSituations[8] de Ultimamente las cosas no han ido bien que desbloquea:
                    if (option == 1) // Si elige "Deberia ir al psicologo"
                        lockedSpecificSituations[6] = false; // Visita al psicologo
                    break;

            }
        }

        // Si pasa un numero de situaciones (Por ejemplo, la mitad del año), chequeamos las stats para ver si desbloqueamos alguna situacion
        // hasta que termine el año
        if (cardCounter >= cartasAño/2)
        {
            // Si el bienestar esta por debajo de 30, desbloqueamos la situacion de "Ultimamente las cosas no han ido bien"
            if (GameManager.Instance.getStat(1) <= 30) 
                lockedSpecificSituations[8] = false;

            // Si la responsabilidad academica esta por debajo de 30, desbloqueamos la situacion de "Tutoria profesor"
            else if(GameManager.Instance.getStat(2) <= 30)
                lockedSpecificSituations[2] = false;
        }

        // Chequeamos si las stats son normales, en cuyo caso volvemos a bloquear algunas situaciones
        if(GameManager.Instance.getStat(1) > 30) // Bienestar
            lockedSpecificSituations[8] = true;
        else if(GameManager.Instance.getStat(2) > 30) // Responsabilidad academica
            lockedSpecificSituations[2] = true;
    }

    public bool getIsSpecific()
    {
        return isSpecific;
    }

    public void exportStatisticsToJson(string name)
    {
        StringBuilder jsonBuilder = new StringBuilder();
        // Define la ruta del archivo JSON dentro de la carpeta "Assets/ExportedStats"
        string folderPath = Path.Combine(Directory.GetCurrentDirectory(), "ExportedStats");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        jsonBuilder.Append("[\n");
        // Serializa los objetos de 'situations' y 'numRepeatedSelections' a JSON
        for (int i = 0; i < situations.Length; i++)
        {
            StadisticsToExport stadisticsOfOneSituation;
            if (numRepeatedSelections[i].nSelectedLeft + numRepeatedSelections[i].nSelectedRight + numRepeatedSelections[i].nSelectedDown > 0)
            {
                stadisticsOfOneSituation = new StadisticsToExport
                {
                    situation = situations[i].situation,
                    nAppearances = numRepeatedSelections[i].nSelectedLeft + numRepeatedSelections[i].nSelectedRight + numRepeatedSelections[i].nSelectedDown,
                    elec1 = situations[i].elec1,
                    nSelectedLeft = numRepeatedSelections[i].nSelectedLeft,
                    maxStreakLeft = numRepeatedSelections[i].maxStreakLeft,
                    chosenPercentajeElec1 = ((float)numRepeatedSelections[i].nSelectedLeft / (numRepeatedSelections[i].nSelectedLeft + numRepeatedSelections[i].nSelectedRight + numRepeatedSelections[i].nSelectedDown)) * 100,
                    elec2 = situations[i].elec2,
                    nSelectedRight = numRepeatedSelections[i].nSelectedRight,
                    maxStreakRight = numRepeatedSelections[i].maxStreakRight,
                    chosenPercentajeElec2 = ((float)numRepeatedSelections[i].nSelectedRight / (numRepeatedSelections[i].nSelectedLeft + numRepeatedSelections[i].nSelectedRight + numRepeatedSelections[i].nSelectedDown)) * 100,
                    elec3 = situations[i].elec3,
                    nSelectedDown = numRepeatedSelections[i].nSelectedDown,
                    maxStreakDown = numRepeatedSelections[i].maxStreakDown,
                    chosenPercentajeElec3 = ((float)numRepeatedSelections[i].nSelectedDown / (numRepeatedSelections[i].nSelectedLeft + numRepeatedSelections[i].nSelectedRight + numRepeatedSelections[i].nSelectedDown)) * 100

                };
            }
            else
            {
                stadisticsOfOneSituation = new StadisticsToExport
                {
                    situation = situations[i].situation,
                    nAppearances = 0,
                    elec1 = situations[i].elec1,
                    nSelectedLeft = numRepeatedSelections[i].nSelectedLeft,
                    maxStreakLeft = numRepeatedSelections[i].maxStreakLeft,
                    chosenPercentajeElec1 = 0.0f,
                    elec2 = situations[i].elec2,
                    nSelectedRight = numRepeatedSelections[i].nSelectedRight,
                    maxStreakRight = numRepeatedSelections[i].maxStreakRight,
                    chosenPercentajeElec2 = 0.0f,
                    elec3 = situations[i].elec3,
                    nSelectedDown = numRepeatedSelections[i].nSelectedDown,
                    maxStreakDown = numRepeatedSelections[i].maxStreakDown,
                    chosenPercentajeElec3 = 0.0f

                };
            }
            // Serializar el objeto anónimo
            jsonBuilder.Append(JsonUtility.ToJson(stadisticsOfOneSituation,true));
            jsonBuilder.Append(",\n");
            
        }

        // Serializa los objetos de 'specificSituations' y las repeticiones correspondientes
        for (int i = 0; i < specificSituations.Length-4; i++)
        {
            StadisticsToExport stadisticsOfOneSituation;
            if (numRepeatedSelections[situations.Length + i].nSelectedLeft + numRepeatedSelections[situations.Length + i].nSelectedRight + numRepeatedSelections[situations.Length + i].nSelectedDown > 0)
            {
                stadisticsOfOneSituation = new StadisticsToExport
                {
                    situation = specificSituations[i].situation,
                    nAppearances = numRepeatedSelections[situations.Length + i].nSelectedLeft + numRepeatedSelections[situations.Length + i].nSelectedRight + numRepeatedSelections[situations.Length + i].nSelectedDown,
                    elec1 = specificSituations[i].elec1,
                    nSelectedLeft = numRepeatedSelections[situations.Length + i].nSelectedLeft,
                    maxStreakLeft = numRepeatedSelections[situations.Length + i].maxStreakLeft,
                    chosenPercentajeElec1 =((float) numRepeatedSelections[situations.Length + i].nSelectedLeft / (numRepeatedSelections[situations.Length + i].nSelectedLeft + numRepeatedSelections[situations.Length + i].nSelectedRight + numRepeatedSelections[situations.Length + i].nSelectedDown)) * 100,
                    elec2 = specificSituations[i].elec2,
                    nSelectedRight = numRepeatedSelections[situations.Length + i].nSelectedRight,
                    maxStreakRight = numRepeatedSelections[situations.Length + i].maxStreakRight,
                    chosenPercentajeElec2 =((float) numRepeatedSelections[situations.Length + i].nSelectedRight / (numRepeatedSelections[situations.Length + i].nSelectedLeft + numRepeatedSelections[situations.Length + i].nSelectedRight + numRepeatedSelections[situations.Length + i].nSelectedDown)) * 100,
                    elec3 = specificSituations[i].elec3,
                    nSelectedDown = numRepeatedSelections[situations.Length + i].nSelectedDown,
                    maxStreakDown = numRepeatedSelections[situations.Length + i].maxStreakDown,
                    chosenPercentajeElec3 = ((float)numRepeatedSelections[situations.Length + i].nSelectedDown / (numRepeatedSelections[situations.Length + i].nSelectedLeft + numRepeatedSelections[situations.Length + i].nSelectedRight + numRepeatedSelections[situations.Length + i].nSelectedDown)) * 100

                };
            }
            else
            {
                stadisticsOfOneSituation = new StadisticsToExport
                {
                    situation = specificSituations[i].situation,
                    nAppearances = 0,
                    elec1 = specificSituations[i].elec1,
                    nSelectedLeft = numRepeatedSelections[situations.Length + i].nSelectedLeft,
                    maxStreakLeft = numRepeatedSelections[situations.Length + i].maxStreakLeft,
                    chosenPercentajeElec1 = 0.0f,
                    elec2 = specificSituations[i].elec2,
                    nSelectedRight = numRepeatedSelections[situations.Length + i].nSelectedRight,
                    maxStreakRight = numRepeatedSelections[situations.Length + i].maxStreakRight,
                    chosenPercentajeElec2 = 0.0f,
                    elec3 = specificSituations[i].elec3,
                    nSelectedDown = numRepeatedSelections[situations.Length + i].nSelectedDown,
                    maxStreakDown = numRepeatedSelections[situations.Length + i].maxStreakDown,
                    chosenPercentajeElec3 = 0.0f

                };
            }
            // Serializar el objeto anónimo
            jsonBuilder.Append(JsonUtility.ToJson(stadisticsOfOneSituation, true));

            // Si no es el último elemento, agrega una coma
            if (i < specificSituations.Length - 5)
            {
                jsonBuilder.Append(",\n");
            }
        }

        jsonBuilder.Append("\n]");

        // Convertir el StringBuilder a un string
        string json = jsonBuilder.ToString();

        // Define la ruta del archivo JSON
        if (name != "" && name != null)
        {
            string path = Path.Combine(folderPath, name);

            // Guarda el archivo JSON
            File.WriteAllText(path, json);

            Debug.Log("Estadísticas exportadas a: " + path);
        }
        else
        {
            Debug.LogError("El nombre del archivo no puede estar vacío.");
        }
        

    }
}
