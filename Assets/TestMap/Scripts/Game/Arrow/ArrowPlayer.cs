using System;
using System.Collections.Generic;
using UnityEngine;

public class ArrowPlayer : MonoBehaviour
{
    [SerializeField] Camp camp;
    [SerializeField] ArrowView arrow;
    [SerializeField] Transform container;

    [SerializeField] KeyCode left;
    [SerializeField] KeyCode up;
    [SerializeField] KeyCode right;
    [SerializeField] KeyCode down;

    Queue<Direction> queue;
    public Action<Camp> onWrong;
    public Action<Camp> onCompleted;
    private readonly Dictionary<KeyCode, Direction> keys = new Dictionary<KeyCode, Direction>();

    public bool Active { get; set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        keys.Add(left, Direction.Left);
        keys.Add(up, Direction.Up);
        keys.Add(right, Direction.Right);
        keys.Add(down, Direction.Down);
    }

    // Update is called once per frame
    void Update()
    {
        if (Active)
        {
            foreach (var k in keys)
            {
                if (Input.GetKeyDown(k.Key))
                {
                    InputDirection(k.Value);
                }
            }
        }   
    }

    public void SetQueue(List<Direction> list)
    {
        queue = new Queue<Direction>(list.Count);
        foreach (var l in list)
        {
            queue.Enqueue(l);
        }
    }

    void InputDirection(Direction dir)
    {
        var a = Instantiate(arrow, container);
        var d = queue.Dequeue();
        a.SetDirection(dir);
        if (d == dir)
        {
            a.ShowCorrect(true);
            if (queue.Count == 0)
            {
                onCompleted?.Invoke(camp);
            }
        }
        else
        {
            a.ShowCorrect(false);
            onWrong?.Invoke(camp);
        }
        a.gameObject.SetActive(true);
    }
}
