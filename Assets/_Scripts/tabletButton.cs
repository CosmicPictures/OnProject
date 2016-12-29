using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class tabletButton : MonoBehaviour
{

    PlayerController pc;
    public enum Action { Previous, Next };
    private Button button;

    public Action buttonAction;

    private AudioSource source;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        pc = GameObject.Find("OVRCameraRig").GetComponent<PlayerController>();
        button = GetComponent<Button>();
        //rect = rect.GetComponent<RectTransform>();

        if (!pc)
            Debug.Log("Could not find player controller!");


        if (!button)
            Debug.Log("No button found!");

    }

    // Update is called once per frame
    void Update()
    {
        /*
        switch (buttonAction)
        {
            case Action.Previous:
                if ((pc.webcamEnabled && !toggle.isOn) || (!pc.webcamEnabled && toggle.isOn))
                    simulateClick();
                break;
            case Action.Next:
                if ((pc.movieEnabled && !toggle.isOn) || (!pc.movieEnabled && toggle.isOn))
                    simulateClick();
                break;
            default:
                break;
        }
        */
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
            switch (buttonAction)
            {
                case Action.Previous:
                    pc.previousSong();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                case Action.Next:
                    pc.nextSong();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown));
                    break;
                default:
                    break;
            }
        }
#if UNITY_EDITOR
        Debug.Log("pressed " + buttonAction.ToString());
#endif  

    }

    private void simulateClick()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
        if (source)
        {
            source.Play();
        }
    }
}
