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
    [SerializeField] int degatBombardement = 5;
    [SerializeField] int degatInvasionAllemand;
    public ParticleSystem[] listParticleBomb;


    private void Awake()
    {
        calendarEvent = new List<Event>();
        //print(eventEndDay);
        //eventEndDay += Start();
        calendarEvent.Add(new Event("TestEndDay", "juste un test fin de journee", (5 * 60) + 10, 80, EnumDefine.NameEvent.InvasionAllemand));
        calendarEvent.Add(new Event("TestInvasion", "test de l'event invasion", (5 * 60) + 20, 100, EnumDefine.NameEvent.bombardement));
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

    public delegate void EventBombardement(int degat);
    public event EventBombardement eventBombardement;

    public delegate void EventInvasionAllemand(int degat);
    public event EventInvasionAllemand eventInvasionAllemand;
    #endregion

    #region Event
    private class Event
    {
        public string name;
        public string description;
        public int heureDeDeclenchement;
        public float probaAppear;
        public EnumDefine.NameEvent eventName;

        public Event(string n, string d, int t, float p, EnumDefine.NameEvent nameEvent)
        {
            name = n;
            description = d;
            heureDeDeclenchement = t;
            probaAppear = p;
            eventName = nameEvent;
        }
    }
    private List<Event> calendarEvent;
    public void CallEvent(EnumDefine.NameEvent eventN)
    {
        switch (eventN)
        {
            case EnumDefine.NameEvent.endDay:
                if (eventEndDay != null)
                {
                    eventEndDay();
                }
                gameManager.endDayManager.CallEndDay();
                break;
            case EnumDefine.NameEvent.bombardement:
                if(eventBombardement != null)
                {
                    //eventBombardement(degatBombardement);
                    StartCoroutine(MiseEnSceneBombarde(0));
                    StartCoroutine(ExplosionsBombarde());
                }
                break;
            case EnumDefine.NameEvent.letter:
                if (eventLetter != null)
                {
                    eventLetter();
                }
                break;
            case EnumDefine.NameEvent.InvasionAllemand:
                if(eventInvasionAllemand != null)
                {
                    eventInvasionAllemand(degatInvasionAllemand);
                }
                break;
        }
    }

    public void ReduceProbaEventInvasionAllemand(float proba)
    {
        for(int i = 0; i < calendarEvent.Count; i++)
        {
            if(calendarEvent[i].eventName == EnumDefine.NameEvent.InvasionAllemand)
            {
                calendarEvent[i].probaAppear -= proba;
                if(calendarEvent[i].probaAppear <= 0)
                {
                    calendarEvent.RemoveAt(i);
                    gameManager.uiManager.infoTranche = "Attention, un éclaireur allemand est aperçu par Léon !";
                    return;
                }
            }
        }
    }

    private IEnumerator MiseEnSceneBombarde(int nb)
    {
        yield return new WaitForSeconds(2);
        if (eventBombardement != null)
        {
            eventBombardement(degatBombardement);
            print("bombard !!!");
            //inserer effet bombardement
        }
        if (nb < 6)
        {
            StartCoroutine(MiseEnSceneBombarde(nb + 1));
        }
        else
        {
            gameManager.uiManager.infoTranche = "Fin des bombardements !!!";
        }
    }

    private IEnumerator ExplosionsBombarde()
    {
        listParticleBomb[0].Play();
        yield return new WaitForSeconds(2f);
        listParticleBomb[1].Play();
        yield return new WaitForSeconds(1f);
        listParticleBomb[2].Play();
        yield return new WaitForSeconds(1f);
        listParticleBomb[3].Play();
        yield return new WaitForSeconds(2f);
        listParticleBomb[4].Play();

        yield return new WaitForSeconds(0.5f);

        listParticleBomb[0].Play();
        yield return new WaitForSeconds(1.5f);
        listParticleBomb[1].Play();
        yield return new WaitForSeconds(1f);
        listParticleBomb[2].Play();
        yield return new WaitForSeconds(1f);
        listParticleBomb[3].Play();
        yield return new WaitForSeconds(2f);
        listParticleBomb[4].Play();


    }
    #endregion
}
