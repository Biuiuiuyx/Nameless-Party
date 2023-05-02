using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class LookAtCamera : MonoBehaviour
{
    Transform cam;
    Transform[] targets;

    // Start is called before the first frame update
    void Start()
    {
        GameObject mainCameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        if (mainCameraObject != null)
        {
            cam = mainCameraObject.transform;
        }
        else
        {
            UnityEngine.Debug.LogError("找不到带有 'MainCamera' 标签的相机。");
            return;
        }

        targets = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            targets[i] = transform.GetChild(i);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (cam == null) return;

        foreach (Transform t in targets)
        {
            t.rotation = cam.rotation;
        }
    }
}

