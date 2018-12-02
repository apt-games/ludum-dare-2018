using UnityEngine;

public static class TrapFactory
{
    public static TrapBehaviour CreateTrap()
    {
        var prefab = Resources.Load("Traps/RoomPrefab");

        var trapBehaviour = (GameObject.Instantiate(prefab, Vector3.zero, Quaternion.identity) as GameObject).GetComponent<TrapBehaviour>();


        return trapBehaviour;
    }
}
