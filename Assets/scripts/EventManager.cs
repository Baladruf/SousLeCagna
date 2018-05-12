using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EventManager : MonoBehaviour {

    [SerializeField] Text timerText;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void TimeEvent(int minute)
    {
        timerText.text = ConvertTimeInText(minute);
    }

    private string ConvertTimeInText(int time)
    {
        int heure = time / 60;
        int minute = time % 60;
        string heureS = heure < 10 ? "0" + heure : "" + heure;
        string minuteS = minute < 10 ? "0" + minute : "" + minute;
        return heureS + ":" + minuteS;
    }
}
