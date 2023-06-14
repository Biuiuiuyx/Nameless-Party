using Framework;
using GameProject;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 游戏结束界面
/// </summary>
public class GameOverPanel : UIBase
{
    [SerializeField] Text title;            // 胜者标题
    [SerializeField] Button restartBtn;     // 重新开始按钮
    [SerializeField] Button exitBtn;        // 退出按钮

    public override UILayer Layer => UILayer.Panel;

    // Start is called before the first frame update
    void Start()
    {
        restartBtn.onClick.AddListener(ToRestart);
        exitBtn.onClick.AddListener(GameManager.ExitGame);
        AudioManager.Instance.Play("Victory1");
    }

    public void ShowWinner(User _winner)
    {
        title.text = $"<{_winner.Name}>获得最终的胜利！";
    }

    private void ToRestart()
    {
        UIMask.Instance.FadeInAndOut(() =>
        {
            UIManager.Instance.Close(nameof(GameOverPanel), false);
            Map.Load();
        });
    }
}
