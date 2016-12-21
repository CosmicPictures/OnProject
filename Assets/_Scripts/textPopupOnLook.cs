﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;
using UnityEngine.UI;
using DG.Tweening;

public class textPopupOnLook : MonoBehaviour {

    [SerializeField]
    private VRInteractiveItem m_InteractiveItem;
    public Text[] copy;
    public float fadeDuration = 1.0f;
    public bool rotateTowardCamera = false;
    public float maxRotateAngle = 30f;
    private Quaternion initialRotation;
    public float rotationDamping = 3f;
    public GameObject cam;

    private void Awake()
    {
        //copy.enabled = false;
        foreach (Text t in copy)
        {
            t.color = new Color(t.color.r, t.color.g, t.color.b, 0);

        }
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.color = new Color(img.color.r, img.color.g, img.color.b, 0);
        }
        initialRotation = copy[0].rectTransform.rotation;


    }

    private void Update()
    {
        if(copy[0].color.a > 0)
        {
            
            Vector3 from = new Vector3((-transform.forward).x, 0, (-transform.forward).z);
            Vector3 to = new Vector3((cam.transform.position - transform.position).x, 0, (cam.transform.position - transform.position).z);
            float angle = Mathf.Clamp( Vector3.Angle(from,to),-maxRotateAngle,maxRotateAngle);

            //copy.rectTransform.rotation.eulerAngles;
            copy[0].rectTransform.rotation = Quaternion.Euler(new Vector3(initialRotation.eulerAngles.x, initialRotation.eulerAngles.y - angle, initialRotation.eulerAngles.z));
            //copy.rectTransform.DORotate(new Vector3(initialRotation.eulerAngles.x,initialRotation.eulerAngles.y - angle, initialRotation.eulerAngles.z) , Time.deltaTime);
        }
    }


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        m_InteractiveItem.OnOut += HandleOut;
        //m_InteractiveItem.OnClick += HandleClick;
        //m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        m_InteractiveItem.OnOut -= HandleOut;
        //m_InteractiveItem.OnClick -= HandleClick;
        //m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
    }


    //Handle the Over event
    private void HandleOver()
    {
        foreach (Text t in copy)
        {
            t.DOColor(new Color(t.color.r, t.color.g, t.color.b, 1), fadeDuration);
        }
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.DOColor(new Color(img.color.r, img.color.g, img.color.b, 1), fadeDuration);
        }
            //copy.enabled = true;
            //Debug.Log("Show over state");
            //m_Renderer.material = m_OverMaterial;
    }


    //Handle the Out event
    
    private void HandleOut()
    {
        foreach (Text t in copy)
        {
            t.DOColor(new Color(t.color.r, t.color.g, t.color.b, 0), fadeDuration);
        }
        foreach (Image img in GetComponentsInChildren<Image>())
        {
            img.DOColor(new Color(img.color.r, img.color.g, img.color.b, 0), fadeDuration);
        }
        //Debug.Log("Show out state");

    }

    /*
    //Handle the Click event
    private void HandleClick()
    {
        Debug.Log("Show click state");
        m_Renderer.material = m_ClickedMaterial;
    }


    //Handle the DoubleClick event
    private void HandleDoubleClick()
    {
        Debug.Log("Show double click");
        m_Renderer.material = m_DoubleClickedMaterial;
    }
    */
}