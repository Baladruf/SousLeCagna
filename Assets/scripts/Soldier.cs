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
    private Animator animator;
    private Rigidbody rigi;

    //property soldier
    [SerializeField] string nameSoldier;
    [SerializeField] EnumDefine.RankMilitary rangSoldier;
    [SerializeField] EnumDefine.Perks[] competence;
    private List<EnumDefine.Perks> listPerks;

    [SerializeField] float maxMoral;
    private float moral;
    private float hp;

    private Spot hasTask = null;
    private bool hasWork = false;

    //[SerializeField] int toleranceTask = 5;
    [SerializeField] float ratioStressTolerance = 1.2f;
    public float tolerance { get; set; }
    public string lastTask { get; set; }

    [SerializeField] float speedMove; // a voir
    private string animEnCour;

    #region animation
    //private float speedAnim = 0;
    private float deratisse = 0;
    public bool isDeratisation
    {
        get
        {
            return deratisse == 1;
        }
        set
        {
            if (value)
            {
                deratisse = 1;
            }
            else
            {
                deratisse = 0;
            }
        }
    }

    public void SetAnimSpot(EnumDefine.AnimSpot anim)
    {
        switch (anim)
        {
            case EnumDefine.AnimSpot.fusil:
                break;
            case EnumDefine.AnimSpot.read:
                break;
            case EnumDefine.AnimSpot.read_letter:
                break;
            case EnumDefine.AnimSpot.eat:
                break;
            case EnumDefine.AnimSpot.drink:
                break;
            case EnumDefine.AnimSpot.hitReaction:
                break;
            case EnumDefine.AnimSpot.walkBox:
                break;
            case EnumDefine.AnimSpot.mitrailleuse:
                break;
            case EnumDefine.AnimSpot.research:
                break;
            case EnumDefine.AnimSpot.death:
                break;
        }
    }
    #endregion

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
        GameManager.instance.uiManager.uISoldier.SetIdentitySoldier(photo, rank, DescriptionSoldier());
        GameManager.instance.uiManager.uIAtelier.DesactiveUI();
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
    void Awake() {
        mainCamera = Camera.main;
        listPerks = new List<EnumDefine.Perks>();
        if (competence.Length > 0)
        {
            listPerks.AddRange(competence);
        }
        agent = GetComponent<NavMeshAgent>();
        animator = transform.GetChild(0).GetComponent<Animator>();
        rigi = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        GameManager.instance.eventManager.eventSoldier += EventEffect;
    }

    // Update is called once per frame
    void Update() {
        float speedA = agent.velocity.magnitude / agent.speed;
        //print("vitesse = " + speedA);
        animator.SetFloat("deratisation", deratisse);
        animator.SetFloat("speed", speedA);
    }

    public void SetDestination(Vector3 pos, Spot spot)
    {
        if (hasTask != null)
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
        if (hasTask != null)
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
        for (int i = 0; i < listPerks.Count; i++)
        {
            if (perk == listPerks[i])
            {
                return true;
            }
        }
        return false;
    }

    public void ResultEndTaskStress(string taskName, float stressTask)
    {
        if (taskName == lastTask && stressTask >= 0)// a voir
        {
            tolerance *= ratioStressTolerance;
        }
        else
        {
            tolerance = 1;
        }
        moral = stressTask * tolerance;
    }

    public void EndCurrentWork()
    {
        hasWork = false;
    }

    private void EventEffect(float perteHp, float perteMoral)
    {
        hp -= perteHp;
        if(hp <= 0)
        {
            //dead etc
            return;
        }

        moral = Mathf.Max(0, Mathf.Min(maxMoral, moral - perteMoral));
    }

    #region UIPrint
    [SerializeField] Sprite photo;
    [SerializeField] Sprite rank;

    public string[] DescriptionSoldier()
    {
        return new string[] { nameSoldier, rank.name, listPerks[0].ToString(), hp.ToString(), moral.ToString()};
    }
    #endregion
}
