using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public GameObject firePS;
    public MovieTexture movie;
    public RawImage TVScreen;
    public RawImage WebcamScreen;
    private WebCamTexture webcamTex;
    public bool fireEnabled = true;
    public bool movieEnabled = true;
    public bool webcamEnabled = false;
    public int targetFPS = 90;

	// Use this for initialization
	void Start () {

        Application.targetFrameRate = targetFPS;
        QualitySettings.vSyncCount = 0;
        movie.Stop();

        webcamTex = new WebCamTexture();
        webcamTex.requestedWidth = (int)WebcamScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        webcamTex.requestedHeight = (int)WebcamScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        toggleFire();
        toggleTV();
        toggleWebcam();
        
	}

    private void toggleTV()
    {
        if(!movieEnabled)
        {
            movie = (MovieTexture)TVScreen.texture;
            movie.Pause();
            TVScreen.gameObject.SetActive(false);
        }
        else
        {
            webcamEnabled = false;
            toggleWebcam();
            movie = (MovieTexture)TVScreen.texture;
            TVScreen.gameObject.SetActive(true);
            if (!movie.isPlaying)
                movie.Play();
        }
    }
    private void toggleWebcam()
    {
        if (!webcamEnabled)
        {
            WebcamScreen.gameObject.SetActive(false);

        }
        else
        {

            movieEnabled = false;
            toggleTV();
            WebcamScreen.texture = webcamTex;
            webcamTex.Play();

            WebcamScreen.gameObject.SetActive(true);
            //movie = (MovieTexture)WebcamScreen.texture;
            //movie.Play();
        }
    }

    // Update is called once per frame
    void Update () {
		
        if(Input.GetKeyDown(KeyCode.Escape))
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }


        if (Input.GetKeyDown(KeyCode.F))
        {
            fireEnabled = !fireEnabled;
            toggleFire();

        }
        if(Input.GetKeyDown(KeyCode.T))
        {
            movieEnabled = !movieEnabled;
            toggleTV();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            webcamEnabled = !webcamEnabled;
            toggleWebcam();
        }
    }

    void toggleFire()
    {
        foreach (ParticleSystem ps in firePS.GetComponentsInChildren<ParticleSystem>())
        {
            if (!fireEnabled)
                ps.Stop();
            else
                ps.Play();
        }
        foreach (Light l in firePS.GetComponentsInChildren<Light>())
        {
            if (!fireEnabled)
            {
                l.gameObject.SetActive(false);

            }
            else
                l.gameObject.SetActive(true);
        }
        AudioSource source = firePS.GetComponent<AudioSource>();
        if(source)
        {
            if (!fireEnabled)
            {
                source.Stop();
            }
            else
                source.Play();
        }
    }
}
