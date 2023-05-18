using System;
using UnityEngine;

/// <summary>
/// 按键比赛的玩家
/// </summary>
public class ButtonRacePlayer : MonoBehaviour
{
    [SerializeField] KeyCode key;

    private int count = 0;
    public Action<int> onCount;

    public int Count => count;
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
        count++;
        onCount?.Invoke(count);
    }

    public void ToReset()
    {
        count = 0;
        onCount?.Invoke(count);
    }
}
