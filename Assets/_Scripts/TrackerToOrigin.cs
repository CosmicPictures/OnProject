using UnityEngine;

public class TrackerToOrigin : MonoBehaviour
{
    public OVRPose origin = OVRPose.identity;
    public bool useWorldSpace = true;

    void Awake()
    {
        var rig = GameObject.FindObjectOfType<OVRCameraRig>();
        rig.UpdatedAnchors += OnUpdatedAnchors;
    }

    void OnUpdatedAnchors(OVRCameraRig rig)
    {
        var oldPose = rig.trackerAnchor.ToOVRPose(!useWorldSpace);

        var offset = origin * oldPose.Inverse();

        if (useWorldSpace)
            offset = rig.transform.ToOVRPose() * offset;

        rig.trackingSpace.FromOVRPose(offset, !useWorldSpace);
    }
}