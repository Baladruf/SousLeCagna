using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {

    [SerializeField] Text timerText;
    [SerializeField] int nbMinuteByPrint = 5;
    [System.NonSerialized]
    public TimeManager timeManager;
    [System.NonSerialized]
    public GameManager gameManager;


    private void Awake()
    {
        calendarEvent = new List<Event>();
        //print(eventEndDay);
        //eventEndDay += Start();
        calendarEvent.Add(new Event("TestEndDay", "juste un test fin de journee", (5 * 60) + 10, EnumDefine.NameEvent.endDay));
        //print(calendarEvent[0].eventTime);
        //eventTest.eventTime.DynamicInvoke();
    }

    /*public void AddEvent(Delegate del)
    {
        calendarEvent.Add(new Event("TestEndDay", "juste un test fin de journee", (5 * 60) + 10, del));
    }*/

    private void Start()
    {
        //eventEndDay();
    }

    // Update is called once per frame
    void Update () {
		//en theorie system d'alea pour event

	}

    public void TimeEvent(int minute)
    {
        ConvertTimeInText(minute);
        for(int i = 0; i < calendarEvent.Count; i++)
        {
            if(calendarEvent[i].heureDeDeclenchement == minute)
            {
                CallEvent(calendarEvent[i].eventName);
            }
        }
    }

    private void ConvertTimeInText(int time)
    {
        int minute = time % 60;
        if (minute % nbMinuteByPrint == 0) {
            int heure = time / 60;
            string heureS = heure < 10 ? "0" + heure : "" + heure;
            string minuteS = minute < 10 ? "0" + minute : "" + minute;
            timerText.text = heureS + ":" + minuteS;
        }
    }

    #region CallEvent
    public delegate void EventSoldier(float degatHp, float degatMoral);
    public event EventSoldier eventSoldier;

    public delegate void EventLetter();
    public event EventLetter eventLetter;

    public delegate void EventEndDay();
    public event EventEndDay eventEndDay;
    #endregion

    #region Event
    private struct Event
    {
        public string name;
        public string description;
        public int heureDeDeclenchement;
        public EnumDefine.NameEvent eventName;

        public Event(string n, string d, int t, EnumDefine.NameEvent nameEvent)
        {
            name = n;
            description = d;
            heureDeDeclenchement = t;
            eventName = nameEvent;
        }
    }
    private List<Event> calendarEvent;
    public void CallEvent(EnumDefine.NameEvent eventN)
    {
        switch (eventN)
        {
            case EnumDefine.NameEvent.endDay:
                eventEndDay();
                break;
            case EnumDefine.NameEvent.bombardement:
                break;
            case EnumDefine.NameEvent.letter:
                eventLetter();
                break;
        }
    }
    #endregion
}
