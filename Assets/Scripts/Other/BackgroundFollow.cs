using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    [SerializeField] private Transform mTarget;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(mTarget.position.x, transform.position.y, 500f);
    }
}
