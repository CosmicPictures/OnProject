using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;

public class thermostatButton : MonoBehaviour {

    PlayerController pc;
    private AudioSource source;

    private int numColliding = 0;
    private tabletToggle toggleOther;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        pc = GameObject.Find("OVRCameraRig").GetComponent<PlayerController>();

        if (!pc)
            Debug.Log("Could not find player controller!");

    }

    private void OnTriggerEnter(Collider other)
    {
        numColliding++;

        if (numColliding <= 1)
        {
            pc.toggleFire();
            pc.TriggerHapticPulse(0.1f, OVRInput.Controller.RTouch);
            if (!toggleOther)
                toggleOther = GameObject.Find("FireToggle").GetComponent<tabletToggle>();

            if(toggleOther)
                toggleOther.simulateClick();
        }

    }

    private void OnTriggerExit(Collider other)
    {
        numColliding--;
    }
}
