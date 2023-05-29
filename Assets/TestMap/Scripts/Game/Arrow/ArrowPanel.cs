using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rd = GameProject.Random;

public class ArrowPanel : UIBase, IGameCompleted
{
    [SerializeField] ArrowPlayer p1;
    [SerializeField] ArrowPlayer p2;
    [SerializeField] ArrowView arrow;
    [SerializeField] Transform container;
    [SerializeField] int count = 7;

    [SerializeField] Text p1flag;
    [SerializeField] Text p1result;
    [SerializeField] Text p2flag;
    [SerializeField] Text p2result;
 
    [SerializeField] Text tip;
    [SerializeField] GameObject mask;

    public override UILayer Layer => UILayer.Panel;
    public bool Active { get; set; } = false;
    public bool Completed { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        p1.onWrong += ShowWrong;
        p2.onWrong += ShowWrong;
        p1.onCompleted += ShowCompleted;
        p2.onCompleted += ShowCompleted;
        ToReset();
        StartCoroutine(DelayStart());
    }

    void ToReset()
    {
        p1result.text = "";
        p1flag.text = "";
        p2result.text = "";
        p2flag.text = "";
    }

    void ShowWrong(Camp loser)
    {
        if (Active)
        {
            p1.Active = false;
            p2.Active = false;
            Active = false;
            if (loser == Camp.Player1)
            {
                p1flag.text = "GAME OVER";
            }
            else
            {
                p2flag.text = "GAME OVER";
            }
            StartCoroutine(DelayResult(loser));
        }
    }

    void ShowCompleted(Camp winner)
    {
        if (Active)
        {
            p1.Active = false;
            p2.Active = false;
            Active = false;
            if (winner == Camp.Player1)
            {
                p1flag.text = "PERFECT!";
                StartCoroutine(DelayResult(Camp.Player2));
            }
            else
            {
                p2flag.text = "PERFECT!";
                StartCoroutine(DelayResult(Camp.Player1));
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

        List<Direction> list = new List<Direction>(count);
        for (int i = 0; i < count; i++)
        {
            Direction d = (Direction)Rd.GetValue(0, 4);
            var a = Instantiate(arrow, container);
            a.SetDirection(d);
            a.gameObject.SetActive(true);
            list.Add(d);
        }

        p1.SetQueue(list);
        p2.SetQueue(list);
        p1.Active = true;
        p2.Active = true;
        Active = true;
    }

    IEnumerator DelayResult(Camp loser)
    {
        if (loser == Camp.Player1)
        {
            p1result.text = ResultType.Lose.ToString();
            p2result.text = ResultType.Win.ToString();
        }
        else if (loser == Camp.Player2)
        {
            p1result.text = ResultType.Win.ToString();
            p2result.text = ResultType.Lose.ToString();
        }
        yield return new WaitForSeconds(2);
        Map.Instance.GetUser(loser).StopRound++;
        DialogueManager.Instance.ShowDialogue(new DialogueData($"<{loser.Name()}> fails. Pause one turn"), () =>
        {
            Completed = true;
            CloseSelf(true);
        });
    }
}
