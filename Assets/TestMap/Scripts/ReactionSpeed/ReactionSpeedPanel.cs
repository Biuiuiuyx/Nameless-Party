using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ReactionSpeedPanel : UIBase, IGameCompleted
{
    [SerializeField] ReactionSpeedPlayer p1;
    [SerializeField] ReactionSpeedPlayer p2;
    [SerializeField] Text p1percent;
    [SerializeField] Text p1estimateLabel;
    [SerializeField] Text p1result;

    [SerializeField] Text p2percent;
    [SerializeField] Text p2estimateLabel;
    [SerializeField] Text p2result;

    [SerializeField] Image flag;
    [SerializeField] Text tip;
    [SerializeField] GameObject mask;

    float center;
    float p1Time;
    float p2Time;
    float curTime;
    float startTime;

    public const float During = 1f;

    public override UILayer Layer => UILayer.Panel;
    public bool Active { get; private set; } = false;
    public bool Completed { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        p1.onSubmit += ShowP1Result;
        p2.onSubmit += ShowP2Result;
        ToReset();
        StartCoroutine(DelayStart());
    }

    // Update is called once per frame
    void Update()
    {
        if (!Active) return;
        curTime += Time.deltaTime;
        if (curTime > center + .2f)
        {
            flag.gameObject.SetActive(false);
        }
        else
        {
            flag.transform.localScale = Vector3.one * Mathf.LerpUnclamped(4f, 1f, (curTime - startTime)/During);
        }
        if (curTime > center + During)
        {
            Active = false;
            flag.gameObject.SetActive(false);
            StartCoroutine(DelayResult());
        }
    }

    void ToReset()
    {
        p1percent.text = "";
        p1estimateLabel.text = "";
        p1result.text = "";

        p2percent.text = "";
        p2estimateLabel.text = "";
        p2result.text = "";
    }

    void ShowP1Result()
    {
        if (Active)
        {
            p1.Active = false;
            p1Time = Time.time;
            var e = GetEstimate(p1Time - center);
            p1estimateLabel.text = e.ToString();
            p1percent.text = $"{((1 - (Mathf.Abs(p1Time - center) / During)) * 100).ToString("F3")}%";
        }
    }

    Estimate GetEstimate(float offset)
    {
        var o = Mathf.Abs(offset);
        if (o <= Estimate.Perfect.GetRate())
        {
            return Estimate.Perfect;
        }else if (o < Estimate.Great.GetRate())
        {
            return Estimate.Great;
        }else if (o < Estimate.Good.GetRate())
        {
            return Estimate.Good;
        }else if (o < Estimate.Bad.GetRate())
        {
            return Estimate.Bad;
        }
        else
        {
            return Estimate.Miss;
        }
    }

    void ShowP2Result()
    {
        if (Active)
        {
            p2.Active = false;
            p2Time = Time.time;
            var e = GetEstimate(p2Time - center);
            p2estimateLabel.text = e.ToString();
            p2percent.text = $"{((1 - (Mathf.Abs(p2Time - center) /During))* 100).ToString("F3")}%";
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
        center = Time.time + During;
        curTime = Time.time;
        startTime = curTime;
        p1.Active = true;
        p2.Active = true;
        Active = true;
        flag.gameObject.SetActive(true);
    }

    IEnumerator DelayResult()
    {
        if (p1.Active && p2.Active)
        {
            p1.Active = false;
            p2.Active = false;
            yield return new WaitForSeconds(.6f);
            p1estimateLabel.text = Estimate.Miss.ToString();
            p1percent.text = "-.---%";
            p2estimateLabel.text = Estimate.Miss.ToString();
            p2percent.text = "-.---%";
            p1result.text = ResultType.Tie.ToString();
            p2result.text = ResultType.Tie.ToString();
            yield return new WaitForSeconds(2f);
            Completed = true;
            CloseSelf(true);
        }else if (p1.Active)
        {
            p1.Active = false;
            yield return new WaitForSeconds(.6f);
            p1estimateLabel.text = Estimate.Miss.ToString();
            p1percent.text = "-.---%";
            p1result.text = ResultType.Lose.ToString();
            p2result.text = ResultType.Win.ToString();
            Map.Instance.GetUser(Camp.Player1).StopRound++;
            yield return new WaitForSeconds(2f);
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player1.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }else if (p2.Active)
        {
            p2.Active = false;
            yield return new WaitForSeconds(.6f);
            p2estimateLabel.text = Estimate.Miss.ToString();
            p2percent.text = "-.---%";
            p2result.text = ResultType.Lose.ToString();
            p1result.text = ResultType.Win.ToString();
            Map.Instance.GetUser(Camp.Player2).StopRound++;
            yield return new WaitForSeconds(2f);
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player2.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }else
        {
            yield return new WaitForSeconds(.6f);
            var e1 = Mathf.Abs(p1Time - center);
            var e2 = Mathf.Abs(p2Time - center);
            if (e1 < e2)
            {
                p1result.text = ResultType.Win.ToString();
                p2result.text = ResultType.Lose.ToString();
                Map.Instance.GetUser(Camp.Player2).StopRound++;
                yield return new WaitForSeconds(2f);
                DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player2.Name()}> fails. Pause one turn"), () =>
                {
                    Completed = true;
                    CloseSelf(true);
                });
            }
            else if (e1 > e2)
            {
                p2result.text = ResultType.Win.ToString();
                p1result.text = ResultType.Lose.ToString();
                Map.Instance.GetUser(Camp.Player1).StopRound++;
                yield return new WaitForSeconds(2f);
                DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player1.Name()}> fails. Pause one turn"), () =>
                {
                    Completed = true;
                    CloseSelf(true);
                });
            }
            else
            {
                p1result.text = ResultType.Tie.ToString();
                p2result.text = ResultType.Tie.ToString();
                yield return new WaitForSeconds(2f);
                Completed = true;
                CloseSelf(true);
            }
        }
    }
}
