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
    public GameObject musicButtons;
    public enum Interaction { Fire, Webcam, Video, Music, Coffee, Clean, Light, TurnOn, Information };
    public Text informationText;
    public Text moreText;
    private Toggle toggle;
    public Vector3 offset;
    public float tweenDuration = 1.0f;
    private RectTransform rect;
    public GameObject[] upperBullets;
    public GameObject[] lowerBullets;

    public Interaction toggleInteraction;
    public bool disableOnClick = false;
    public Collider disableOther;
    public GameObject enableObject;

    private AudioSource source;

    private int numColliding = 0;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
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

    
    private void OnEnable()
    {
        if(toggleInteraction == Interaction.Fire)
        {
            if (pc)
            {
                if (pc.fireEnabled)
                {
                    toggle.isOn = true;
                }
            }
        }
    }
    

    private void OnTriggerEnter(Collider other)
    {
        numColliding++;

        if (pc.canPressButton && numColliding <=1 )
        {
            switch (toggleInteraction)
            {
                case Interaction.Fire:
                    pc.toggleFire();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Interaction.Webcam:
                    if (pc.enableWebcam)
                    {
                        pc.toggleWebcam();
                    }
                        simulateClick();
                        StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Interaction.Video:
                    pc.toggleTV();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Interaction.Music:
                    if (musicButtons.activeInHierarchy)
                        musicButtons.SetActive(false);
                    else
                        musicButtons.SetActive(true);
                    pc.toggleSpeakerClips();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Interaction.Coffee:
                    pc.toggleCoffee();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Interaction.Clean:
                    pc.toggleRoomba();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Interaction.Light:
                    pc.toggleLamp();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Interaction.Information:
                    toggleText();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, false, true));
                    break;
                case Interaction.TurnOn:
                    //GetComponent<Collider>().enabled = false;
                    if (toggles.activeInHierarchy)
                        toggles.SetActive(false);
                    else
                        toggles.SetActive(true);
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                default:
                    break;
            }
            if(disableOnClick)
            {
                GetComponent<Collider>().enabled = false;
                if(GetComponent<Image>())
                    GetComponent<Image>().enabled = false;
                foreach(Image i in GetComponentsInChildren<Image>())
                {
                    i.enabled = false;
                }
                //gameObject.SetActive(false);
                if (disableOther)
                {
                    disableOther.enabled = false;
                    if (enableObject)
                        enableObject.SetActive(true);
                }
            }
        }
#if UNITY_EDITOR
        Debug.Log("pressed " + toggleInteraction.ToString());
#endif  

    }

    private void OnTriggerExit(Collider other)
    {
        numColliding--;
    }

    private void toggleText()
    {
        if (informationText.enabled)
        {
            informationText.enabled = false;
            moreText.enabled = true;
            if (upperBullets.Length > 0)
            {
                foreach (GameObject g in upperBullets)
                {
                    if(g)
                        g.SetActive(false);
                }
            }
            if (lowerBullets.Length > 0)
            {
                foreach (GameObject g in lowerBullets)
                {
                    if(g)
                        g.SetActive(true);
                }
            }
        }
        else
        {
            informationText.enabled = true;
            moreText.enabled = false;
            foreach (GameObject g in upperBullets)
            {
                if(g)
                    g.SetActive(true);
            }
            foreach (GameObject g in lowerBullets)
            {
                if(g)
                    g.SetActive(false);
            }
        }
    }

    public void setToDefaultText()
    {
        informationText.enabled = true;
        moreText.enabled = false;
        if(toggle)
            toggle.isOn = false;
        foreach (GameObject g in upperBullets)
        {
            if (g)
                g.SetActive(true);
        }
        foreach (GameObject g in lowerBullets)
        {
            if (g)
                g.SetActive(false);
        }
    }

    public void simulateClick()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(toggle.gameObject, pointer, ExecuteEvents.submitHandler);
        if(source)
        {
            source.Play();
        }
    }
}
