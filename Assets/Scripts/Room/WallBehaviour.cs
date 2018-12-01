using UnityEngine;

public class WallBehaviour : MonoBehaviour
{
    public GameObject Left;
    public GameObject Right;
    public GameObject Door;

    public void SetIsDoor(bool isDoor = true)
    {
        Door.SetActive(!isDoor);
    }
}