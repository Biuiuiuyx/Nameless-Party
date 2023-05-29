using UnityEngine;
using UnityEngine.UI;

public class ArrowView : MonoBehaviour
{
    [SerializeField] Image flag;
    [SerializeField] Text label;
    [SerializeField] Color rightColor;
    [SerializeField] Color wrongColor;

    public void SetDirection(Direction dir)
    {
        label.text = dir.GetDesc();
    }

    public void ShowCorrect(bool isCorrect)
    {
        flag.color = isCorrect ? rightColor : wrongColor;
    }
}
