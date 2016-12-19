using UnityEngine;
using WeMo;
using System.Collections.Generic;

//From https://www.youtube.com/watch?v=ifzmJFdvNEE
public class wemoInteraction : MonoBehaviour {

    public enum Action
    {
        On,
        Off
    }

    public string targetDevice;
    private WeMoSwitch wemoSwitch;

    void Start()
    {
#if UNITY_EDITOR
        Debug.Log("Searching for device {0}..." + targetDevice);
#endif
        wemoSwitch = (WeMoSwitch)WeMoDevice.GetDevice(targetDevice);

        if (wemoSwitch == null)
        {
#if UNITY_EDITOR
            Debug.Log("Could not find device: " + targetDevice);
#endif
        }
    }

    void Update () {

            if (Input.GetKeyDown(KeyCode.Keypad0))
            {
                toggleDevice(Action.Off);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                toggleDevice(Action.On);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                checkForDevice();
            }
        
    }

    public void toggleDevice(Action action)
    {
        if(wemoSwitch != null)
        switch (action)
        {
            case Action.On:
                wemoSwitch.On();
#if UNITY_EDITOR
                Debug.Log(targetDevice + " switch has been turned on");
#endif
                break;

            case Action.Off:
                wemoSwitch.Off();
#if UNITY_EDITOR
                Debug.Log(targetDevice + " switch has been turned off");
#endif
                break;
        }
        else
        {
#if UNITY_EDITOR
            Debug.Log(targetDevice + " was not found");
#endif
        }

    }
    
    public void checkForDevice()
    {
        Debug.Log("Searching...");
        List<WeMoDevice> devices = WeMoDevice.GetDevices();

        if (devices.Count > 0)
        {
            Debug.Log("Found Device(s)        ");
            Debug.Log("=========================");
            foreach (WeMoDevice device in devices)
            {
                Debug.Log("Name: {0}" + device.Name);
            }
        }
        else
        {
            Debug.Log("\rFailed to find WeMo devices on your network");
        }
    }
}
