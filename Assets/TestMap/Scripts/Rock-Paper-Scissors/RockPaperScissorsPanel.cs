using Framework;
using GameProject;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

/// <summary>
/// 剪刀石头布界面
/// </summary>
public class RockPaperScissorsPanel : UIBase
{
    [SerializeField] Text p1Rock;
    [SerializeField] Text p1Paper;
    [SerializeField] Text p1Scissors;

    [SerializeField] Text p2Rock;
    [SerializeField] Text p2Paper;
    [SerializeField] Text p2Scissors;

    [SerializeField] RockPaperScissorsPlayer p1;
    [SerializeField] RockPaperScissorsPlayer p2;

    [SerializeField] Image p1pic;
    [SerializeField] GameObject p1tip;
    [SerializeField] Text p1result;
    [SerializeField] Image p2pic;
    [SerializeField] GameObject p2tip;
    [SerializeField] Text p2result;

    [SerializeField] Text tip;
    [SerializeField] GameObject mask;

    bool isP1completed;
    bool isP2completed;
    RockPaperScissorsType p1type;
    RockPaperScissorsType p2type;

    public bool Completed { get; private set; } = false;

    public override UILayer Layer => UILayer.Panel;

    // Start is called before the first frame update
    void Start()
    {
        ToReset();
        p1Rock.text = p1.GetKey(RockPaperScissorsType.Rock).ToString();
        p1Paper.text = p1.GetKey(RockPaperScissorsType.Paper).ToString();
        p1Scissors.text = p1.GetKey(RockPaperScissorsType.Scissors).ToString();

        p2Rock.text = p2.GetKey(RockPaperScissorsType.Rock).ToString();
        p2Paper.text = p2.GetKey(RockPaperScissorsType.Paper).ToString();
        p2Scissors.text = p2.GetKey(RockPaperScissorsType.Scissors).ToString();

        p1.onKeyDown += t =>
        {
            isP1completed = true;
            p1.Active = false;
            p1type = t;
            p1tip.SetActive(true);
            Check();
        };
        p2.onKeyDown += t =>
        {
            isP2completed = true;
            p2.Active = false;
            p2type = t;
            p2tip.SetActive(true);
            Check();
        };
        StartCoroutine(DelayStart());
    } 

    void ToReset()
    {
        p1pic.gameObject.SetActive(false);
        p1tip.SetActive(false);
        p1result.text = "";
        isP1completed = false;

        p2pic.gameObject.SetActive(false);
        p2tip.SetActive(false);
        p2result.text = "";
        isP2completed = false;
    }

    void Check()
    {
        if (isP1completed && isP2completed)
        {
            StartCoroutine(DelayResult());
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
    }

    IEnumerator DelayResult()
    {
        yield return new WaitForSeconds(1.2f);
        p1pic.sprite = Resources.Load<Sprite>($"Sprite/{p1type}");
        p1pic.gameObject.SetActive(true);
        p1tip.SetActive(false);

        p2pic.sprite = Resources.Load<Sprite>($"Sprite/{p2type}");
        p2pic.gameObject.SetActive(true);
        p2tip.SetActive(false);

        int r = p1type - p2type;
        ResultType result = ResultType.Tie;
        if (r == 1 || r == -2)
        {
            p1result.text = ResultType.Win.ToString();
            p2result.text = ResultType.Lose.ToString();
            Map.Instance.GetUser(Camp.Player2).StopRound++;
            result = ResultType.Win;
        }else if (r == 0)
        {
            p1result.text = ResultType.Tie.ToString();
            p2result.text = ResultType.Tie.ToString();
        }
        else
        {
            p1result.text = ResultType.Lose.ToString();
            p2result.text = ResultType.Win.ToString();
            Map.Instance.GetUser(Camp.Player1).StopRound++;
            result = ResultType.Lose;
        }
        yield return new WaitForSeconds(2f);
        if (result == ResultType.Win)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player2.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }else if (result == ResultType.Lose)
        {
            DialogueManager.Instance.ShowDialogue(new DialogueData($"<{Camp.Player1.Name()}> fails. Pause one turn"), () =>
            {
                Completed = true;
                CloseSelf(true);
            });
        }else
        {
            Completed = true;
            CloseSelf(true);
        }
        
    }
}
