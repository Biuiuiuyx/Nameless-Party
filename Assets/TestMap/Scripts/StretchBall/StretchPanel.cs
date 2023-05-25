using Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using GameProject;

public class StretchPanel : UIBase, IGameCompleted
{
    [SerializeField] StretchPlayer p1;
    [SerializeField] StretchPlayer p2;
    [SerializeField] Ball ball;

    [SerializeField] GameObject infoPanel;
    [SerializeField] Text p1result;
    [SerializeField] Text p2result;

    [SerializeField] Text tip;
    [SerializeField] GameObject mask;

    public override UILayer Layer => UILayer.Panel;
    public bool Completed { get; private set; } = false;

    // Start is called before the first frame update
    void Start()
    {
        ball.onResult += SetWinner;
        infoPanel.SetActive(false);
        StartCoroutine(DelayStart());
    }

    void SetWinner(Camp winnder)
    {
        ball.Active = false;
        ball.gameObject.SetActive(false);
        p1.Active = false;
        p2.Active = false;
        StartCoroutine(DelayResult(winnder));
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
        ball.Active = true;
    }

    IEnumerator DelayResult(Camp winner)
    {
        yield return new WaitForSeconds(.6f);
        infoPanel.SetActive(true);
        if (winner == Camp.Player1)
        {
            p1result.text = ResultType.Win.ToString();
            p2result.text = ResultType.Lose.ToString();
            Map.Instance.GetUser(Camp.Player2).StopRound++;
            yield return new WaitForSeconds(1.5f);
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player2.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }else if (winner == Camp.Player2)
        {
            p1result.text = ResultType.Lose.ToString();
            p2result.text = ResultType.Win.ToString();
            Map.Instance.GetUser(Camp.Player1).StopRound++;
            yield return new WaitForSeconds(1.5f);
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player1.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }
    }
}
