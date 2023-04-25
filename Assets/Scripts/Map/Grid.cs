using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 格子
/// </summary>
public class Grid : MonoBehaviour
{
    #region ---- 配置数据 ----
    [SerializeField] int gid;               // 格子id 主键
    [SerializeField] GridType type;         // 格子类型
    [SerializeField] int nextId;            // 下一个格子id
    [SerializeField] int value = 1;         // 用于奖励和惩罚的值

    public int GridId => gid;
    public GridType Type => type;
    public int NextId => nextId;
    public int Value => value;
    #endregion

    private void Awake()
    {
       
    }

#if UNITY_EDITOR
    private GridView v;
    private GridView View
    {
        get
        {
            if (v == null)
            {
                v = GetComponent<GridView>();
            }
            return v;
        }
    }

    private void OnValidate()
    {
        //Debug.Log($"onValidate - {gameObject.name}");
        View.Init();
    }
#endif
}
