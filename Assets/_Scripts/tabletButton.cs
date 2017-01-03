using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System;
using UnityEngine.SceneManagement;

public class tabletButton : MonoBehaviour
{

    PlayerController pc;
    public enum Action { Previous, Next, Load };
    private Button button;

    public Action buttonAction;

    private bool isLoading = false;
    private string lastLoadProgress = null;
    public Text loadingText;
    private string loadProgress = "Loading...";
    private AudioSource source;

    // Use this for initialization
    void Start()
    {
        source = GetComponent<AudioSource>();
        pc = GameObject.Find("OVRCameraRig").GetComponent<PlayerController>();
        button = GetComponent<Button>();
        //rect = rect.GetComponent<RectTransform>();

        if (!pc)
            Debug.Log("Could not find player controller!");


        if (!button)
            Debug.Log("No button found!");

    }

    // Update is called once per frame
    void Update()
    {
        /*
        switch (buttonAction)
        {
            case Action.Previous:
                if ((pc.webcamEnabled && !toggle.isOn) || (!pc.webcamEnabled && toggle.isOn))
                    simulateClick();
                break;
            case Action.Next:
                if ((pc.movieEnabled && !toggle.isOn) || (!pc.movieEnabled && toggle.isOn))
                    simulateClick();
                break;
            default:
                break;
        }
        */
    }

    /*
    private void OnEnable()
    {
        rect = GetComponent<RectTransform>();
        //Debug.Log(GetComponent<RectTransform>().anchoredPosition);
        rect.DOMove(rect.anchoredPosition3D + offset, tweenDuration);
        //transform.DOMove(transform.position + offset,tweenDuration);
    }
    */

    private void OnTriggerEnter(Collider other)
    {
        if (pc.canPressButton)
        {
            switch (buttonAction)
            {
                case Action.Previous:
                    pc.previousSong();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Action.Next:
                    pc.nextSong();
                    simulateClick();
                    StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                case Action.Load:
                    StartCoroutine(loadAsync());
                    simulateClick();
                    //StartCoroutine(pc.disableInputForTime(pc.buttonCooldown, true, true));
                    break;
                default:
                    break;
            }
        }
#if UNITY_EDITOR
        Debug.Log("pressed " + buttonAction.ToString());
#endif  

    }

    private void simulateClick()
    {
        PointerEventData pointer = new PointerEventData(EventSystem.current);
        ExecuteEvents.Execute(button.gameObject, pointer, ExecuteEvents.submitHandler);
        if (source)
        {
            source.Play();
        }
    }

    IEnumerator loadAsync()
    {
        if (!isLoading)
        {
            isLoading = true;
            GetComponent<Collider>().enabled = false;
            AsyncOperation _async = new AsyncOperation();
            _async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            loadingText.text = "Loading...";
            _async.allowSceneActivation = false;

            while (_async.progress < 0.9f)
            {
                yield return null;
            }

            _async.allowSceneActivation = true;

            while (!_async.isDone)
            {
                yield return null;
            }
            /*
            Scene nextScene = SceneManager.GetSceneByName(name);
            if (nextScene.IsValid())
            {
                SceneManager.SetActiveScene(nextScene);
                SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene().name);
            }
            */
        }
        /*
        if (!isLoading)
        {
            isLoading = true;
            GetComponent<Collider>().enabled = false;
            AsyncOperation a = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            a.allowSceneActivation = false;
            loadingText.text = "Loading...";
            while (!a.isDone)
            {
                if (a.progress < 0.9f)
                {
                    Debug.Log(a.progress);
                    yield return null;
                }
                else // if progress >= 0.9f the scene is loaded and is ready to activate.
                {

                    a.allowSceneActivation = true;

                }
            }
            yield return null;
        }
        yield return null;
        */
        /*
        if (!isLoading)
        {
            isLoading = true;
            GetComponent<Collider>().enabled = false;
            yield return new WaitForSeconds(1f);
            SceneManager.LoadScene(1);

        }
        */
    }
}
