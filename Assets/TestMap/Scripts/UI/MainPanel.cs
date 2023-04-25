using Framework;
using UnityEngine;

public class MainPanel : UIBase
{
    [SerializeField] UserView player1;
    [SerializeField] UserView player2;

    public override UILayer Layer => UILayer.Panel;

    // Start is called before the first frame update
    void Start()
    {
        var m = Map.Instance;
        var camp = Camp.Player1;
        var p1 = m.GetUser(camp);
        player1.ShowFlag(false);
        player1.SetCamp(camp);
        player1.ShowName(camp.Name());
        player1.ShowGold(p1.Gold);
        p1.onChangeGold += player1.ShowGold;

        camp = Camp.Player2;
        var p2 = m.GetUser(camp);
        player2.ShowFlag(false);
        player2.SetCamp(camp);
        player2.ShowName(camp.Name());
        player2.ShowGold(p2.Gold);
        p2.onChangeGold += player2.ShowGold;

        m.onCamp += ChangePlayer;
    }

    void ChangePlayer(Camp camp)
    {
        player1.ShowFlag(camp == Camp.Player1);
        player2.ShowFlag(camp == Camp.Player2);
    }
}
