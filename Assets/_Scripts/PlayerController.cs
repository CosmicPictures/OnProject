using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlayerController : MonoBehaviour {

    public GameObject firePS;
    public GameObject coffeePS;
    public GameObject roomba;
    public MovieTexture movie;
    private AudioClip movieAudio;
    public AudioSource movieAudioSource;
    public AudioSource[] speakers;
    public AudioSource masterSpeaker;
    public AudioSource[] slaveSpeakers;
    public AudioClip[] speakerClips;
    public AudioClip[,] loadClips;
    public RawImage TVScreen;
    public RawImage WebcamScreen;
    public RawImage securityScreen;
    private WebCamTexture webcamTex;
    private WebCamTexture securityTex;
    private int speakerClipIndex = 0;
    public bool fireEnabled = true;
    public bool movieEnabled = true;
    public bool webcamEnabled = false;
    public bool roombaEnabled = false;
    public bool coffeeEnabled = false;
    public bool speakersEnabled = false;
    public int targetFPS = 90;
    public float buttonCooldown = 0.25f;
    public bool canPressButton = true;
    public float hapticPulseDuration = 0.2f;
    private roomNavigation roombaScript;
    public float coffeeDelayTime = 5.0f;
    public float coffeeFillTime = 5.0f;
    public GameObject coffeeFill;
    private Tweener coffeeScaleTween;
    private Tweener coffeeMoveTween;
    private float coffeeInitialY;

	// Use this for initialization
	void Start () {
        coffeeInitialY = coffeeFill.transform.localPosition.y;
        Application.targetFrameRate = targetFPS;

        roombaScript = roomba.GetComponent<roomNavigation>();

        //QualitySettings.vSyncCount = 0;
        loadClips = new AudioClip[speakers.Length,speakerClips.Length];

        for(int i = 0; i < speakers.Length; i++)
        {
            for(int j = 0; j < speakerClips.Length; j++)
            {
                if(speakerClips[j])
                    loadClips[i, j] = Instantiate(speakerClips[j]);
            }
        }

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

        if (WebCamTexture.devices.Length > 0)
            securityTex.Play();
        else
            Debug.Log("No webcams found!");

           

        fireEnabled = !fireEnabled;
        toggleFire();

        movieEnabled = !movieEnabled;
        toggleTV();

        webcamEnabled = !webcamEnabled;
        toggleWebcam();

        if(speakers[0])
            masterSpeaker = speakers[0];

        if(speakers.Length > 1)
        {
            slaveSpeakers = new AudioSource[speakers.Length - 1];
            for(int i = 1; i < speakers.Length; i++)
            {
                slaveSpeakers[i - 1] = speakers[i];
            }
        }

        //Disable fire ps and sound on start
        foreach (ParticleSystem ps in firePS.GetComponentsInChildren<ParticleSystem>())
        {
            ps.Stop();
        }
        foreach (Light l in firePS.GetComponentsInChildren<Light>())
        {
            l.gameObject.SetActive(false);
        }
        AudioSource source = firePS.GetComponent<AudioSource>();
        if (source)
        {
            source.Stop();
        }

    }

    public void toggleTV()
    {
        movieEnabled = !movieEnabled;
        if (!movieEnabled)
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
            //disableWebcam();
            movie = (MovieTexture)TVScreen.texture;
            TVScreen.gameObject.SetActive(true);
            if (!movie.isPlaying)
            {
                movie.Play();
                movieAudioSource.Play();
            }
        }
    }

    public void disableWebcam()
    {
        webcamEnabled = false;
        WebcamScreen.gameObject.SetActive(false);
    }
    public void disableMovie()
    {
        movieEnabled = false;
        movie = (MovieTexture)TVScreen.texture;
        movie.Stop();
        movieAudioSource.Stop();
        TVScreen.gameObject.SetActive(false);

    }

    public void toggleWebcam()
    {
        webcamEnabled = !webcamEnabled;
        if (!webcamEnabled)
        {
            WebcamScreen.gameObject.SetActive(false);


        }
        else
        {
            //disableMovie();

            WebcamScreen.texture = webcamTex;
            if (WebCamTexture.devices.Length > 0)
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
            
            toggleFire();

        }
        if(Input.GetKeyDown(KeyCode.T))
        {

            toggleTV();
        }
        if (Input.GetKeyDown(KeyCode.C))
        {
            toggleWebcam();
        }
        if(Input.GetKeyDown(KeyCode.M))
        {
            toggleSpeakerClips();
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            toggleRoomba();
        }

            //Sync speakers
            if (masterSpeaker.isPlaying)
        {
            foreach(AudioSource slave in slaveSpeakers)
            {
                slave.timeSamples = masterSpeaker.timeSamples;
            }
        }
    }

    public void toggleRoomba()
    {
        roombaEnabled = !roombaEnabled;

        if (!roombaEnabled)
        {
            roombaScript.agent.destination = roombaScript.initialPosition;
            roombaScript.navigationEnabled = false;
        }
        else
        {
            roombaScript.agent.destination = roombaScript.GenerateRandomPoint(roombaScript.navigationMesh);
            roombaScript.navigationEnabled = true;
            roombaScript.agent.Resume();
            if (!roombaScript.audio.isPlaying)
                roombaScript.audio.Play();
        }
    }

    public void toggleCoffee()
    {
        coffeeEnabled = !coffeeEnabled;

        if (coffeeEnabled)
        {
            StopCoroutine("fillCoffeeAfterTime");
            StartCoroutine(fillCoffeeAfterTime(coffeeDelayTime));
        }
        else
        {
            StopCoroutine("fillCoffeeAfterTime");
            if (coffeeScaleTween != null && coffeeScaleTween.IsPlaying())
            {
                coffeeScaleTween.Pause();
                coffeeMoveTween.Pause();
            }
        }

        foreach (ParticleSystem ps in coffeePS.GetComponentsInChildren<ParticleSystem>())
        {
            if (!coffeeEnabled)
                ps.Stop();
            else
                ps.Play();
        }
        AudioSource source = coffeePS.GetComponent<AudioSource>();
        if (source)
        {
            if (!coffeeEnabled)
            {
                source.Stop();
            }
            else
                source.Play();
        }

    }

    IEnumerator fillCoffeeAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        if (!coffeeFill.activeInHierarchy)
        {
            coffeeFill.SetActive(true);
        }
        if (coffeeScaleTween != null && !coffeeScaleTween.IsPlaying())
        {
            coffeeScaleTween.Play();
            coffeeMoveTween.Play();
        }
        else
        {
            coffeeScaleTween = coffeeFill.transform.DOScaleY(1.0f, coffeeFillTime);
            coffeeMoveTween = coffeeFill.transform.DOLocalMoveY(0f, coffeeFillTime);
        }
        
        
    }

    public void toggleFire()
    {
        fireEnabled = !fireEnabled;

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

    public void toggleSpeakerClips()
    {
        speakersEnabled = !speakersEnabled;

        if (speakersEnabled)
        {
            if (speakerClips[speakerClipIndex])
            {
                for (int i = 0; i < speakers.Length; i++)
                {
                    speakers[i].Stop();
                    speakers[i].clip = loadClips[i, speakerClipIndex];
                    speakers[i].Play();
                }
            }
            else
            {
                foreach (AudioSource source in speakers)
                {
                    source.Stop();
                }
            }
        }
        else
        {
            foreach (AudioSource source in speakers)
            {
                source.Stop();
            }
        }
    }

    public void previousSong()
    {
        speakerClipIndex--;

        if (speakerClipIndex < 0)
            speakerClipIndex = speakerClips.Length-1;

        if (speakerClips[speakerClipIndex])
        {
            for (int i = 0; i < speakers.Length; i++)
            {
                speakers[i].Stop();
                speakers[i].clip = loadClips[i, speakerClipIndex];
                speakers[i].Play();
            }
        }
        else
        {
            foreach (AudioSource source in speakers)
            {
                source.Stop();
            }
        }
    }

    public void nextSong()
    {
        speakerClipIndex++;

        if (speakerClipIndex >= speakerClips.Length)
            speakerClipIndex = 0;

        if (speakerClips[speakerClipIndex])
        {
            for (int i = 0; i < speakers.Length; i++)
            {
                speakers[i].Stop();
                speakers[i].clip = loadClips[i, speakerClipIndex];
                speakers[i].Play();
            }
        }
        else
        {
            foreach (AudioSource source in speakers)
            {
                source.Stop();
            }
        }
    }

    public IEnumerator disableInputForTime(float time)
    {
        TriggerHapticPulse(hapticPulseDuration, OVRInput.Controller.LTouch);
        TriggerHapticPulse(hapticPulseDuration, OVRInput.Controller.RTouch);
        canPressButton = false;
        yield return new WaitForSeconds(time);
        canPressButton = true;
        yield return null;
    }

    public void TriggerHapticPulse(float duration = 0.1f, OVRInput.Controller controller = OVRInput.Controller.RTouch)
    {
        if (OVRInput.GetActiveController() == OVRInput.Controller.Touch)
        {
            StopCoroutine("DoHapticPulse");
            StartCoroutine(DoHapticPulse(duration, controller));
        }
    }

    private IEnumerator DoHapticPulse(float duration, OVRInput.Controller controller)
    {
        OVRInput.SetControllerVibration(0.2f, 0.2f, controller);    //Should we allow setting strength
        float endTime = Time.time + (duration);
        do
        {
            yield return null;
        } while (Time.time < endTime);
        OVRInput.SetControllerVibration(0, 0, controller);
    }

    private void OnApplicationQuit()
    {
        OVRInput.SetControllerVibration(0, 0);
    }
}
