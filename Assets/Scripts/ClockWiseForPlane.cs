using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockWiseForPlane : MonoBehaviour
{
    // 평면 상에서 시계 방향인지 반시계 방향인지 판단하는 식

    public Material red, blue;
    public GameObject[] planeObject;
    public GameObject[] clockCheckObject;
    LineRenderer lr;
    List<GameObject> dotOnthePlane = new List<GameObject>();
    
    float ccwBy2D(Vector3 a, Vector3 b, Vector3 c) // 시점이 바뀌지 않고 3점 모두 y축이 0이여야한다
    {
        Vector3 p = b - a;
        Vector3 q = c - b;

        return Vector3.Cross(p, q).y;
    }
    
    // 3D 환경에서는 3점이 이어진 순서가 바뀌지 않아도, 시점에 따라 시계방향, 반시계방향이 바뀔 수 있다

    float distancePlaneToPoint(Vector3 normal, Vector3 planeDot, Vector3 point)
    {
        Plane plane = new Plane(normal, planeDot);
        return plane.GetDistanceToPoint(point);
    }// 평면과 점 사이의 최단거리 구하는 방법 (내적을 통해 구하는 방식)

    Vector3 getPositionOnthePlane(Vector3 normal, Vector3 planeDot, Vector3 position)
    {
        float distance = distancePlaneToPoint(normal, planeDot, position);
        return position - normal * distance;
    }// 구한 최단거리를 통해 평면안에서 점과 가장 가까운 위칠를 구함

    // 위의 함수를 통해 해당하는 평면에 대한 방향 판단을 할 수 있는 함수를 만든다
    float ccwByPlane(Vector3 a, Vector3 b, Vector3 c, Vector3 normal, Vector3 planeDot)
    {
        Vector3 a_OnthePlane = getPositionOnthePlane(normal, planeDot, a);
        Vector3 b_OnthePlane = getPositionOnthePlane(normal, planeDot, b);
        Vector3 c_OnthePlane = getPositionOnthePlane(normal, planeDot, c);

        Vector3 p = b_OnthePlane - a_OnthePlane;
        Vector3 q = c_OnthePlane - b_OnthePlane;
        // 각 점들이 평면에 수직되는 점들을 구하고 그 점에서 다른 점으로가는 벡터를 구한다

        return Vector3.Dot(Vector3.Cross(p, q), normal);
        // 벡터들을 외적한뒤에 평면의 normal벡터와 내적을 하여 양수라면 시계방향, 음수라면 반시계방향이 나온다
    }

    void Start()
    {
        /*Vector3 d1, d2, d3;

        d1 = new Vector3(0, 0, 0);
        d2 = new Vector3(1, 0, 1);
        d3 = new Vector3(0, 0, 1);

        // 점 3개로 Plane을 만들때의 방향과 동일하다
        Plane p1 = new Plane(d1, d2, d3);
        Plane p2 = new Plane(d1, d3, d2);

        // Plane의 방향과 위의 식을 사용한 결과가 같다.
        Debug.Log("p1 normal : " + p1.normal);
        Debug.Log("p2 normal : " + p2.normal);
        Debug.Log("counterclockwise : " + ccwBy2D(d1, d2, d3));
        Debug.Log("clockwise : " + ccwBy2D(d1, d3, d2));*/

        lr = this.GetComponent<LineRenderer>();
        lr.startWidth = lr.endWidth = .2f;
        lr.material.color = Color.yellow;

        lr.positionCount = 3;

        for(int i = 0; i < planeObject.Length * 3; i++)
        {
            GameObject dot = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            dotOnthePlane.Add(dot);
        }

    }

    // 각 점을 가까운 평면에 mapping 시키고 그 평면에서 시계방향이면 점이 빨간색 아니라면 파란색으로 변경
    void Update()
    {
        Vector3 a, b, c;

        a = clockCheckObject[0].transform.position;
        b = clockCheckObject[1].transform.position;
        c = clockCheckObject[2].transform.position;

        lr.SetPosition(0, a);
        lr.SetPosition(1, b);
        lr.SetPosition(2, c);

        for(int i = 0; i<planeObject.Length; i++)
        {
            Vector3 normal = planeObject[i].transform.up;
            Vector3 planeDot = planeObject[i].transform.position;

            for(int j = 0; j<3; j++)
            {
                Vector3 point = clockCheckObject[j].transform.position;
                dotOnthePlane[i * 3 +j].transform.position = getPositionOnthePlane(normal,planeDot,point);
            }

            Vector3 d1 = dotOnthePlane[i*3 +0].transform.position;
            Vector3 d2 = dotOnthePlane[i*3 +1].transform.position;
            Vector3 d3 = dotOnthePlane[i*3 +2].transform.position;

            if(ccwByPlane(d1,d2,d3,normal,planeDot)>0)
            {
                for(int j = 0; j<3;j++)
                {
                    Renderer rd = dotOnthePlane[i * 3 +j].GetComponent<Renderer>();
                    rd.material = red;
                }
            }
            else
            {
                for (int j = 0; j < 3; j++)
                {
                    Renderer rd = dotOnthePlane[i * 3 + j].GetComponent<Renderer>();
                    rd.material = blue;
                }
            }
        }
    }
}
