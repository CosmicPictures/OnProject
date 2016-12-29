using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class fadeOverTime : MonoBehaviour {

    public float fadeTime = 3.0f;
    public textPopupOnLook disableScript;

	// Use this for initialization
	void Start () {
		
	}
	
    void OnEnable()
    {
        
        StartCoroutine(startFadeAfterTime(fadeTime));
        //t.DOColor(new Color(t.color.r, t.color.g, t.color.b, 0f), fadeTime);
    }

    IEnumerator startFadeAfterTime(float time)
    {
        disableScript.enabled = false;
        yield return new WaitForSeconds(time);
        
        Text t = GetComponentInParent<Text>();
        t.DOColor(new Color(t.color.r, t.color.g, t.color.b, 0f), fadeTime);
        Image i = GetComponent<Image>();
        i.DOFade(0, fadeTime);
    }

	// Update is called once per frame
	void Update () {
		
	}
}
