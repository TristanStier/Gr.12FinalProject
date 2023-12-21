using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private float mFollowSpeed = 8f;
    [SerializeField] private Transform mTarget;

    // Update is called once per frame
    void Update()
    {
        Vector3 newPos = new Vector3(mTarget.position.x, mTarget.position.y+2, -10f);
        transform.position = Vector3.Slerp(transform.position, newPos, mFollowSpeed*Time.deltaTime);
    }
}
