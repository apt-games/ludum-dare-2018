using UnityEngine;

public class StoneAbility : BaseAbility
{
    public GameObject Prefab;

    public override void Use()
    {
        Debug.Log("Used Stone!");

        // make flare
        var stone = Instantiate(Prefab, transform.position, Quaternion.identity);

        var room = GameController.Instance.SelectedRoom;
        room.SetVisited(true);

        var target = room.transform.position;

        iTween.MoveTo(stone, target, 2f);

        base.Use();
    }
}
