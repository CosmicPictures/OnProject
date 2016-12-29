using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class blinkText : MonoBehaviour {

    public float blinkDuration = 1.0f;

	// Use this for initialization
	void Start () {
        Text t = GetComponent<Text>();
        t.DOBlendableColor(Color.red, blinkDuration).SetLoops(-1, LoopType.Yoyo);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
