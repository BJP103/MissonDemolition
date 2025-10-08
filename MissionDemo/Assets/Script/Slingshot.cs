using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [Header("Inscribed")]

    public GameObject projectPrefab;
    public float velocityMult = 10f;
    public GameObject projLinePrefab;

    [Header("Dynamic")]
    public GameObject lauchPoint;
    public Vector3 lauchPos;
    public GameObject projectile;
    public bool aimingMode;

     void Awake()
    {
        Transform lauchPointTrans = transform.Find("LauchPoint");
        lauchPoint = lauchPointTrans.gameObject;
        lauchPoint.SetActive(false);
        lauchPos = lauchPointTrans.position;
    }
     void OnMouseEnter()
    {
        //print("Slingshot.OnMouseEnter()");
        lauchPoint.SetActive(true);
    }

     void OnMouseExit()
    {
        //print("Sling.OnMouserExit()");
        lauchPoint.SetActive(false);
    }

     void OnMouseDown()
    {
        // The player pressed mouse button
        aimingMode = true;
        // Instantiate a projectile
        projectile = Instantiate(projectPrefab) as GameObject;
        // Start it at the Lauch point
        projectile.transform.position = lauchPos;
        // Set to is kinematic
        projectile.GetComponent<Rigidbody>().isKinematic = true;
    }

     void Update()
    {
        // if slingslhot is not in aiming mode
        if(!aimingMode)return;

        //Getting mouse pos
        Vector3 mousePos2D = Input.mousePosition;
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePose3D = Camera.main.ScreenToWorldPoint( mousePos2D );

        //Find delta from lauchpos
        Vector3 mouseDelta = mousePose3D -lauchPos;
        //Limit mouse to radius of the slingshot spherecollider
        float maxMagnitude = this.GetComponent<SphereCollider>().radius;
        if(mouseDelta.magnitude > maxMagnitude)
        {
            mouseDelta.Normalize();
            mouseDelta *= maxMagnitude;

        }
        //Move the projectile to the new pos
        Vector3 projPos = lauchPos + mouseDelta;
        projectile.transform.position = projPos;

        if (Input.GetMouseButtonUp(0))
        {
            //Mouse has been released
            aimingMode=false;
            Rigidbody projRB = projectile.GetComponent<Rigidbody>();
            projRB.isKinematic = false;
            projRB.collisionDetectionMode = CollisionDetectionMode.Continuous;
            projRB.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            //Add a Projectile Line to the Projectile
            Instantiate<GameObject>(projLinePrefab, projectile.transform);
            projectile = null;
            MissionDemolition.SHOT_FIRED();
        }
    }
}
