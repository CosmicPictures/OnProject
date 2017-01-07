using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class exitManager : MonoBehaviour {

    private bool isLoading = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
#if UNITY_STANDALONE
            Application.Quit();
#endif

        }
        if(Input.GetKeyDown(KeyCode.L))
        {
            StartCoroutine(loadAsync());
        }
    }
    IEnumerator loadAsync()
    {
        if (!isLoading)
        {
            isLoading = true;
            AsyncOperation _async = new AsyncOperation();
            _async = SceneManager.LoadSceneAsync(1, LoadSceneMode.Single);
            //loadingText.text = "Loading...";
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
        }
    }
}
