using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 漂浮文字
/// </summary>
public class FloatView : MonoBehaviour
{
    [SerializeField] RectTransform rect;
    [SerializeField] Text text;

    /// <summary>
    /// 显示漂浮文字
    /// </summary>
    /// <param name="_str">文本</param>
    /// <param name="_pos">位置</param>
    /// <param name="_color">颜色</param>
    /// <param name="_showTime">时间</param>
    public void ShowFloatStr(string _str, Vector2 _pos, Color _color, float _showTime = 1.5f)
    {
        rect.anchoredPosition = _pos;
        text.text = _str;
        text.color = _color;
        if (!string.IsNullOrWhiteSpace(_str) && _showTime > 0)
        {
            gameObject.SetActive(true);
            Sequence seq = DOTween.Sequence();
            seq.Append(text.DOFade(1, 0.1f).From(0));
            seq.Append(text.transform.DOLocalMoveY(_pos.y + 4, _showTime));
            seq.Append(text.transform.DOLocalMoveY(_pos.y + 20, 0.5f));
            //seq.AppendInterval(showTime);
            seq.Insert(_showTime + 0.2f, text.DOFade(0, 0.5f));
            seq.AppendCallback(OnEnd);
        }
        else
        {
            OnEnd();
        }
    }

    /// <summary>
    /// 显示结束后销毁
    /// </summary>
    private void OnEnd()
    {
        Destroy(gameObject);
    }
}
