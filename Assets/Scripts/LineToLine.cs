using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineToLine : MonoBehaviour
{
    public Transform point1, point2, point3, point4;
    public Transform cube1, cube2;
    public LineRenderer lr1, lr2, lr3;
    // 3������ �ִ� �� ������ �ִܰŸ� ����
    float getDistanceTwoLine(Vector3 A, Vector3 B, Vector3 C, Vector3 D)
    {
        Vector3 AB = B - A;
        Vector3 CD = D - C;
        Vector3 AC = C - A;
        Vector3 line = Vector3.Cross(AB, CD);

        return Mathf.Abs(Vector3.Dot(AC, line)) / line.magnitude;
    }

    // 3������ �ִ� �� ������ �ִܰŸ��� ����� ������ �� ������ ��� �����̾�� �Ѵ�
    // ���� �� ������ ������ ����AB �� ���� CD �� �����̴�.
    // �̶�, ����� ���Ҿ �������� ���ؾ� �ϴµ�, ���� �Ѱ��� �� ������ ������ ���͸� ����ϸ� �������� ���� �� �ִ�.
    // ������ ���� AB�� �ٽ� �����ϰ� ���� AB�� �����ϴ� ����� ���Ѵ�

    // ���� ������ ���� ��ǥ�� ���ϴ� �������� CD�� �ִ� �ִ� �Ÿ��� ����� ���� ���� �� �ִ�.
    Vector3 getContactPoint(Vector3 normal, Vector3 planeDot, Vector3 A, Vector3 B)
    {
        Vector3 nAB = (B - A).normalized;

        return A + nAB * Vector3.Dot(normal, planeDot - A) / Vector3.Dot(normal, nAB);
    }

    // ����Ƽ C#�� Ʃ���� �̿��Ͽ� �� ���� �����ϴ� �Լ��� ����� �ش�
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
