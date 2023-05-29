using DG.Tweening;
using Framework;
using GameProject;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DodgePanel : UIBase, IGameCompleted
{
    [SerializeField] DodgePlayer p1;
    [SerializeField] DodgePlayer p2;
    [SerializeField] Block block;
    [SerializeField] Transform blockContainer;

    [SerializeField] Text p1flag;
    [SerializeField] Text p1result;
    [SerializeField] Text p2flag;
    [SerializeField] Text p2result;

    [SerializeField] Text tip;
    [SerializeField] GameObject mask;
    [SerializeField] float startY;
    [SerializeField] float[] speeds = new float[] { 1, 2, 3,4,5,6};

    int speedIndex = 0;
    float curSpeed;
    public override UILayer Layer => UILayer.Panel;

    public bool Completed { get; set; } = false;
    public bool Active { get; set; } = false;

    CoolDown duringCD;
    CoolDown blockCD;
    CoolDown waitCD;
    bool isWait = false;

    // Start is called before the first frame update
    void Start()
    {
        duringCD = new CoolDown(5f);
        duringCD.onCompleted += DuringCompleted;
        blockCD = new CoolDown(.6f);
        blockCD.onCompleted += CreateBlock;
        waitCD = new CoolDown(1);
        waitCD.onCompleted += OnWait;
        p1.onDeath += ShowResult;
        p2.onDeath += ShowResult;
        ToReset();
        StartCoroutine(DelayStart());
    }

    private void FixedUpdate()
    {
        if (!Active) return;
        if (isWait)
        {
            waitCD.AddDeltaTime(Time.fixedDeltaTime);
        }
        else
        {
            duringCD.AddDeltaTime(Time.fixedDeltaTime);
            blockCD.AddDeltaTime(Time.fixedDeltaTime);
        }
    }

    void ToReset()
    {
        p1result.text = "";
        p1flag.text = "";
        p2result.text = "";
        p2flag.text = "";
    }

    void ShowResult(Camp loser)
    {
        if (Active)
        {
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
        speedIndex = 0;
        curSpeed = speeds[0];
        p1.Active = true;
        p2.Active = true;
        Active = true;
    }

    void CreateBlock()
    {
        bool left = GameProject.Random.GetValue(0, 2) == 0 ? true : false;
        var b1 = Instantiate(block, blockContainer);
        b1.Speed = curSpeed;
        var b2 = Instantiate(block, blockContainer);
        b2.Speed = curSpeed;
        if (left)
        {
            b1.transform.position = new Vector3(p1.LeftX, startY, 0);
            b2.transform.position = new Vector3(p2.LeftX, startY, 0);
        }
        else
        {
            b1.transform.position = new Vector3(p1.RightX, startY, 0);
            b2.transform.position = new Vector3(p2.RightX, startY, 0);
        }
        b1.gameObject.SetActive(true);
        b2.gameObject.SetActive(true);
    }

    void AddSpeed()
    {
        if (speedIndex < speeds.Length - 1)
        {
            UIWarn.Instance.ShowWarn("SPEED UP!", .4f);
            speedIndex++;
            curSpeed = speeds[speedIndex];
        }
    }

    void DuringCompleted()
    {
        if (speedIndex < speeds.Length - 1)
        {
            isWait = true;
        }
    }

    void OnWait()
    {
        AddSpeed();
        isWait = false;
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
