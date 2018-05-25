using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TimeManager : MonoBehaviour {

    public static TimeManager instance;
    private EventManager eventManager;

    public float nbSecondForOneMinuteInGame = 1.5f;
    private int minuteInGame = 0;
    private float timer;
    public int hourBeginDay = 5, hourEndDay = 23;
    public int minuteBeginDay = 0, minuteEndDay = 0;
    public float speedTime {
        get
        {
            return actualTime * Time.deltaTime;
        }
    }
    private float actualTime = 1;
    [SerializeField] float ratioSpeedX2 = 1.5f, ratioSpeedX3 = 2;
    [SerializeField] Button[] buttonTime;
    private Image[] imageButton;

    // Use this for initialization
    void Awake () {
        instance = this;
        eventManager = GetComponent<EventManager>();
        InitButtonTime();
        minuteInGame = (hourBeginDay * 60) + minuteBeginDay;
        eventManager.TimeEvent(minuteInGame);
	}
	
	// Update is called once per frame
	void Update () {
        ModeTimeInput();
        TimeInGame();
	}

    private void ModeTimeInput()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if(actualTime == 0)
            {
                ModeTime(1);
            }
            else
            {
                ModeTime(0);
            }
        }
    }

    private void InitButtonTime()
    {
        imageButton = new Image[buttonTime.Length];
        for(int i = 0; i < buttonTime.Length; i++)
        {
            int valTemp = i;
            buttonTime[i].onClick.AddListener(() =>
            {
                ModeTime(valTemp);
            });
            imageButton[i] = buttonTime[i].transform.GetChild(0).GetComponent<Image>();
        }
    }

    private void ColorButtonTime(int mode)
    {
        for(int i = 0; i < 4; i++)
        {
            if(i == mode)
            {
                imageButton[i].color = Color.red;
            }
            else
            {
                imageButton[i].color = Color.white;
            }
        }
    }

    private void ModeTime(int mode)
    {
        switch (mode)
        {
            case 0:
                actualTime = 0;
                ColorButtonTime(mode);
                break;
            case 1:
                actualTime = 1;
                ColorButtonTime(mode);
                break;
            case 2:
                actualTime = ratioSpeedX2;
                ColorButtonTime(mode);
                break;
            case 3:
                actualTime = ratioSpeedX3;
                ColorButtonTime(mode);
                break;
            default:
                actualTime = 1;
                ColorButtonTime(1);
                break;
        }
    }

    private void TimeInGame()
    {
        if (minuteInGame < (hourEndDay * 60) + minuteEndDay)
        {
            timer += speedTime;
            if (timer > nbSecondForOneMinuteInGame)
            {
                timer -= nbSecondForOneMinuteInGame;
                minuteInGame++;
                eventManager.TimeEvent(minuteInGame);
            }
        }
        else
        {
            //fin de journée -> reset du temps / récapitulatif / etc
        }
    }

    public float RatioTime()
    {
        return actualTime;
    }
}
