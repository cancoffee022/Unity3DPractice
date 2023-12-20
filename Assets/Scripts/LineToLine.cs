using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToLine : MonoBehaviour
{
    public Transform point1, point2, point3, point4;
    public Transform cube1, cube2;
    public LineRenderer lr1, lr2, lr3;
    // 3차원에 있는 두 직선의 최단거리 공식
    float getDistanceTwoLine(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        Vector3 AB = B - A;
        Vector3 CD = D - C;
        Vector3 AC = C - A;
        Vector3 line = Vector3.Cross(AB, CD);

        return Mathf.Abs(Vector3.Dot(AC, line)) / line.magnitude;
    }

    // 3차원에 있는 두 직선의 최단거리를 만드는 직선은 두 직선에 모두 수직이어야 한다
    // 따라서 이 직선의 방향은 벡터AB 와 벡터 CD 의 외적이다.
    // 이때, 방향과 더불어서 교차점을 구해야 하는데, 직선 한개와 두 직선의 수직인 벡터를 사용하면 교차점을 구할 수 있다.
    // 외적과 직선 AB를 다시 외적하고 직선 AB를 포함하는 평면을 구한다

    // 평면과 직전의 접점 좌표를 구하는 공식으로 CD에 있는 최단 거리를 만드는 점을 구할 수 있다.
    Vector3 getContactPoint(Vector3 normal, Vector3 planeDot, Vector3 A, Vector3 B)
    {
        Vector3 nAB = (B - A).normalized;

        return A + nAB * Vector3.Dot(normal, planeDot - A) / Vector3.Dot(normal, nAB);
    }

    // 유니티 C#의 튜플을 이용하여 두 점을 리턴하는 함수를 만들어 준다
    (Vector3 point1, Vector3 point2) getShortestPath(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        Vector3 AB = B - A;
        Vector3 CD = D - C;
        Vector3 line = Vector3.Cross(AB, CD);
        Vector3 crossLineAB = Vector3.Cross(line, AB);
        Vector3 crossLineCD = Vector3.Cross(line, CD);

        return (getContactPoint(crossLineAB, A, C, D), getContactPoint(crossLineCD, C, A, B));
    }

    void setLineRenderer(LineRenderer lr, List<Vector3> list, Color color)
    {
        lr.startWidth = lr.endWidth = .2f;
        lr.material.color = color;
        lr.positionCount = list.Count;

        for (int i = 0; i < list.Count; i++)
            lr.SetPosition(i, list[i]);
    }

    void Update()
    {
        Vector3 p1, p2, p3, p4;

        p1 = point1.position;
        p2 = point2.position;
        p3 = point3.position;
        p4 = point4.position;

        (Vector3 linePoint1, Vector3 linePoint2)
            = getShortestPath(p1, p2, p3, p4);

        cube1.position = linePoint1;
        cube2.position = linePoint2;

        setLineRenderer(lr1, new List<Vector3>() { p1, p2 }, Color.red);
        setLineRenderer(lr2, new List<Vector3>() { p3, p4 }, Color.blue);
        setLineRenderer(lr3, new List<Vector3>() { cube1.position, cube2.position }, Color.green);

        float distanceCube1 = getDistanceTwoLine(p1, p2, p3, p4);
        float distanceCube2 = Vector3.Distance(cube1.position, cube2.position);

        Debug.Log(distanceCube1 + " / " + distanceCube2);
    }
}
