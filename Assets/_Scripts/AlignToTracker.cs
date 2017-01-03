using UnityEngine;
using System.Collections;

public class AlignToTracker : MonoBehaviour
{
    public OVRPose trackerPose = OVRPose.identity;

    private Quaternion initialRotation;
    private OVRCameraRig rig;

    void Awake()
    {
        rig = GameObject.FindObjectOfType<OVRCameraRig>();

        if (rig != null)
            rig.UpdatedAnchors += OnUpdatedAnchors;

    }


    void OnUpdatedAnchors(OVRCameraRig rig)
    {
        if (!enabled)
            return;
        
        OVRPose pose = rig.trackerAnchor.ToOVRPose(true).Inverse();
        trackerPose.orientation = Quaternion.LookRotation(Vector3.left, Vector3.up);
        pose = pose * trackerPose;
        rig.trackingSpace.FromOVRPose(pose, true);
    }
}
