using UnityEngine;
using System.Collections;

public class CameraCollision : MonoBehaviour
{
    Transform playerTransform;
    Quaternion targetLook;
    Vector3 targetMove;
    public float rayHitMoveInFront = 0.1f;
    Vector3 targetMoveUse;
    public float smoothLook = 0.5f;
    public float smoothMove = 0.5f;
    Vector3 smoothMoveV;
    public float distFromPlayer = 5.0f;
    public float heightFromPlayer = 2.0f;

    // Use this for initialization
    void Start()
    {
        playerTransform = GameObject.FindWithTag("Player").transform;


    }

    // Update is called once per frame
    void LateUpdate()
    {
        targetMove = playerTransform.position + (playerTransform.rotation * new Vector3(0, heightFromPlayer, -distFromPlayer));

        RaycastHit hit;
        if (Physics.Raycast(playerTransform.position, targetMove - playerTransform.position, out hit, Vector3.Distance(playerTransform.position, targetMove)))
        {
            targetMoveUse = Vector3.Lerp(hit.point, playerTransform.position, rayHitMoveInFront);
        }
        else
        {
            targetMoveUse = targetMove;
        }

        transform.position = Vector3.SmoothDamp(transform.position, targetMoveUse, ref smoothMoveV, smoothMove);

    }
}


/*
 *     string TargetTag  = "Player";
private GameObject target  ;
    float distance = 5.0f;
    float xSpeed = 250.0f;
    float ySpeed = 120.0f;
    float yMinLimit = -20;
    float yMaxLimit = 80;
    float distanceMin = 1;
    float distanceMax = 5;
private float x = 0.0f;
    private float y = 0.0f;
    private float zoom ;

    void Start()
    {
        zoom = distanceMax;
      //  yield return new WaitForSeconds(0.1f);

        target = GameObject.FindWithTag(TargetTag);
        var angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;
        if (GetComponent<Rigidbody>()) GetComponent<Rigidbody>().freezeRotation = true;
    }

    void Update()
    {
        if (target)
        {
            x += Input.GetAxis("Mouse X") * xSpeed * 0.02f;
            y -= Input.GetAxis("Mouse Y") * ySpeed * 0.02f;
            y = ClampAngle(y, yMinLimit, yMaxLimit);
            var rotation = Quaternion.Euler(y, x, 0);
            zoom = Mathf.Clamp(zoom - Input.GetAxis("Mouse ScrollWheel") * 5, distanceMin, distanceMax);
            var SuggestedPos = rotation * new Vector3(0.0f, 0.0f, -zoom) + target.transform.position;
            RaycastHit hit ;
            if (Physics.Linecast(target.transform.position, SuggestedPos, out hit)) distance = Mathf.Lerp(distance, hit.distance - .5f, .25f);
            else distance = Mathf.Lerp(distance, zoom, .1f); var position = rotation * new Vector3(0.0f, 0.0f, -distance - .5f) + target.transform.position;
            transform.rotation = Quaternion.Slerp(transform.rotation, rotation, 10);
            transform.position = Vector3.Lerp(transform.position, position, 10);
        }
    }


    static float ClampAngle(float angle , float min, float max)
    {
        if (angle < -360)
            angle += 360;
        if (angle > 360)
            angle -= 360;
        return Mathf.Clamp(angle, min, max);
    }
 */
