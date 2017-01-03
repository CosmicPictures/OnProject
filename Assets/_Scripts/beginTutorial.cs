using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VR = UnityEngine.VR;

public class beginTutorial : MonoBehaviour {

    public GameObject[] hideObjects;
    public GameObject[] showObjects;

    private bool started = false;

	// Use this for initialization
	void Start () {
        foreach (GameObject g in showObjects)
        {
            g.SetActive(false);
        }
    }
	
	// Update is called once per frame
	void Update () {

        if(!started && (OVRInput.GetUp(OVRInput.Button.One) || OVRInput.GetUp(OVRInput.Button.Two) || OVRInput.GetUp(OVRInput.Button.Three) || OVRInput.GetUp(OVRInput.Button.Four) || OVRInput.GetUp(OVRInput.Button.PrimaryThumbstick)))
        {
            VR.InputTracking.Recenter();
            started = true;
            foreach(GameObject g in hideObjects)
            {
                g.SetActive(false);
            }
            foreach(GameObject g in showObjects)
            {
                g.SetActive(true);
            }
        }

    }
}
