using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class MeshController : MonoBehaviour
{
    public List<MeshFilter> filters;
    public Vector3 size = new Vector3(0.05f, 0.05f, 0.05f);
    public Vector3 rotation = new Vector3(-90, 180, 0);

    void Start()
    {
        Center();
        Resize(size);
        Rotate(rotation);

    }

    public void Center()
    {
        CombineInstance[] combines = filters.Select(f =>
        {
            CombineInstance combine = new CombineInstance();
            combine.mesh = f.sharedMesh;
            combine.transform = f.transform.localToWorldMatrix;

            return combine;
        }).ToArray();

        if (combines.Length > 0)
        {
            Mesh mesh = new Mesh();
            mesh.CombineMeshes(combines);

            List<Vector3> vertices = mesh.vertices.ToList();
            Vector3 pivot = vertices.Aggregate(Vector3.zero, (acc, curr) => acc + curr) / vertices.Count;
            filters.ForEach(f =>
            {
                f.mesh.Center(pivot);
            });
        }
    }

    public void Resize(Vector3 size)
    {
        filters.ForEach(f =>
        {
            f.mesh.Resize(size);
        });
    }

    public void Rotate(Vector3 rotation)
    {
        filters.ForEach(f => f.transform.localRotation = Quaternion.Euler(rotation));
    }
}