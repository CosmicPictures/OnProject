


using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class ArduinoController : MonoBehaviour
{

    public static ArduinoController instance = null;

    public SerialBoardController sbc;

    public bool fanOn;
    public bool heaterOn;


    //Awake is always called before any Start functions
    void Awake()
    {
        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)

            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        turnFanOff();
        turnHeaterOff();
    }

    public void toggleFan()
    {
        if (fanOn)
        {
            sbc.sendMessage("0");
            fanOn = false;
        }
        else
        {
            sbc.sendMessage("1");
            fanOn = true;
        }
    }

    public void toggleHeater()
    {
        if (heaterOn)
        {
            sbc.sendMessage("2");
            heaterOn = false;
        }
        else
        {
            sbc.sendMessage("3");
            heaterOn = true;
        }
    }

    public void turnFanOn()
    {
        sbc.sendMessage("1");
        fanOn = true;
    }
    public void turnFanOff()
    {
        sbc.sendMessage("0");
        fanOn = false;
    }

    public void turnHeaterOn()
    {
        sbc.sendMessage("3");
        heaterOn = true;
    }
    public void turnHeaterOff()
    {
        sbc.sendMessage("2");
        heaterOn = false;
    }

    private void OnApplicationQuit()
    {
        turnFanOff();
        turnHeaterOff();
    }

}