using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRCameraCtrl : MonoBehaviour
{
    public float speed = 0.1f;

    void Update()
    {
        Vector2 dir = OVRInput.Get(OVRInput.Axis2D.SecondaryThumbstick);
        transform.position += new Vector3(dir.x * speed, 0f, dir.y * speed);
        
        if(OVRInput.Get(OVRInput.Button.Two)){
            transform.position += new Vector3(0, speed, 0);
        }
        if(OVRInput.Get(OVRInput.Button.One)) {
            transform.position += new Vector3(0, -speed, 0);
        }
    }
}
