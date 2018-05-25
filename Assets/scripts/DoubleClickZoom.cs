using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class DoubleClickZoom : MonoBehaviour {

    private bool firstClick = false;
    private bool isTimerRunning = false;
    private float timerClick = 0;
    [SerializeField] float delayBetweenClick = 0.3f;


    private Camera mainCamera;
    private RTS_Camera rtsCam;
    private float zoomMax;
    [SerializeField] float delayTransition = 0.5f;
    private static Transform transformTransition;
    private float timerTransition = 0;
    [SerializeField] float distanceBetweenCameraAndTarget = -2;

    private void Awake()
    {
        mainCamera = Camera.main;
        rtsCam = mainCamera.GetComponent<RTS_Camera>();
        zoomMax = rtsCam.minHeight;
    }

    // Update is called once per frame
    void Update () {
        //DoubleClick();
        TransitionIntoTarget();
	}

    private void DoubleClick()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (!firstClick)
            {
                firstClick = true;
                timerClick = 0;
            }
            else
            {
                firstClick = false;

                transformTransition = transform;
                timerTransition = 0;
            }
        }

        if (firstClick)
        {
            timerClick += Time.deltaTime;
            if (timerClick > delayBetweenClick)
            {
                firstClick = false;
            }
        }
    }

    private void TransitionIntoTarget()
    {
        if(transformTransition != null && ReferenceEquals(transformTransition, transform))
        {
            timerTransition += Time.deltaTime;
            if(timerTransition <= delayBetweenClick)
            {
                float valLerp = timerTransition / delayBetweenClick;
                rtsCam.SetZoomPos = 0;
                mainCamera.transform.position = new Vector3(Mathf.Lerp(mainCamera.transform.position.x, transform.position.x, valLerp), Mathf.Lerp(mainCamera.transform.position.y, zoomMax, valLerp), Mathf.Lerp(mainCamera.transform.position.z, transform.position.z + distanceBetweenCameraAndTarget, valLerp));

            }
            else
            {
                transformTransition = null;
                rtsCam.targetFollow = transform;
            }
        }
    }

    private void OnMouseDown()
    {
        DoubleClick();
    }
}
