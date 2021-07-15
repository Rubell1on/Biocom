using UnityEditor;
using UnityEngine;
using System;

[Serializable]
public static class GameObjectExtension
{
    [SerializeField]
    public static void Destroy1(this GameObject gameObject)
    {
        GameObject.Destroy(gameObject);
    }
}