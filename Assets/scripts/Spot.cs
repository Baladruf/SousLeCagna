using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spot : MonoBehaviour {

    [SerializeField] int nbPlaceSoldier = 1;
    [SerializeField] Transform[] posForSoldiers;
    private int actualPlace = 0;
    [SerializeField] bool spotLooping = false;
    private Coroutine[] listTask;
    [SerializeField] float delayTask = 5;

    private class TaskSoldier : MonoBehaviour
    {
        public Soldier soldier;
        public float timer;
        public Coroutine task;
        public void EndTask()
        {
            //stress et co
            //soldier -> back
            soldier = null;
            timer = 0;
        }

        public void BreakTask()
        {
            soldier = null;
            timer = 0;
            if (task != null)
            {
                StopCoroutine(task);
            }
        }
    }

    private void Awake()
    {
        listTask = new Coroutine[nbPlaceSoldier];
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

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
        //set animation
        //demarrer timer
    }

    private IEnumerator TimerTask(Soldier soldier)
    {
        yield return new WaitForSeconds(delayTask);
        //stress et co
    }
}
