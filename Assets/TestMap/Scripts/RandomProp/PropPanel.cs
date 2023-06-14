using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rd = GameProject.Random;

public class PropPanel : UIBase, IGameCompleted
{
    [SerializeField]
    private Text text;
    [SerializeField]
    public override UILayer Layer => UILayer.Panel;

    public bool Completed { get; private set; } = false;

    private void Start()
    {
        text.text = "Getting";
        StartCoroutine(GetProp());
    }
    IEnumerator GetProp()
    {
        yield return new WaitForSeconds(1);
        int prop = Rd.GetValue(1, 6);
        text.text = $"{prop}";
        yield return new WaitForSeconds(1);
        if (prop == 1)
        {
            //Map.Instance.CurUser.MoveToGrid()
        }
        CloseSelf(true);
        Completed = true;
    }
}
