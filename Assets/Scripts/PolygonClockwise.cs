    using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolygonClockwise : MonoBehaviour
{
    // v1 = x1,y1 이고, v2 = x2,y2 일떄 v1 X v2 (외적) 하였을때 값이 0보다 크다면 
    // v1 과 x축의 각도보다 v2와 x축의 각도가 크다는 의미이다.
    // 즉 v1 X v2 가 0보다 클때는 반시계 방향으로 회전하는 것과 같고, 0이라면 일직선상, 0보다 작다면 시계방향이라는 의미와 같다.
    // 따라서 (O, v1, v2) (O, v2, v3) ... (O, vn, vn1) 의 외적의 크기를 각각 더하여 0보다 클때는 반시계 0보다 작을때는 시계방향이 나온다

    public GameObject offset;
    public GameObject polygons;
    public GameObject lineRenderManager;
    LineRenderer[] lrs;
    List<Vector3> polygonPositions = new List<Vector3>();
    
    void Start()
    {
        lrs = lineRenderManager.GetComponentsInChildren<LineRenderer>();
    }

    void Update()
    {
        // 시계방향이면 다각형을 잇는 선의 색이 초록, 반대라면 노랑
        polygonPositions.Clear();
        foreach(Transform tr in polygons.transform)
            polygonPositions.Add(tr.position);

        float clockWiseSum
            = clockWisePolygon(polygonPositions, new Vector3(0, 1, 0), offset.transform.position);

        Debug.Log(clockWiseSum);

        if (clockWiseSum > 0)
        {
            setLineRenderer(lrs[0], polygonPositions, Color.green, 0.5f);
        }
        else
        {
            setLineRenderer(lrs[0], polygonPositions, Color.yellow, 0.5f);
        }
    }

    // 3개의 점이 만드는 삼각형이 시계방향일때는 더하고, 아닐때는 뺀다
    float clockWisePolygon(List<Vector3> polygon, Vector3 normal, Vector3 offset)
    {
        Vector3 crossSum = Vector3.zero;
        float triangleSum = 0;
        List<Vector3> temp =new List<Vector3>(polygon);

        temp.Add(polygon[0]);

        for(int i = 0; i < polygon.Count; i++)
        {
            crossSum += Vector3.Cross(temp[i], temp[i + 1]);
            if (ccwByPlane(temp[i], temp[i+1], offset, normal, offset) > 0)
            {
                setLineRenderer(lrs[i+1], new List<Vector3>() { temp[i], temp[i+1], offset }, Color.red);
                triangleSum += getAreaOfTriangle(temp[i], temp[i + 1], offset);
            }
            else
            {
                setLineRenderer(lrs[i + 1], new List<Vector3>() { temp[i], temp[i + 1], offset }, Color.blue);
                triangleSum -= getAreaOfTriangle(temp[i], temp[i + 1], offset);
            }
        }

        float ret = Vector3.Dot(crossSum, normal);

        Debug.Log($"Cross Sum : {ret / 2.0f} / triangleSum : {triangleSum}");

        return ret;
    }

    float distancePlaneToPoint(Vector3 normal, Vector3 planeDot, Vector3 point)
    {
        Plane plane = new Plane(normal, planeDot);
        return plane.GetDistanceToPoint(point);
    }

    Vector3 getPositionOnthePlane(Vector3 normal, Vector3 planeDot, Vector3 position)
    {
        float distance = distancePlaneToPoint(normal, planeDot, position);
        return position - normal * distance;
    }

    float ccwByPlane(Vector3 a, Vector3 b, Vector3 c, Vector3 normal, Vector3 planeDot)
    {
        Vector3 d = getPositionOnthePlane(normal, planeDot, a);
        Vector3 e = getPositionOnthePlane(normal, planeDot, b);
        Vector3 f = getPositionOnthePlane(normal, planeDot, c);

        Vector3 p = e - d;
        Vector3 q = f - e;

        return Vector3.Dot(Vector3.Cross(p, q), normal);
    }

    float getAreaOfTriangle(Vector3 dot1, Vector3 dot2, Vector3 dot3)
    {
        Vector3 a = dot2 - dot1; // 1번점에서 2번으로 향하는 벡터
        Vector3 b = dot3 - dot1; // 1번점에서 3번으로 향하는 벡터
        Vector3 cross = Vector3.Cross(a, b);

        return cross.magnitude / 2.0f;
    }

    void setLineRenderer(LineRenderer lr, List<Vector3> list, Color color, float height = 0)
    {
        lr.startWidth = lr.endWidth = .2f;
        lr.material.color = color;
        lr.positionCount = list.Count;

        for (int i = 0; i < list.Count; i++)
            lr.SetPosition(i, new Vector3(list[i].x, height, list[i].z));

        lr.loop = true;
    }
}
