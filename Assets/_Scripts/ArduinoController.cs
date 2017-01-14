


using UnityEngine;
using System.Collections;
using System.Collections.Generic;       //Allows us to use Lists. 

public class ArduinoController : MonoBehaviour
{
    public SerialBoardController sbc;

    public bool fanOn;
    public bool heaterOn;

    public float signalSendDelay = 0.25f;

    private void Start()
    {
        StartCoroutine(turnBothOff());
    }

    public IEnumerator turnBothOff()
    {
        yield return new WaitForSeconds(signalSendDelay);
        turnFanOff();
        yield return new WaitForSeconds(signalSendDelay);
        turnHeaterOff();
        yield return null;
    }

    public void resetBoth()
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

    public IEnumerator turnFanOnAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        sbc.sendMessage("1");
        fanOn = true;
        yield return null;
    }
    public void turnFanOff()
    {
        sbc.sendMessage("0");
        fanOn = false;
    }
    public IEnumerator turnFanOffAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        sbc.sendMessage("0");
        fanOn = false;
        yield return null;
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

    
    void OnApplicationQuit()
    {
        turnFanOff();
        turnHeaterOff();
    }
    
    

}