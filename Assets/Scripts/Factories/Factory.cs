using System;
using UnityEngine;
using Random = UnityEngine.Random;

public static class Factory
{
    private static readonly int[] _rotations = { 0, 90, 180, 270 };

    public static T LoadPrefab<T>(string path)
    {
        try
        {
            return (GameObject.Instantiate(Resources.Load(path), Vector3.zero, Quaternion.identity) as GameObject)
                .GetComponent<T>();
        }
        catch
        {
            throw new NullReferenceException("Failed to load " + path);
        }
    }
    public static T LoadPrefab<T>(string path, Vector3 position, Transform parent)
    {
        try
        {
            return (GameObject.Instantiate(Resources.Load(path), position, Quaternion.identity, parent) as GameObject)
                .GetComponent<T>();
        }
        catch
        {
            throw new NullReferenceException("Failed to load " + path);
        }
    }

    public static int RandomRotation => _rotations[Random.Range(0, _rotations.Length)];
}
