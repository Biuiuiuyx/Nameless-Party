using System;
using System.Collections.Generic;
using UnityEngine;

public class PokerPlayer : MonoBehaviour
{
    [SerializeField] KeyCode Akey;
    [SerializeField] KeyCode Bkey;
    [SerializeField] KeyCode Ckey;
    [SerializeField] KeyCode Dkey;

    public Action<int> onSelected;

    public bool Active { get; set; } = false;
    private readonly Dictionary<KeyCode, int> keys = new Dictionary<KeyCode, int>();

    // Start is called before the first frame update
    void Start()
    {
        keys.Add(Akey, 0);
        keys.Add(Bkey, 1);
        keys.Add(Ckey, 2);
        keys.Add(Dkey, 3);
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        foreach (var k in keys)
        {
            if (Input.GetKeyDown(k.Key))
            {
                onSelected?.Invoke(k.Value);
            }
        }
    }
}
