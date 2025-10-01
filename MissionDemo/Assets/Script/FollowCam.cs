using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI;
    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;

    [Header("Dynamic")]
    public float camZ;

    void Awake()
    {
        camZ = this.transform.position.z;

    }

     void FixedUpdate()
    {
        if(POI == null) return;

        Vector3 desination = POI.transform.position;

        Vector3 destination = Vector3.zero;

        if(POI != null)
        {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if ((poiRigid != null) && poiRigid.IsSleeping())  
            {
                POI = null;
            }
        }

        if (POI != null) { 
            destination = POI.transform.position;
        }

        //Get pos of poi

        //Vector3 destination = POI.transform.position;
        //Limit the minium values fo des x and y
        destination.x = Mathf.Max(minXY.x, destination.x);
        destination.y = Mathf.Max(minXY.y, destination.y);

        //Interpolate from the current Camera pos toward dest
        //destination = Vector3.Lerp(transform.position, destination, easing);
        //Force destination to keep the cam far away
        destination.z = camZ;

        transform.position = destination;

        //Set the orthographicSize of the Camera to keep Ground in view
        Camera.main.orthographicSize = destination.y + 10;
    }


}
