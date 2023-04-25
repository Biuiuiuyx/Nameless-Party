using UnityEngine;
using Framework;
using UnityEngine.UI;
using GameProject;

/// <summary>
/// 开始界面
/// </summary>
public class StartPanel : UIBase
{
    [SerializeField] Button startBtn;       // 开始按钮
    [SerializeField] Button exitBtn;        // 退出按钮

    public override UILayer Layer => UILayer.Panel;

    // Start is called before the first frame update
    void Start()
    {
        startBtn.onClick.AddListener(ToStart);
        exitBtn.onClick.AddListener(GameManager.ExitGame);
    }

    private void ToStart()
    {
        UIMask.Instance.FadeInAndOut(() =>
        {
            Map.Load();
            CloseSelf(false);
        }, .4f, () =>
        {
            CloseSelf(true);
        });
    }
}
