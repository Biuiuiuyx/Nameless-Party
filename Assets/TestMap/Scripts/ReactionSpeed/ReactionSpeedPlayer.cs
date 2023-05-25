using System;
using UnityEngine;

public class ReactionSpeedPlayer : MonoBehaviour
{
    [SerializeField] KeyCode key;

    public Action onSubmit;

    public KeyCode Key => key;
    public bool Active { get; set; } = false;

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        if (Input.GetKeyDown(key))
        {
            PressDown();
        }
    }

    void PressDown()
    {
        if (!Active) return;
        onSubmit?.Invoke();
    }
}
