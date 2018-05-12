using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RTS_Cam;

public class GroundOption : MonoBehaviour {

    private RTS_Camera mainCamera;

    private void Awake()
    {
        mainCamera = Camera.main.GetComponent<RTS_Camera>();
    }

    private void Update()
    {
        if(mainCamera.targetFollow != null)
        {
            if(Input.GetAxis("Horizontal") != 0 || Input.GetAxis("Vertical") != 0)
            {
                mainCamera.targetFollow = null;
            }
        }
    }

    private void OnMouseDown()
    {
        mainCamera.targetFollow = null;
    }
}
