using System.Collections;
using UnityEngine;
using Framework;
using UnityEngine.UI;
using GameProject;

/// <summary>
/// 随机摇骰子界面
/// </summary>
public class RandomPanel : UIBase
{
    [SerializeField] RectTransform rect;
    [SerializeField] Animator ani;
    [SerializeField] Image img;
    [SerializeField] Button btn;
    [SerializeField] Text nameLabel;

    public override UILayer Layer => UILayer.Mask;
    public bool Completed { get; private set; }
    //private bool run = false;
    private float speed = 10000;
    private int dice;
    private bool played;

    // Start is called before the first frame update
    void Start()
    {
        ani.gameObject.SetActive(false);
        //btn.onClick.AddListener(Click);
    }

    /// <summary>
    /// 设置最终显示的点数
    /// </summary>
    public void SetDice(int _dice)
    {
        dice = _dice;
    }

    /// <summary>
    /// 设置用户阵营
    /// </summary>
    public void SetCamp(Camp _camp)
    {
        nameLabel.text = $"[{_camp}]的回合";
        nameLabel.color = _camp.GetColor();
    }

    private void Update()
    {
        if (!played)
        {
            if (Input.anyKeyDown)
            {
                Click();
            }
            return;
        }
        // 每帧旋转
        rect.Rotate(Vector3.forward, speed * Time.deltaTime);
    }

    /// <summary>
    /// 点击骰子,开始播放动画
    /// </summary>
    private void Click()
    {
        played = true;
        AudioManager.Instance.Play("dice");
        ani.gameObject.SetActive(true);
        ani.SetTrigger("Run");
        btn.gameObject.SetActive(false);
        //run = true;

        StartCoroutine(Run());
    }

    private IEnumerator Run()
    {
        yield return new WaitForSeconds(1.5f);
        img.sprite = Resources.Load<Sprite>($"Sprite/{dice}");
        speed = 1200;
        yield return new WaitForSeconds(.2f);
        speed = 0;
        //run = false;
        rect.localEulerAngles = Vector3.zero;
        //rect.DOLocalRotate(Vector3.zero, 0.2f);
        yield return new WaitForSeconds(1f);
        Completed = true;
    }
}
