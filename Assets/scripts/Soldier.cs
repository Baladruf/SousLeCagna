using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Soldier : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Vector3 posDepart;
    private Camera mainCamera;
    public LayerMask layerDrag;

    //property soldier
    [SerializeField] string nameSoldier;
    [SerializeField] EnumDefine.RankMilitary rangSoldier;
    [SerializeField] EnumDefine.Perks[] competence;
    private List<EnumDefine.Perks> listPerks;
    private float stress;
    private float hp;

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

    // Use this for initialization
    void Awake () {
        mainCamera = Camera.main;
        listPerks = new List<EnumDefine.Perks>();
        if(competence.Length > 0)
        {
            listPerks.AddRange(competence);
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
