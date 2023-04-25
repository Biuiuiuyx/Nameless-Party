using UnityEngine;
using Framework;

/// <summary>
/// 漂浮文字管理器
/// </summary>
public class FloatManager : MonoSingleton<FloatManager>
{
    private FloatView fv;           // 漂浮文字
    private Transform container;    // 容器

    public override void OnInit()
    {
        // 初始化加载漂浮文字预制体
        fv = Resources.Load<FloatView>("Float/FloatView");
        // 初始化容器
        container = GameObject.FindGameObjectWithTag("Canvas").transform.Find("Warn");
    }

    /// <summary>
    /// 展示新的漂浮文字
    /// </summary>
    /// <param name="_str">文本</param>
    /// <param name="_pos">位置</param>
    /// <param name="_color">颜色</param>
    /// <param name="_showTime">显示时间</param>
    public void ShowFloatStr(string _str, Vector2 _pos, Color _color, float _showTime = 1.5f)
    {
        var f = Instantiate(fv, container);
        f.ShowFloatStr(_str, _pos, _color, _showTime);
    }
}
