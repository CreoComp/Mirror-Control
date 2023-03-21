using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Transform player;
    public Vector3 offset;
    Vector3 distance;
    void Update()
    {
        
        float mw = Input.GetAxis("Mouse ScrollWheel");
        if (mw != 0)
        {
            if (mw > 0.1) mw = 0.1f;
            if (mw < -0.1) mw = -0.1f;
            Vector3 lastDistance = distance;
            distance += transform.forward * 13 * mw * 10 ;/*Приближение*/

            if (Vector3.Distance(transform.position, player.position) < 50f)
            {
                distance -= transform.forward * 13;
            }
            else if (Vector3.Distance(transform.position, player.position) > 150f)
            {
                distance += transform.forward * 13;
            }
        }

      //  Debug.Log(Vector3.Distance(transform.position, player.position));

        transform.position = player.position + offset + distance;
    }
}
