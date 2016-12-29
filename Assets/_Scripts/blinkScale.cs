using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class blinkScale : MonoBehaviour {

    private Vector3 initialScale;
    public float scaleUpRatio = 1.2f;
    public float scaleDuration = 1.0f;

	// Use this for initialization
	void Start () {
        initialScale = GetComponent<RectTransform>().localScale;

        GetComponent<RectTransform>().DOScale(scaleUpRatio, scaleDuration).SetLoops(-1, LoopType.Yoyo);

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
