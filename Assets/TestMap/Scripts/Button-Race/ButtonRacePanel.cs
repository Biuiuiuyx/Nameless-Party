using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 按键比赛界面
/// </summary>
public class ButtonRacePanel : UIBase
{
    [SerializeField] Text p1key;
    [SerializeField] Text p2key;
    [SerializeField] ButtonRacePlayer p1;
    [SerializeField] ButtonRacePlayer p2;
    [SerializeField] Text timeLabel;

    [SerializeField] Text p1Count;
    [SerializeField] Text p2Count;
    [SerializeField] Text p1result;
    [SerializeField] Text p2result;

    [SerializeField] Text tip;
    [SerializeField] GameObject mask;

    private float during = 12;
    private float curTime;

    public override UILayer Layer => UILayer.Panel;
    public bool Completed { get; private set; } = false;
    public bool Active { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        //p1key.text = p1.Key.ToString();
        //p2key.text = p2.Key.ToString();
        p1.onCount += ShowP1Result;
        p2.onCount += ShowP2Result;
        ToReset();
        StartCoroutine(DelayStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        curTime += Time.deltaTime;
        if (curTime >= during)
        {
            curTime = during;
            Active = false;
            p1.Active = false;
            p2.Active = false;
            StartCoroutine(DelayResult());
        }
        ShowTime();
        
    }

    void ToReset()
    {
        p1result.text = "";
        p2result.text = "";
        curTime = 0;
        ShowTime();
        p1.ToReset();
        p2.ToReset();
    }

    void ShowTime()
    {
        timeLabel.text = GameManager.FormatTime(during - curTime);
    }

    void ShowP1Result(int count)
    {
        p1Count.text = $"{count}";
    }

    void ShowP2Result(int count)
    {
        p2Count.text = $"{count}";
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
        p1.Active = true;
        p2.Active = true;
        Active = true;
    }

    IEnumerator DelayResult()
    {
        yield return new WaitForSeconds(.6f);
        ResultType result = ResultType.Tie;
        if (p1.Count > p2.Count)
        {
            p1result.text = ResultType.Win.ToString();
            p2result.text = ResultType.Lose.ToString();
            Map.Instance.GetUser(Camp.Player2).StopRound++;
            result = ResultType.Win;
        }else if (p1.Count < p2.Count)
        {
            p1result.text = ResultType.Lose.ToString();
            p2result.text = ResultType.Win.ToString();
            Map.Instance.GetUser(Camp.Player1).StopRound++;
            result = ResultType.Lose;
        }
        else
        {
            p1result.text = ResultType.Tie.ToString();
            p2result.text = ResultType.Tie.ToString();
        }
        yield return new WaitForSeconds(2f);
        if (result == ResultType.Win)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player2.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }
        else if (result == ResultType.Lose)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player1.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }
        else
        {
            Completed = true;
            CloseSelf(true);
        }
    }
}
