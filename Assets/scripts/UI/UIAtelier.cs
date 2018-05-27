using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIAtelier : MonoBehaviour {

    [SerializeField] Image iconAtelier;
    [SerializeField] Text description;

	public void ActiveUI(Sprite icon, Text descrip, Transform pos)
    {
        iconAtelier.sprite = icon;
        description.text = descrip.text;
        iconAtelier.transform.position = pos.position.WithRelativeY(1);
        iconAtelier.gameObject.SetActive(true);
    }
	
	public void DesactiveUI()
    {
        iconAtelier.gameObject.SetActive(false);
    }
}
