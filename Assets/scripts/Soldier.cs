using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.AI;

public class Soldier : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 posDepart;
    private Camera mainCamera;
    public LayerMask layerDrag;
    private NavMeshAgent agent;

    //property soldier
    [SerializeField] string nameSoldier;
    [SerializeField] EnumDefine.RankMilitary rangSoldier;
    [SerializeField] EnumDefine.Perks[] competence;
    private List<EnumDefine.Perks> listPerks;
    private float stress;
    private float hp;

    private Spot hasTask = null;
    private bool hasWork = false;

    //[SerializeField] int toleranceTask = 5;
    [SerializeField] float ratioStressTolerance = 1.2f;
    public float tolerance { get; set; }
    public string lastTask { get; set; }

    [SerializeField] float speedMove; // a voir

    public void OnBeginDrag(PointerEventData eventData)
    {
        print("drag begin");
        posDepart = transform.position;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Ray ray = eventData.pressEventCamera.ScreenPointToRay(eventData.position);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, float.MaxValue, layerDrag))
            transform.position = hit.point.WithY(0.3f);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, mainCamera.transform.forward, out hit) && hit.transform.tag == "Spot")
        {
            transform.position = hit.transform.position.WithY(0.3f);
        }
        else
        {
            transform.position = posDepart;
        }
    }

    private void OnMouseDown()
    {
        GameManager.instance.soldierSelected = this;
        //affiche ui
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasTask && ReferenceEquals(other.GetComponent<Spot>(), hasTask) && !hasWork)
        {
            agent.isStopped = true;
            hasWork = true;
            hasTask.SetTaskSoldier(this);
        }
    }

    // Use this for initialization
    void Awake () {
        mainCamera = Camera.main;
        listPerks = new List<EnumDefine.Perks>();
        if(competence.Length > 0)
        {
            listPerks.AddRange(competence);
        }
        agent = GetComponent<NavMeshAgent>();
	}
	
	// Update is called once per frame
	void Update () {

	}

    public void SetDestination(Vector3 pos, Spot spot)
    {
        if(hasTask != null)
        {
            hasTask.BreakTask(this);
            hasTask.FreePlace();
        }
        hasTask = spot;
        agent.isStopped = false;
        agent.SetDestination(pos);
    }

    public void BackWaitingZone()
    {
        if(hasTask != null)
        {
            //hasTask.BreakTask(this);
            hasTask.FreePlace();
            //go zone waiting
        }
        hasWork = false;
        hasTask = null;
        agent.isStopped = false;
        agent.SetDestination(GameManager.instance.spotWaiting.transform.position);
    }

    public bool HasPerk(EnumDefine.Perks perk)
    {
        for(int i = 0; i < listPerks.Count; i++)
        {
            if(perk == listPerks[i])
            {
                return true;
            }
        }
        return false;
    }

    public void ResultEndTaskStress(string taskName, float stressTask)
    {
        if(taskName == lastTask && stressTask >= 0)// a voir
        {
            tolerance *= ratioStressTolerance;
        }
        else
        {
            tolerance = 1;
        }
        stress = stressTask * tolerance;
    }

    public void EndCurrentWork()
    {
        hasWork = false;
    }
}
