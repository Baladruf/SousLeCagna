using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class EndDayManager : MonoBehaviour {

    [System.NonSerialized]
    public GameManager gameManager;
    [SerializeField] Image transitionFade;
    [SerializeField] GameObject mainCamera, cameraEndDay;
    [SerializeField] float durationFade = 2;
    [SerializeField] Transform[] positionEnd;
    [SerializeField] GameObject uiDesactive;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        /*if (Input.GetKeyDown(KeyCode.M))
        {
            CallEndDay();
        }*/
	}

    public void CallEndDay()
    {
        transitionFade.DOFade(1, durationFade).OnComplete(()=>
        {
            mainCamera.SetActive(false);
            cameraEndDay.SetActive(true);
            uiDesactive.SetActive(false);
            var soldiers = gameManager.playerManager.soldiers;
            for(int i = 0; i < soldiers.Count; i++)
            {
                soldiers[i].BreakTask();
                soldiers[i].transform.position = positionEnd[i].position;
                soldiers[i].transform.rotation = positionEnd[i].rotation;
            }
            transitionFade.DOFade(0, durationFade).OnComplete(() =>
            {
                gameManager.letterManager.LetterEndDay();
            });
        });
    }
}
