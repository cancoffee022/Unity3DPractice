using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DivideLine : MonoBehaviour
{
    public Transform p1, p2;

    List<Vector3> getDivideLinePoint(Vector3 a, Vector3 b, int n)
    {
        Vector3 divide = (b - a) / n;
        List<Vector3> points = new List<Vector3>();

        for(int i = 1; i < n; i++) points.Add(a + divide * i);

        return points;
    }

    void Start()
    {
        List<Vector3> points = getDivideLinePoint(p1.position, p2.position, 5);

        foreach(Vector3 v in points)
        {
            GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            go.transform.position = v;
        }
    }
}
