using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointToLine : MonoBehaviour
{
    public Transform pointA, pointB, target, perpendicular;

    public GameObject lineRenderer;
    LineRenderer[] lrs;

    // 직선 AB와 점 사이의 거리
    float getDistancePointAndLine(Vector3 a, Vector3 b, Vector3 point)
    {
        Vector3 AB = b - a;
        return (Vector3.Cross(point - a, AB)).magnitude / AB.magnitude;
    }

    Vector3 getPerpendicularOntoLine(Vector3 A, Vector3 B, Vector3 D)
    {
        // 기존 직선
        Vector3 line1 = A - B;

        // 점과 끝점을 이은 선
        Vector3 line2 = D - B;

        // 두 선의 각도 (내적을 이용함)
        float cos = Vector3.Dot(line1, line2) / (line1.magnitude * line2.magnitude);

        // 두 점 사이의 거리
        float lengthBD = Vector3.Distance(B, D);

        // 점의 수직으로 내린 점과 B사이의 거리
        float projectionLength = lengthBD * cos;

        Vector3 normalAB = (B - A).normalized;

        return (B - normalAB * projectionLength);
    }

    void setLineRenderer(LineRenderer lr, Color color)
    {
        lr.startWidth = lr.endWidth = .2f;
        lr.material.color = color;
        lr.positionCount = 2;
    }

    void Start()
    {
        lrs = lineRenderer.GetComponentsInChildren<LineRenderer>();

        setLineRenderer(lrs[0], Color.red);
        setLineRenderer(lrs[1], Color.black);
    }

    void Update()
    {
        lrs[0].SetPosition(0, pointA.position);
        lrs[0].SetPosition(1, pointB.position);

        lrs[1].SetPosition(0, target.position);
        lrs[1].SetPosition(1, perpendicular.position);

        perpendicular.position
            = getPerpendicularOntoLine(pointA.position, pointB.position, target.position);
    }
}

