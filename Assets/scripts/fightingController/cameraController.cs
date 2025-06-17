using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraController : MonoBehaviour
{
    public Transform[] targets;
    public float smoothspeed = 0.125f;
    public Vector3 offset;

    void LateUpdate() 
    {
        if (targets == null || targets.Length == 0) 
        {
            return;
        }

        Transform activeTarget = FindActivecharacter();
        if (activeTarget == null)
            return;

        Vector3 desiredposition = activeTarget.position + offset;
        desiredposition.y = transform.position.y;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredposition, smoothspeed);
        transform.position = smoothedPosition;
    }

    Transform FindActivecharacter()
    {
        foreach (Transform target in targets)
        {
            if (target.gameObject.activeInHierarchy)
                return target;
        }
        return null;
    }
}
