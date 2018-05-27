using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {

    [SerializeField] Text timerText;
    [SerializeField] int nbMinuteByPrint = 5;
    public TimeManager timeManager;

    private void Awake()
    {
        /*var eventTest = new Event("Test", "juste un test", 50, eventLetter);
        eventTest.eventTime.DynamicInvoke();*/
    }

    // Update is called once per frame
    void Update () {
		//en theorie system d'alea pour event

	}

    public void TimeEvent(int minute)
    {
        ConvertTimeInText(minute);
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
    #endregion

    #region Event
    private class Event
    {
        public string name;
        public string description;
        public int heureDeDeclenchement;
        public Delegate eventTime;

        public Event(string n, string d, int t, Delegate dele)
        {
            name = n;
            description = d;
            heureDeDeclenchement = t;
            eventTime = dele;
        }
    }
    #endregion
}
