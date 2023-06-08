using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 格子视图
/// </summary>
public class GridView : MonoBehaviour
{
    [SerializeField] SpriteRenderer render;

    Grid g;
    public Grid Grid
    {
        get
        {
            if (g == null)
            {
                g = GetComponent<Grid>();
            }
            return g;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        ShowGridType(Grid.Type);
    }

    public void Init()
    {
        ShowGridType(Grid.Type);
    }

    /// <summary>
    /// 显示格子类型
    /// </summary>
    private void ShowGridType(GridType _type)
    {
        if (render != null)
        {
            render.sprite = Resources.Load<Sprite>($"Grid/{_type.Image()}");
            render.color = _type.GetColor();
        }
        //render.sprite = Resources.Load<Sprite>($"Grid/{_type}");
    }
}
