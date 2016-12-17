using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class tabletButton : MonoBehaviour {

    PlayerController pc;

    public enum Interaction {Fire, Webcam, Video, Music, Coffee, Clean};
    private Button button;


    public Interaction buttonInteraction;

	// Use this for initialization
	void Start () {

        pc = GameObject.Find("OVRCameraRig").GetComponent<PlayerController>();

        if (!pc)
            Debug.Log("Could not find player controller!");

        button = GetComponent<Button>();
        if (!button)
            Debug.Log("No button found!");

	}
	
	// Update is called once per frame
	void Update () {
		
	}


    private void OnTriggerEnter(Collider other)
    {
        if (pc.canPressButton)
        {
            switch (buttonInteraction)
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
                    break;
                default:
                    break;
            }
        }
#if UNITY_EDITOR
        Debug.Log("pressed "+ buttonInteraction.ToString());
#endif  

    }

    private void simulateClick()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
    }
}
