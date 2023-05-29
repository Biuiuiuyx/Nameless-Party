using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rd = GameProject.Random;

public class AnswerPanel : UIBase, IGameCompleted
{
    [SerializeField] AnswerPlayer p1;
    [SerializeField] AnswerPlayer p2;
    [SerializeField] Text title;
    [SerializeField] Text A;
    [SerializeField] Text B;
    [SerializeField] Text C;
    [SerializeField] Text D;

    [SerializeField] Text p1selected;
    [SerializeField] Text p1flag;
    [SerializeField] Text p1result;

    [SerializeField] Text p2selected;
    [SerializeField] Text p2flag;
    [SerializeField] Text p2result;

    [SerializeField] Text tip;
    [SerializeField] GameObject mask;

    int correct;

    public override UILayer Layer => UILayer.Panel;
    public bool Active { get; set; } = false;
    public bool Completed { get; private set; } = false;
    Dictionary<int, Text> dic;

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
        title.text = "";
        A.gameObject.SetActive(false);
        B.gameObject.SetActive(false);
        C.gameObject.SetActive(false);
        D.gameObject.SetActive(false);
        p1selected.text = "";
        p1result.text = "";
        p1flag.text = "";
        p2selected.text = "";
        p2result.text = "";
        p2flag.text = "";
    }

    void ShowP1Result(int idx)
    {
        if (Active)
        {
            p1.Active = false;
            p1selected.text = dic[idx].text;
            p1flag.text = idx == correct ? "Right!" : "Wrong!";
            if (p2.Active)
            {
                Active = false;
                StartCoroutine(DelayResult(idx == correct ? Camp.Player2 : Camp.Player1));
            }
        }
    }

    void ShowP2Result(int idx)
    {
        if (Active)
        {
            p2.Active = false;
            p2selected.text = dic[idx].text;
            p2flag.text = idx == correct ? "Right!" : "Wrong!";
            if (p1.Active)
            {
                Active = false;
                StartCoroutine(DelayResult(idx == correct ? Camp.Player1 : Camp.Player2));
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

        int x0 = Rd.GetValue(10, 50);
        int x1 = Rd.GetValue(10, 50);
        int y = x0 + x1;

        string str = $"{x0} + {x1} = ?";
        title.text = "";
        foreach (char letter in str.ToCharArray())
        {
            title.text += letter;
            yield return new WaitForSeconds(0.04f);
        }
        yield return new WaitForSeconds(1);
        List<int> temps = new List<int>();
        int c0 = y;
        int min = y - 10;
        int max = y + 10;
        for (int i = min; i < c0; i++)
        {
            temps.Add(i);
        }
        for (int j = c0 + 1; j <= max; j++)
        {
            temps.Add(j);
        }
        List<int> ys = new List<int>(4);
        int idx = 0;
        int count = 3;
        while(count > 0)
        {
            int r = Rd.GetValue(idx, temps.Count);
            int t = temps[r];
            ys.Add(t);
            temps[r] = temps[idx];
            temps[idx] = t;
            idx++;
            count--;
        }

        dic = new Dictionary<int, Text>(4);
        correct = Rd.GetValue(0, 4);
        ys.Insert(correct, y);
        A.text = $"A.{ys[0]}";
        A.gameObject.SetActive(true);
        dic.Add(0, A);
        B.text = $"B.{ys[1]}";
        B.gameObject.SetActive(true);
        dic.Add(1, B);
        C.text = $"C.{ys[2]}";
        C.gameObject.SetActive(true);
        dic.Add(2, C);
        D.text = $"D.{ys[3]}";
        D.gameObject.SetActive(true);
        dic.Add(3, D);

        p1.Active = true;
        p2.Active = true;
        Active = true;
    }

    IEnumerator DelayResult(Camp loser)
    {
        p1.Active = false;
        p2.Active = false;
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
