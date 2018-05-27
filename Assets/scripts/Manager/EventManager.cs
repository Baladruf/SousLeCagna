using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {

    [SerializeField] Text timerText;
    [SerializeField] int nbMinuteByPrint = 5;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
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

    #region Event
    public delegate void EventSoldier(float degatHp, float degatMoral);
    public event EventSoldier eventSoldier;
    #endregion
}
