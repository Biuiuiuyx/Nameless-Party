using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 用户视图，头像面板
/// </summary>
public class UserView : MonoBehaviour
{
    [SerializeField] protected Image chess;     // 棋子图片
    [SerializeField] protected Text nameLabel;  // 名字
    [SerializeField] protected Text goldLabel;  // 金币
    [SerializeField] protected GameObject flag; // 是否正在操作的标记

    private Camp camp;
    public Camp Camp => camp;

    public void SetCamp(Camp _camp)
    {
        camp = _camp;
    }

    public void ShowSprite(Sprite _sp)
    {
        chess.sprite = _sp;
    }

    public void ShowFlag(bool _show)
    {
        flag.SetActive(_show);
    }

    public void ShowName(string _name)
    {
        nameLabel.text = _name;
    }

    public void ShowGold(int _gold)
    {
        goldLabel.text = $"{_gold}";
    }
}
