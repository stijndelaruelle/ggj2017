using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPosition : MonoBehaviour
{
    private void Awake()
    {
        float sign = Mathf.Sign(transform.position.x);

        float viewportX = 1.1f;
        if (sign < 0.0f) { viewportX = -0.1f; }

        //Place ourselves at the outsides of town
        Vector3 worldPosX = Camera.main.ViewportToWorldPoint(new Vector3(viewportX, 0.0f, 0.0f));
        transform.position = new Vector3(worldPosX.x, transform.position.y, transform.position.z);
    }
}
