using UnityEngine;

public class SerialCommunicationExample : MonoBehaviour {

	public SerialBoardController sbc;
	//public Light spotlight;

	// Use this for initialization
	void Awake () {
	    //sbc.OnMsgReceived += OnMsgReceived;
	}

    protected void Update()
    {
        if(OVRInput.GetDown(OVRInput.Button.Two))
        {
            toggleFan();
        }
        else if(OVRInput.GetDown(OVRInput.Button.Three))
        {
            toggleHeater();
        }
    }

    protected bool fanOn;
    protected bool heaterOn;

    public void toggleFan()
	{
		if (fanOn) {
			sbc.sendMessage("0");
			fanOn = false;
		}
		else {
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

    //protected void OnMsgReceived(string msg)
    //{
    //	//Debug.Log(msg);
    //	//return;
    //	float value;

    //	try {
    //		value = float.Parse(msg);
    //		value = value / 600 * 8;
    //		spotlight.intensity = value;
    //	}
    //	catch { }

    //}
}
