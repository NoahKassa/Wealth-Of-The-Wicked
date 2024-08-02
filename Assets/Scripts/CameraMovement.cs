using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public float interpVelocity;
    public float minDistance;
    public float followDistance;
    public GameObject target;
    public Vector3 offset;
    Vector3 targetPos;
    public Moving Moving;

    private void Start()
    {
        Moving = GameObject.Find("saitama").GetComponent<Moving>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if (target && Moving.GameStart == true)
        {
            Vector3 posNoZ = transform.position;
            posNoZ.z = target.transform.position.z;

            Vector3 targetDirection = (target.transform.position - posNoZ);

            interpVelocity = targetDirection.magnitude * 10f;

            targetPos = transform.position + (targetDirection.normalized * interpVelocity * Time.deltaTime);

            transform.position = Vector3.Lerp(transform.position, targetPos + offset, 0.25f);

        }
    }
}
