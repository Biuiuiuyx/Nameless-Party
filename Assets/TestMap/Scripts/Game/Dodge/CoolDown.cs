using System;
using UnityEngine;

public class CoolDown
{
    private float cd = 1;
    private float curTime = 0;
    public Action onCompleted;

    public bool Completed => curTime >= cd;

    public CoolDown() { }
    public CoolDown(float _cd) => SetCD(_cd);

    public void SetCD(float _cd) => cd = _cd;

    public void AddDeltaTime(float _deltaTime)
    {
        curTime = Mathf.Clamp(curTime + _deltaTime, 0, cd);
        if (Completed)
        {
            onCompleted?.Invoke();
            Reset();
        }
    }

    public void Reset() => curTime = 0;
}
