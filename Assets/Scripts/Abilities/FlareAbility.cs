using UnityEngine;

public class FlareAbility : BaseAbility
{
    public GameObject Prefab;

    public override void Use()
    {
        Debug.Log("Used Flare!");

        // make flare
        var flare = Instantiate(Prefab, transform.position, Quaternion.identity);

        var room = GameController.Instance.SelectedRoom;
        room.SetVisited(true);

        var target = room.transform.position + new Vector3(Random.Range(-0.05f, 0.05f), Random.Range(-0.05f, 0.05f), 0);

        iTween.MoveTo(flare, target, 2f);

        foreach (Transform child in flare.transform)
            iTween.RotateBy(child.gameObject, new Vector3(0, 0, Random.Range(1f, 3f)), 2f);

        base.Use();
    }
}
