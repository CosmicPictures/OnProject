using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class faucetOnHover : MonoBehaviour
{

    private EllipsoidParticleEmitter[] emitters;
    private AudioSource source;
    private float initialVolume = 0f;
    private float fadeTime = 1f;

    // Use this for initialization
    void Start()
    {
        emitters = GetComponentsInChildren<EllipsoidParticleEmitter>();
        source = GetComponent<AudioSource>();
        if (source)
            initialVolume = source.volume;
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(EllipsoidParticleEmitter e in emitters)
        {
            if (e.emit == false)
            {
                e.emit = true;
            }
            if(!source.isPlaying)
            {
                source.Play();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        foreach (EllipsoidParticleEmitter e in emitters)
        {
            if (e.emit == true)
                e.emit = false;

            if (source.isPlaying)
                source.Pause();
        }
    }
    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }

    public IEnumerator FadeOut(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        //audioSource.volume = initialVolume;
    }
    public IEnumerator FadeIn(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume < initialVolume)
        {
            audioSource.volume += initialVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.volume = initialVolume;
    }
}
