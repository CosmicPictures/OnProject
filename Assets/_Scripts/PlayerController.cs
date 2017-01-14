using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using System.IO;

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
    public bool lampEnabled = false;
    public GameObject lampOn;
    public GameObject lampOff;
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
    public TeleportController tpController;
    public GameObject monitorCanvas;
    private bool webcamSet = false;
    public bool enableWebcam = true;
    private bool loadingMain = false;
    public string webcamName = "Logitech HD Webcam C615";
    public bool enableUpstairsTeleports = false;
    public GameObject upstairsTeleports;

    private int screenshotNum = 0;

    public float maxTimeOutOfHMDBeforeLoad = 20f;
    private float currentTimeOutOfHMD = 0f;
    private static string persistentDataPath = null;

    // Use this for initialization
    void Start () {
        coffeeInitialY = coffeeFill.transform.localPosition.y;
        Application.targetFrameRate = targetFPS;

        roombaScript = roomba.GetComponent<roomNavigation>();

        if(upstairsTeleports && enableUpstairsTeleports)
        {
            upstairsTeleports.SetActive(true);
        }

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
        if (enableWebcam)
        {
            List<string> webcamNames = new List<string>();
            foreach(WebCamDevice w in WebCamTexture.devices)
            {
#if UNITY_EDITOR
                Debug.Log(w.name);
#endif
                webcamNames.Add(w.name);
            }

            if(webcamNames.Contains(webcamName))
            {
                webcamTex = new WebCamTexture(webcamName);
            }
            else
            {
#if UNITY_EDITOR
                Debug.Log("Could not find webcam " + webcamName + ", finding any webcam.");
#endif
                webcamTex = new WebCamTexture();
            }
                

            webcamTex.requestedWidth = (int)(WebcamScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.x / WebcamScreen.transform.parent.GetComponent<RectTransform>().localScale.x);
            webcamTex.requestedHeight = (int)(WebcamScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.y / WebcamScreen.transform.parent.GetComponent<RectTransform>().localScale.y);

            securityTex = webcamTex;
            //securityTex.requestedWidth = (int)securityScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.x;
            //securityTex.requestedHeight = (int)securityScreen.transform.parent.GetComponent<RectTransform>().sizeDelta.y;
            securityScreen.texture = securityTex;

            if (WebCamTexture.devices.Length > 0)
            {
                webcamTex.Play();
                securityTex.Play();
            }
            else
                Debug.Log("No webcams found!");

            webcamEnabled = !webcamEnabled;
            toggleWebcam();
        }
        else
        {
            WebcamScreen.gameObject.SetActive(false);
            securityScreen.gameObject.SetActive(false);
        }


        //fireEnabled = !fireEnabled;
        //toggleFire();
        //turnOffFire();

        movieEnabled = !movieEnabled;
        toggleTV();



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

        if (persistentDataPath == null)
            persistentDataPath = Application.persistentDataPath;
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

    private void FixedUpdate()
    {
        if (OVRPlugin.userPresent && monitorCanvas.activeInHierarchy)
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            monitorCanvas.SetActive(false);
        }

        if(!OVRPlugin.userPresent && !monitorCanvas.activeInHierarchy)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            monitorCanvas.SetActive(true);
        }

        if (OVRPlugin.userPresent && currentTimeOutOfHMD > 0)
        {
            currentTimeOutOfHMD = 0;
        }
        else if (!OVRPlugin.userPresent)
        {
            currentTimeOutOfHMD += Time.deltaTime;
            /*
#if UNITY_EDITOR
            Debug.Log(currentTimeOutOfHMD);
#endif
*/
            if (currentTimeOutOfHMD > maxTimeOutOfHMDBeforeLoad)
            {
                loadMain();
            }
        }

        //Double check for heater left on
        if (!fireEnabled && tpController.arduinoController.heaterOn)
        {
            tpController.arduinoController.turnHeaterOff();
        }

    }

    public void loadMain()
    {
        if (!loadingMain)
        {
            loadingMain = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            tpController.arduinoController.resetBoth();
            webcamTex.Stop();
            webcamTex = null;
            securityTex.Stop();
            securityTex = null;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        }
    }

    // Update is called once per frame
    void Update () {



        if(Input.GetKeyDown(KeyCode.Escape))
        {
            tpController.arduinoController.resetBoth();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
            loadMain();
#endif
            
        }
        /*
        if (Input.GetKeyDown(KeyCode.F))
        {

            toggleFire();

        }
        
        if (Input.GetKeyDown(KeyCode.T))
        {

            toggleTV();
        }



        
        if (Input.GetKeyDown(KeyCode.C))
        {
            toggleWebcam();
        }
        
        if (Input.GetKeyDown(KeyCode.C))
        {
            toggleCoffee();
        }
        if (Input.GetKeyDown(KeyCode.M))
        {
            toggleSpeakerClips();
        }
        if (Input.GetKeyDown(KeyCode.B))
        {
            toggleRoomba();
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            toggleLamp();
        }
        */
        if (Input.GetKeyDown(KeyCode.R))
        {
            loadMain();

        }


        
        /*
        if (Input.GetKeyDown(KeyCode.P) || (OVRInput.GetDown(OVRInput.Button.Start)))
        {
            while(screenshotNum < 1000)
            {
                if (File.Exists("Screenshot_" + screenshotNum.ToString() + ".png"))
                    screenshotNum++;
                else
                    break;  
            }
            Debug.Log("Data Path =  " + persistentDataPath);
            Application.CaptureScreenshot("Screenshot_" + screenshotNum.ToString() + ".png");
            screenshotNum++;
        }
        */
        //Sync speakers
        if (masterSpeaker.isPlaying)
        {
            foreach(AudioSource slave in slaveSpeakers)
            {
                slave.timeSamples = masterSpeaker.timeSamples;
            }
        }

        //if (!webcamSet)
         //   checkWebcamSet();
    }

    public void checkWebcamSet()
    {
        if (webcamTex.width >= (int)WebcamScreen.transform.GetComponent<RectTransform>().sizeDelta.x)
        {

        }
            

        webcamTex.requestedWidth = (int)WebcamScreen.transform.GetComponent<RectTransform>().sizeDelta.x;
        webcamSet = true;
        Debug.Log("Webcam resolution set");
    }

    public void toggleRoomba()
    {
        roombaEnabled = !roombaEnabled;

        if (!roombaEnabled)
        {
            if (roombaScript.agent.isOnNavMesh)
                roombaScript.agent.destination = roombaScript.initialPosition;
            roombaScript.navigationEnabled = false;
        }
        else
        {
            
            roombaScript.navigationEnabled = true;
            if (roombaScript.agent.isOnNavMesh)
            {
                roombaScript.agent.destination = roombaScript.GenerateRandomPoint(roombaScript.navigationMesh);
                roombaScript.agent.Resume();
            }
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
        if(fireEnabled == false && firePS.activeInHierarchy == false)
        {
            fireEnabled = true;
            firePS.SetActive(true);
        }
        else
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
            if (source)
            {
                if (!fireEnabled)
                {
                    source.Stop();
                }
                else
                    source.Play();
            }
        }

        if (fireEnabled && tpController.heaterList.Contains(tpController.currentTeleportPoint) && !tpController.arduinoController.heaterOn)
        {
            tpController.arduinoController.turnHeaterOn();
        }

        if (!fireEnabled && tpController.arduinoController.heaterOn)
        {
            tpController.arduinoController.turnHeaterOff();
        }


    }

    public void turnOffFire()
    {

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

        if (!fireEnabled && tpController.arduinoController.heaterOn)
        {
            tpController.arduinoController.turnHeaterOff();
        }
    }

    public void toggleLamp()
    {
        lampEnabled = !lampEnabled;

        if(lampEnabled)
        {
            lampOn.SetActive(true);
            lampOff.SetActive(false);
        }
        else
        {
            lampOn.SetActive(false);
            lampOff.SetActive(true);
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

    public IEnumerator disableInputForTime(float time, bool leftHaptic = false, bool rightHaptic = false)
    {
        if(leftHaptic)
            TriggerHapticPulse(hapticPulseDuration, OVRInput.Controller.LTouch);
        if(rightHaptic)
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
