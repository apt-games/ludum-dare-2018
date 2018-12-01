using UnityEngine;

public class CharacterBehaviour : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void MoveTo(Transform room)
    {
        Debug.Log("Moving character to " + room.position);

        var target = new Vector3(room.position.x, room.position.y, transform.position.z);

        iTween.MoveTo(gameObject, target, 1f);
    }
}
