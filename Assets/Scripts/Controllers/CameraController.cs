using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void ShowRoom(RoomBehaviour room)
    {
        var roomPosition = room.transform.position;
        var target = new Vector3(roomPosition.x, roomPosition.y, transform.position.z);
        iTween.MoveTo(gameObject, target, 2f);
    }
}
