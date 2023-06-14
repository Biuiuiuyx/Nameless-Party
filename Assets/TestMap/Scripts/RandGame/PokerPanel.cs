using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rd = GameProject.Random;

public class PokerPanel : UIBase, IGameCompleted
{
    static string[] Num2Char = new string[] { "A", "2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K" };
    [SerializeField]
    int[] pokerVals = new int[] { 0, 0, 0, 0 };
    [SerializeField] PokerPlayer p1;
    [SerializeField] PokerPlayer p2;
    [SerializeField] Text title;
    [SerializeField] Text[] PokerNums;
    [SerializeField] Image[] PokerImgs;

    [SerializeField] Text p1selected;
    [SerializeField] Text p1result;

    [SerializeField] Text p2selected;
    [SerializeField] Text p2result;

    [SerializeField] Text tip;
    [SerializeField] GameObject mask;
    [SerializeField] Sprite frontSprite;
    [SerializeField] Sprite backSprite;

    public override UILayer Layer => UILayer.Panel;
    public bool Active { get; set; } = false;
    public bool Completed { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        p1.onSelected += ShowP1Result;
        p2.onSelected += ShowP2Result;
        ToReset();
        StartCoroutine(DelayStart());
    }
    void ToReset()
    {
        foreach (Text text in PokerNums)
            text.text = "";
        p1result.text = "";
        p2result.text = "";
        p1selected.text = "";
        p2selected.text = "";
        foreach (Image img in PokerImgs)
            img.sprite = backSprite;
    }

    int p1Num = -1;
    int p2Num = -1;
    void ShowP1Result(int idx)
    {
        if (Active)
        {
            if (idx == p2Num)
                return;
            p1Num = pokerVals[idx];
            p1.Active = false;
            p1selected.text = Num2Char[pokerVals[idx]];
            PokerImgs[idx].sprite = frontSprite;
            PokerNums[idx].text = Num2Char[pokerVals[idx]];
            if (!p2.Active)
                Active = false;
        }
    }

    void ShowP2Result(int idx)
    {
        if (Active)
        {
            if (idx == p1Num)
                return;
            p2Num = pokerVals[idx];
            p2.Active = false;
            p2selected.text = Num2Char[pokerVals[idx]];
            PokerImgs[idx].sprite = frontSprite;
            PokerNums[idx].text = Num2Char[pokerVals[idx]];
            if (!p1.Active)
                Active = false;
        }
    }
    void GetRandomArray(int min=0,int max=13)
    {
        HashSet<int> aSet = new HashSet<int>(pokerVals.Length);
        for(int i = 0; i < pokerVals.Length; ++i)
        {
            pokerVals[i] = Rd.GetValue(min, max);
            while (aSet.Contains(pokerVals[i]))
                pokerVals[i] = Rd.GetValue(min, max);
            aSet.Add(pokerVals[i]);
        }
    }
    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1.5f);
        mask.SetActive(true);
        tip.text = "Ready";
        tip.transform.DOScale(1, .9f).SetEase(Ease.OutElastic).From(2.5f);
        yield return new WaitForSeconds(1);
        tip.text = "Go";
        tip.transform.DOScale(1, .9f).SetEase(Ease.OutElastic).From(2.5f);
        yield return new WaitForSeconds(1);
        tip.text = "";
        mask.SetActive(false);

        GetRandomArray();

        yield return new WaitForSeconds(1);

        p1.Active = true;
        p2.Active = true;
        Active = true;
        yield return DelayResult();
    }

    IEnumerator DelayResult()
    {
        yield return new WaitUntil(() => !Active);
        Camp loser = (p1Num > p2Num ? Camp.Player2 : Camp.Player1);
        Camp winner = (p1Num < p2Num ? Camp.Player2 : Camp.Player1);
        p1result.text = (p1Num > p2Num ? ResultType.Win.ToString() : ResultType.Lose.ToString());
        p2result.text = (p1Num < p2Num ? ResultType.Win.ToString() : ResultType.Lose.ToString());
        yield return new WaitForSeconds(1.0f);
        Map.Instance.GetUser(loser).StopRound++;
        //Map.Instance.GetUser(loser).RetreatStep(1);
        //Map.Instance.GetUser(winner).MoveStep(1);
        DialogueManager.Instance.ShowDialogue(new DialogueData($"<{loser.Name()}> fails. Pause one turn"), () =>
        {
            Completed = true;
            CloseSelf(true);
        });
    }
}
