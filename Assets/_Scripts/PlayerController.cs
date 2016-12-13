using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {

    public GameObject firePS;
    public MovieTexture movie;
    private AudioClip movieAudio;
    public AudioSource movieAudioSource;
    public AudioSource[] speakers;
    public AudioClip[] speakerClips;
    public AudioClip[] currentClips;
    public RawImage TVScreen;
    public RawImage WebcamScreen;
    public RawImage securityScreen;
    private WebCamTexture webcamTex;
    private WebCamTexture securityTex;
    private int speakerClipIndex = 0;
    public bool fireEnabled = true;
    public bool movieEnabled = true;
    public bool webcamEnabled = false;
    public int targetFPS = 90;

	// Use this for initialization
	void Start () {

        Application.targetFrameRate = targetFPS;
        //QualitySettings.vSyncCount = 0;
        currentClips = new AudioClip[speakers.Length];
        movie.Stop();
        movieAudioSource = TVScreen.GetComponent<AudioSource>();
        movieAudio = movie.audioClip;
        movieAudioSource.clip = movieAudio;
        webcamTex = new WebCamTexture();
        webcamTex.requestedWidth = (int)WebcamScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        webcamTex.requestedHeight = (int)WebcamScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.y;

        securityTex = webcamTex;
        securityTex.requestedWidth = (int)securityScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
        securityTex.requestedHeight = (int)securityScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
        securityScreen.texture = securityTex;
        securityTex.Play();

        toggleFire();
        toggleTV();
        toggleWebcam();
        
	}

    private void toggleTV()
    {
        if(!movieEnabled)
        {
            movie = (MovieTexture)TVScreen.texture;
            movie.Stop();
            movieAudioSource.Stop();
            //movie.Pause();
            //movieAudioSource.Pause();
            
            TVScreen.gameObject.SetActive(false);
        }
        else
        {
            webcamEnabled = false;
            toggleWebcam();
            movie = (MovieTexture)TVScreen.texture;
            TVScreen.gameObject.SetActive(true);
            if (!movie.isPlaying)
            {
                movie.Play();
                movieAudioSource.Play();
            }
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
        if(Input.GetKeyDown(KeyCode.M))
        {
            toggleSpeakerClips();
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

    void toggleSpeakerClips()
    {
        speakerClipIndex++;

        if (speakerClipIndex >= speakerClips.Length)
            speakerClipIndex = 0;

        if (speakerClips[speakerClipIndex])
        {
            for(int i = 0; i < currentClips.Length; i++)
            {
                speakers[i].Stop();
                if(currentClips[i])
                    Destroy(currentClips[i]);
                currentClips[i] = Instantiate(speakerClips[speakerClipIndex]);
                speakers[i].clip = currentClips[i];
                speakers[i].Play();
            }   
        }
        else
        {
            foreach(AudioSource source in speakers)
            {
                source.Stop();
            }
        }
    }
}
