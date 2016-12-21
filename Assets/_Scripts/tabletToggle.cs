using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class tabletToggle : MonoBehaviour
{

    PlayerController pc;
    public GameObject toggles;
    public enum Interaction { Fire, Webcam, Video, Music, Coffee, Clean, TurnOn, Information };
    public Text informationText;
    public Text moreText;
    private Toggle toggle;
    public Vector3 offset;
    public float tweenDuration = 1.0f;
    private RectTransform rect;
    public GameObject[] upperBullets;
    public GameObject[] lowerBullets;

    public Interaction toggleInteraction;

    // Use this for initialization
    void Start()
    {

        pc = GameObject.Find("OVRCameraRig").GetComponent<PlayerController>();
        toggle = GetComponent<Toggle>();    
        //rect = rect.GetComponent<RectTransform>();

        if (!pc)
            Debug.Log("Could not find player controller!");

        
        if (!toggle)
            Debug.Log("No toggle found!");

    }

    // Update is called once per frame
    void Update()
    {
        switch (toggleInteraction)
        {
            case Interaction.Webcam:
                if((pc.webcamEnabled && !toggle.isOn) || (!pc.webcamEnabled && toggle.isOn))
                    simulateClick();
                break;
            case Interaction.Video:
                if ((pc.movieEnabled && !toggle.isOn) || (!pc.movieEnabled && toggle.isOn))
                    simulateClick();
                break;
            case Interaction.Music:

                break;
            default:
                break;
        }
    }

    /*
    private void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        //Debug.Log(GetComponent<RectTransform>().anchoredPosition);
        rect.DOMove(rect.anchoredPosition3D + offset, tweenDuration);
        //transform.DOMove(transform.position + offset,tweenDuration);
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (pc.canPressButton)
        {
            switch (toggleInteraction)
            {
                case Interaction.Fire:
                    pc.toggleFire();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                case Interaction.Webcam:
                    pc.toggleWebcam();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                case Interaction.Video:
                    pc.toggleTV();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                case Interaction.Music:
                    pc.toggleSpeakerClips();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                case Interaction.Coffee:
                    break;
                case Interaction.Clean:
                    pc.toggleRoomba();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                case Interaction.Information:
                    toggleText();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                case Interaction.TurnOn:
                    //GetComponent<Collider>().enabled = false;
                    if (toggles.activeInHierarchy)
                        toggles.SetActive(false);
                    else
                        toggles.SetActive(true);
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                default:
                    break;
            }
        }
#if UNITY_EDITOR
        Debug.Log("pressed " + toggleInteraction.ToString());
#endif  

    }

    private void toggleText()
    {
        if (informationText.enabled)
        {
            informationText.enabled = false;
            moreText.enabled = true;
            foreach (GameObject g in upperBullets)
            {
                g.SetActive(false);
            }
            foreach(GameObject g in lowerBullets)
            {
                g.SetActive(true);
            }
        }
        else
        {
            informationText.enabled = true;
            moreText.enabled = false;
            foreach (GameObject g in upperBullets)
            {
                g.SetActive(true);
            }
            foreach (GameObject g in lowerBullets)
            {
                g.SetActive(false);
            }
        }
    }

    private void simulateClick()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(toggle.gameObject, pointer, ExecuteEvents.submitHandler);
    }
}
