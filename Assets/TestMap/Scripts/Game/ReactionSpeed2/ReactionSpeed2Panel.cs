using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using Random = GameProject.Random;

public class ReactionSpeed2Panel : UIBase, IGameCompleted
{
    [SerializeField] ReactionSpeedPlayer p1;
    [SerializeField] ReactionSpeedPlayer p2;
    [SerializeField] Text p1result;
    [SerializeField] Text p2result;

    [SerializeField] Image flag;
    [SerializeField] Text tip;
    [SerializeField] GameObject mask;

    public const float During = 5f;
    public const float Min = 2f;
    public const float Max = 4f;
    float p1Time;
    float p2Time;
    float startTime;
    float curTime;
    float endTime;

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

    private void FixedUpdate()
    {
        if (!Active) return;
        curTime += Time.fixedDeltaTime;
        if (curTime <= startTime && curTime + Time.fixedDeltaTime >= startTime)
        {
            flag.color = new Color(0, 1, 0, .5f);
            p1.Active = true;
            p2.Active = true;
        }
        if (curTime >= endTime)
        {
            Active = false;
            StartCoroutine(DelayResult());
        }
    }

    void ToReset()
    {
        p1result.text = "";
        p2result.text = "";
    }

    void ShowP1Result()
    {
        if (Active)
        {
            p1.Active = false;
            p1Time = Time.time;
            if (p2.Active)
            {
                Active = false;
                StartCoroutine(DelayResult());
            }
        }
    }

    void ShowP2Result()
    {
        if (Active)
        {
            p2.Active = false;
            p2Time = Time.time;
            if (p1.Active)
            {
                Active = false;
                StartCoroutine(DelayResult());
            }
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
        startTime = Time.time + Random.GetValue(Min, Max);
        curTime = Time.time;
        endTime = Time.time + During;
        p1Time = endTime;
        p2Time = endTime;
        flag.gameObject.SetActive(true);
        Active = true;
    }

    IEnumerator DelayResult()
    {
        if (p1.Active && p2.Active)
        {
            p1.Active = false;
            p2.Active = false;
            p1result.text = ResultType.Tie.ToString();
            p2result.text = ResultType.Tie.ToString();
            yield return new WaitForSeconds(2f);
            Completed = true;
            CloseSelf(true);
        }
        else
        {
            p1.Active = false;
            p2.Active = false;
            Camp loser = Camp.None;
            if (p1Time < p2Time)
            {
                p1result.text = ResultType.Win.ToString();
                p2result.text = ResultType.Lose.ToString();
                loser = Camp.Player2;
            }else if (p1Time > p2Time)
            {
                p1result.text = ResultType.Lose.ToString();
                p2result.text = ResultType.Win.ToString();
                loser = Camp.Player1;
            }
            else
            {
                p1result.text = ResultType.Tie.ToString();
                p2result.text = ResultType.Tie.ToString();
            }
            yield return new WaitForSeconds(2);
            if (loser == Camp.None)
            {
                Completed = true;
                CloseSelf(true);
            }
            else
            {
                Map.Instance.GetUser(loser).StopRound++;
                DialogueManager.Instance.ShowDialogue(new DialogueData($"<{loser.Name()}> fails. Pause one turn"), () =>
                {
                    Completed = true;
                    CloseSelf(true);
                });
            }
        }
    }
}
