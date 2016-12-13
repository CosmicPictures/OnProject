using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VRStandardAssets.Utils;

public class lightsOnLook : MonoBehaviour {

    [SerializeField]
    private VRInteractiveItem m_InteractiveItem;

    public Light[] lights;

    private bool lightsOn = false;

    private void Awake()
    {
        foreach(Light l in lights)
        {
            l.gameObject.SetActive(false);
        }
    }


    private void OnEnable()
    {
        m_InteractiveItem.OnOver += HandleOver;
        //m_InteractiveItem.OnOut += HandleOut;
        //m_InteractiveItem.OnClick += HandleClick;
        //m_InteractiveItem.OnDoubleClick += HandleDoubleClick;
    }


    private void OnDisable()
    {
        m_InteractiveItem.OnOver -= HandleOver;
        //m_InteractiveItem.OnOut -= HandleOut;
        //m_InteractiveItem.OnClick -= HandleClick;
        //m_InteractiveItem.OnDoubleClick -= HandleDoubleClick;
    }


    //Handle the Over event
    private void HandleOver()
    {
        if(!lightsOn)
        {
            lightsOn = true;
            foreach(Light l in lights)
            {
                l.gameObject.SetActive(true);
            }
        }
        //Debug.Log("Show over state");
        //m_Renderer.material = m_OverMaterial;
    }


    //Handle the Out event
    /*
    private void HandleOut()
    {
        Debug.Log("Show out state");
        m_Renderer.material = m_NormalMaterial;
    }


    //Handle the Click event
    private void HandleClick()
    {
        Debug.Log("Show click state");
        m_Renderer.material = m_ClickedMaterial;
    }


    //Handle the DoubleClick event
    private void HandleDoubleClick()
    {
        Debug.Log("Show double click");
        m_Renderer.material = m_DoubleClickedMaterial;
    }
    */
}
