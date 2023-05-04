using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 玩家
/// </summary>
public class RockPaperScissorsPlayer : MonoBehaviour
{
    [SerializeField] KeyCode keyRock;
    [SerializeField] KeyCode keyPaper;
    [SerializeField] KeyCode keyScissors;

    public Action<RockPaperScissorsType> onKeyDown;

    private readonly Dictionary<RockPaperScissorsType, KeyCode> keys = new Dictionary<RockPaperScissorsType, KeyCode>();
    private readonly Dictionary<KeyCode, RockPaperScissorsType> types = new Dictionary<KeyCode, RockPaperScissorsType>();

    public bool Active { get; set; } = false;

    private void Awake()
    {
        keys.Add(RockPaperScissorsType.Rock, keyRock);
        keys.Add(RockPaperScissorsType.Paper, keyPaper);
        keys.Add(RockPaperScissorsType.Scissors, keyScissors);

        types.Add(keyRock, RockPaperScissorsType.Rock);
        types.Add(keyPaper, RockPaperScissorsType.Paper);
        types.Add(keyScissors, RockPaperScissorsType.Scissors);
    }

    // Update is called once per frame
    void Update()
    {
        if (Active) return;
        foreach (var k in types)
        {
            if (Input.GetKeyDown(k.Key))
            {
                onKeyDown?.Invoke(GetRockPaperScissors(k.Key));
            }
        }
    }

    public KeyCode GetKey(RockPaperScissorsType type)
    {
        keys.TryGetValue(type, out var k);
        return k;
    }

    RockPaperScissorsType GetRockPaperScissors(KeyCode key)
    {
        types.TryGetValue(key, out var type);
        return type;
    }
}
