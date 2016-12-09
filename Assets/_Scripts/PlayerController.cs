using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject firePS;
    public MovieTexture movie;
    public Renderer TVScreen;
    public bool fireEnabled = true;
    public bool movieEnabled = true;

	// Use this for initialization
	void Start () {

        toggleFire();
        toggleTV();
        
	}

    private void toggleTV()
    {
        if(!movieEnabled)
        {
            movie = (MovieTexture)TVScreen.material.mainTexture;
            movie.Pause();
        }
        else
        {
            movie = (MovieTexture)TVScreen.material.mainTexture;
            movie.Play();
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
