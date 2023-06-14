using Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Rd = GameProject.Random;

public class RandGamePanel : UIBase, IGameCompleted
{
    [SerializeField]
    private Text text;
    public bool Completed { get; private set; } = false;

    public override UILayer Layer => UILayer.Panel;

    // Start is called before the first frame update
    void Start()
    {
        text.text = "Getting";
        StartCoroutine(GetRandGame());
    }
    IEnumerator GetRandGame()
    {
        yield return new WaitForSeconds(1);
        int game = Rd.GetValue(1, 3);
        text.text = $"{game}";
        yield return new WaitForSeconds(1);
        if (game == 1)
        {
            var panel = UIManager.Instance.Open<PokerPanel>();
            yield return new WaitUntil(() => panel.Completed);
        }
        else if (game == 2)
        {
            var panel = UIManager.Instance.Open<PokerPanel>();
            yield return new WaitUntil(() => panel.Completed);
        }
        CloseSelf(true);
        Completed = true;
    }
}
