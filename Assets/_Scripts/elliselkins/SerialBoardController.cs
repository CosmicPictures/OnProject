using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports; 

public class SerialBoardController : MonoBehaviour {
	public delegate void MsgReceived(string msg);
	public event MsgReceived OnMsgReceived;


#if UNITY_EDITOR_OSX || UNITY_STANDALONE_OSX
	protected SerialPort sp = new SerialPort("/dev/cu.usbserial-A600ahi9", 9600);
#else
	protected SerialPort sp = new SerialPort("COM7", 9600);
#endif
	protected string bufferStr = "";

	// Use this for initialization
	protected void Start () {
		startReading();
	}

	public void sendMessage(string msg)
	{
		if (!sp.IsOpen)
			return;

		sp.Write(msg);
		Debug.Log("Sending msg: " +  msg);
	}

	protected void startReading()
	{
		try {
			sp.ReadTimeout = 2;
			sp.Open();
			InvokeRepeating("readData", 0.001f, .008f);
		}
		catch(System.Exception e) {
			Debug.Log ("connection failed to arduino: " + e.Message);
		}
	}

	protected void readData()
	{
		if (!sp.IsOpen)
			return;

		bool error = false;

		while(!error)
		{
			try {
				char c = (char)sp.ReadChar();
				bufferStr += c;
				if (c == '\n') {
					dispatchMsg();
				}
			}
			catch(System.Exception) {
				error = true;
			}
		}
	}

	protected void dispatchMsg()
	{
		if(OnMsgReceived != null) {
			OnMsgReceived(bufferStr);
		}
		//Debug.Log("Msg received: " + bufferStr);
		bufferStr = "";
	}
}
