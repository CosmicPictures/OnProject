using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


[RequireComponent(typeof(Rigidbody))]
public class interactionObject : MonoBehaviour {

    public OvrAvatar avatarScript;

    [SerializeField]
    private bool leftHovering = false;
    [SerializeField]
    private bool rightHovering = false;
    private Material[] mats;
    private Material initialMaterial;
    private Renderer rend;
    public float triggerGrabDeadzone = 0.1f;
    public GameObject leftHand;
    public GameObject rightHand;
    public GameObject childOutline;
    private Transform initialTransform;
    private Vector3 initialOffset;
    private Quaternion initialRotation;
    private Rigidbody rigid;
   
    public float interpolateSpeed = 0.5f;

    private List<Vector3> positionHistory;
    private int maxListSize = 10;

    // Use this for initialization
    void Start () {
        initialMaterial = GetComponent<Renderer>().material;
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
        positionHistory = new List<Vector3>();
	}

    // Update is called once per frame
    void Update()
    {
        if ((OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) > triggerGrabDeadzone || OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) > triggerGrabDeadzone) && leftHovering && !avatarScript.leftGrabbingObject && !avatarScript.rightGrabbingObject)
        {
            //Grab with left
            avatarScript.leftGrabbingObject = true;
            avatarScript.currentObject = this.gameObject;
            initialTransform = transform;
            rigid.useGravity = false;
            //rigid.isKinematic = true;
            /*
            
            Vector3 pos = transform.position - leftHand.transform.position;
            Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles - leftHand.transform.rotation.eulerAngles);
            initialOffset= pos;
            initialRotation = rot;*/
            //transform.parent = leftHand.transform;
            transform.SetParent(leftHand.transform);
            
            leftHovering = false;
            childOutline.SetActive(false);
        }

        //Drop left hand
        else if ((OVRInput.Get(OVRInput.RawAxis1D.LIndexTrigger) <= triggerGrabDeadzone && OVRInput.Get(OVRInput.RawAxis1D.LHandTrigger) <= triggerGrabDeadzone) && avatarScript.leftGrabbingObject && avatarScript.currentObject == this.gameObject)
        {
            avatarScript.leftGrabbingObject = false;
            transform.SetParent(null);

            //rigid.isKinematic = false;
            rigid.useGravity = true;
            rigid.velocity = calculateExitVelocity();
            //rigid.velocity = avatarScript.leftHandRigid.velocity;
            positionHistory = new List<Vector3>();
        }

        if ((OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) > triggerGrabDeadzone || OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) > triggerGrabDeadzone) && rightHovering && !avatarScript.leftGrabbingObject && !avatarScript.rightGrabbingObject)
        {
            //Grab with right
            Debug.Log("Grab with right");
            avatarScript.rightGrabbingObject = true;
            avatarScript.currentObject = this.gameObject;   
            rigid.useGravity = false;
            //rigid.isKinematic = true;
            /*
            
            initialTransform = transform;
            Vector3 pos = transform.position - leftHand.transform.position;
            Quaternion rot = Quaternion.Euler(transform.rotation.eulerAngles - rightHand.transform.rotation.eulerAngles);
            initialOffset = pos;
            initialRotation = rot;
            */
            transform.SetParent(rightHand.transform);
            rightHovering = false;
            childOutline.SetActive(false);
        }

        //Drop right hand
        else if ((OVRInput.Get(OVRInput.RawAxis1D.RIndexTrigger) <= triggerGrabDeadzone && OVRInput.Get(OVRInput.RawAxis1D.RHandTrigger) <= triggerGrabDeadzone) && avatarScript.rightGrabbingObject && avatarScript.currentObject == this.gameObject)
        {
            avatarScript.rightGrabbingObject = false;
            transform.SetParent(null);
            
            rigid.useGravity = true;
            rigid.velocity = calculateExitVelocity();
            //rigid.isKinematic = false;
            //rigid.velocity = avatarScript.rightHandRigid.velocity;
            positionHistory = new List<Vector3>();
        }
        

        
    }

    private void FixedUpdate()
    {
        if (avatarScript.rightGrabbingObject && Mathf.Abs((transform.position - rightHand.transform.position).magnitude) > 0.05f && avatarScript.currentObject == this.gameObject)
        {

            transform.localPosition = transform.localPosition * interpolateSpeed;
            //transform.position = transform.position + (rightHand.transform.position - transform.position).normalized * interpolateSpeed;
        }
        else if (avatarScript.leftGrabbingObject && Mathf.Abs((transform.position - leftHand.transform.position).magnitude) > 0.05f && avatarScript.currentObject == this.gameObject)
        {
            transform.localPosition = transform.localPosition * interpolateSpeed;
            //transform.position = transform.position + (leftHand.transform.position - transform.position).normalized * interpolateSpeed;
        }

        if(avatarScript.currentObject == this.gameObject)
        {
            savePosition();
        }

    }

    private void savePosition()
    {
        positionHistory.Add(transform.position);
        if(positionHistory.Count > maxListSize)
        {
            positionHistory.RemoveAt(0);
        }
    }

    private Vector3 calculateExitVelocity()
    {
        if(positionHistory.Count > 1)
        {
            float totalMovement = 0f;
            for(int i = 0; i < positionHistory.Count - 1;i++)
            {
                totalMovement += (positionHistory[i + 1] - positionHistory[i]).magnitude;
            }
            Debug.Log((positionHistory[positionHistory.Count - 1] - positionHistory[positionHistory.Count - 2]) * totalMovement / positionHistory.Count);
            return (positionHistory[positionHistory.Count - 1] - positionHistory[positionHistory.Count - 2]) * totalMovement / positionHistory.Count;
        }
        else
        {
            return Vector3.zero;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.tag == "Hand")
        {
            if (other.name == "controller_left")
            {
                //Empty hand
                if (!avatarScript.leftGrabbingObject && !leftHovering)
                {
                    leftHovering = true;
                }
            }
            else if (other.name == "controller_right" && !rightHovering)
            {
                //Empty hand
                if (!avatarScript.rightGrabbingObject)
                {
                    Debug.Log("check collision");
                    rightHovering = true;
                }

            }
        }

        if(rightHovering || leftHovering)
        {
            childOutline.SetActive(true);
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        OnTriggerEnter(other);
    }
    
    private void OnTriggerExit(Collider other)
    {
        if (other.tag == "Hand")
        {
            if (other.name == "controller_left")
            {

                leftHovering = false;
            }
            else if (other.name == "controller_right")
            {
                Debug.Log("exit collision");
                rightHovering = false;
            }
        }

        if(!rightHovering && !leftHovering)
        {
            childOutline.gameObject.SetActive(false);
        }
    }
    /*
    private void OnCollisionEnter(Collision collision)
    {
        OnCollisionStay(collision);
    }

    private void OnCollisionStay(Collision collision)
    {
        if(collision.collider.gameObject.tag == "Hand")
        {

        }
    }
    */
}
