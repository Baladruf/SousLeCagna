using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spot : MonoBehaviour {

    [SerializeField] int nbPlaceSoldier = 1;
    [SerializeField] Transform[] posForSoldiers;
    private int actualPlace = 0;

    private void Awake()
    {
        listTask = new SoldierTask[nbPlaceSoldier];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		for(int i = 0; i < listTask.Length; i++)
        {
            if(listTask[i] != null && listTask[i].statActiveTask)
            {
                listTask[i].timer += TimeManager.instance.speedTime;
                if(listTask[i].timer >= listTask[i].delay)
                {
                    EndTask(listTask[i]);
                }
            }
        }
	}

    private void OnMouseEnter()
    {
        GameManager.instance.uiManager.uIAtelier.ActiveUI(iconAtelier, descriptionAtelier);
        GameManager.instance.uiManager.uISoldier.ResetUI();
        GameManager.instance.ResetCursor();
    }

    #region Assignation
    private void OnMouseDown()
    {
        GameManager.instance.spotSelected = this;
    }

    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1))
        {
            Soldier soldier = GameManager.instance.soldierSelected;
            if(soldier != null && actualPlace < nbPlaceSoldier)
            {
                soldier.SetDestination(transform.position, this);
                soldier.lastTask = name;
                listTask[actualPlace] = new SoldierTask(soldier, delayTask * RatioPerkTimer(soldier));
                actualPlace++;
            }
        }
    }

    public void FreePlace()
    {
        actualPlace--;
    }

    public void SetTaskSoldier(Soldier soldier)
    {
        soldier.transform.position = posForSoldiers[actualPlace - 1].position;
        for (int i = 0; i < listTask.Length; i++)
        {
            if(listTask[i] != null && ReferenceEquals(listTask[i].soldier, soldier))
            {
                listTask[i].statActiveTask = true; //   obsolete ->    soldier.coroutin = StartCoroutine(TimerTask(listTask[i]));
                break;
            }
        }
        //set animation
        //demarrer timer
    }
    #endregion


    #region TaskEffect
    private class SoldierTask
    {
        public Soldier soldier;
        public float delay;
        public bool statActiveTask;
        //public Coroutine coroutine;
        public float timer;

        public SoldierTask(Soldier p_soldier, float p_timer)
        {
            soldier = p_soldier;
            delay = p_timer;
            statActiveTask = false;
            //coroutine = null;
            timer = 0;
        }
    }

    [SerializeField] bool spotLooping = false;
    private SoldierTask[] listTask;
    [SerializeField] float delayTask = 5;
    public EnumDefine.Perks perkForTask;
    [SerializeField] float rationPerkTask = 0.5f;
    [SerializeField] float stressTask = 5;

    private IEnumerator TimerTask(SoldierTask task)
    {
        yield return new WaitForSeconds(task.delay);
        EndTask(task);
    }

    private void EndTask(SoldierTask soldierTask)
    {
        soldierTask.soldier.ResultEndTaskStress(name, stressTask);
        //gain de fin de tache
        print("tache finit");
        if (spotLooping)
        {
            soldierTask.timer = 0;     // obsolete -> soldierTask.coroutin = StartCoroutine(TimerTask(soldierTask));
        }
        else
        {
            soldierTask.soldier.BackWaitingZone();
            soldierTask.soldier = null;
            soldierTask.statActiveTask = false;
        }
        //stress et co
        //soldier -> back
    }

    public void BreakTask(Soldier soldier)
    {
        SoldierTask soldierTask = null;
        for(int i = 0; i < listTask.Length; i++)
        {
            if(ReferenceEquals(listTask[i].soldier, soldier))
            {
                soldierTask = listTask[i];
                break;
            }
        }
        if(soldierTask == null)
        {
            return;
        }
        soldierTask.soldier.EndCurrentWork();
        soldierTask.soldier = null;
        soldierTask.statActiveTask = false;
        /*if (soldierTask.coroutine != null) {
            StopCoroutine(soldierTask.coroutine);
        }*/
        soldierTask = null;
    }

    private float RatioPerkTimer(Soldier soldier)
    {
        return perkForTask != EnumDefine.Perks.nothings && soldier.HasPerk(perkForTask) ? rationPerkTask : 1;
    }
    #endregion

    #region UIPrint
    Sprite iconAtelier;
    Text descriptionAtelier;
    #endregion
}
