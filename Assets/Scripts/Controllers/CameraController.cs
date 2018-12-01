using UnityEngine;

public class CameraController : MonoBehaviour
{
    public void ShowRoom(Transform room)
    {
        var target = new Vector3(room.position.x, room.position.y, transform.position.z);
        iTween.MoveTo(gameObject, target, 2f);
    }
}
