using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cam;
    Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main.transform;
        targets = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            targets[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        foreach (Transform t in targets)
        {
            t.rotation = cam.rotation;
        }
    }
}
