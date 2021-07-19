using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class MeshExtention
{
    public static void Resize(this Mesh mesh, Vector3 scale)
    {
        Vector3[] vertices = mesh.vertices.ToList().Select(v => new Vector3(v.x * scale.x, v.y * scale.y, v.z * scale.z)).ToArray();
        mesh.vertices = vertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    public static void Center(this Mesh mesh)
    {
        List<Vector3> vertices = mesh.vertices.ToList();
        Vector3 pivot = vertices.Aggregate(Vector3.zero, (acc, curr) => acc + curr) / vertices.Count;
        Vector3[] targetVertices = vertices.Select(v => v - pivot).ToArray();
        mesh.vertices = targetVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }

    public static void Center(this Mesh mesh, Vector3 pivot)
    {
        List<Vector3> vertices = mesh.vertices.ToList();
        Vector3[] targetVertices = vertices.Select(v => v - pivot).ToArray();
        mesh.vertices = targetVertices;
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
    }
}
