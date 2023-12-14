using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClockWiseForPlane : MonoBehaviour
{
    // ��� �󿡼� �ð� �������� �ݽð� �������� �Ǵ��ϴ� ��

    public Material red, blue;
    public GameObject[] planeObject;
    public GameObject[] clockCheckObject;
    LineRenderer lr;
    List<GameObject> dotOnthePlane = new List<GameObject>();
    
    float ccwBy2D(Vector3 a, Vector3 b, Vector3 c) // ������ �ٲ��� �ʰ� 3�� ��� y���� 0�̿����Ѵ�
    {
        Vector3 p = b - a;
        Vector3 q = c - b;

        return Vector3.Cross(p, q).y;
    }
    
    // 3D ȯ�濡���� 3���� �̾��� ������ �ٲ��� �ʾƵ�, ������ ���� �ð����, �ݽð������ �ٲ� �� �ִ�

    float distancePlaneToPoint(Vector3 normal, Vector3 planeDot, Vector3 point)
    {
        Plane plane = new Plane(normal, planeDot);
        return plane.GetDistanceToPoint(point);
    }// ���� �� ������ �ִܰŸ� ���ϴ� ��� (������ ���� ���ϴ� ���)

    Vector3 getPositionOnthePlane(Vector3 normal, Vector3 planeDot, Vector3 position)
    {
        float distance = distancePlaneToPoint(normal, planeDot, position);
        return position - normal * distance;
    }// ���� �ִܰŸ��� ���� ���ȿ��� ���� ���� ����� ��ĥ�� ����

    // ���� �Լ��� ���� �ش��ϴ� ��鿡 ���� ���� �Ǵ��� �� �� �ִ� �Լ��� �����
    float ccwByPlane(Vector3 a, Vector3 b, Vector3 c, Vector3 normal, Vector3 planeDot)
    {
        Vector3 a_OnthePlane = getPositionOnthePlane(normal, planeDot, a);
        Vector3 b_OnthePlane = getPositionOnthePlane(normal, planeDot, b);
        Vector3 c_OnthePlane = getPositionOnthePlane(normal, planeDot, c);

        Vector3 p = b_OnthePlane - a_OnthePlane;
        Vector3 q = c_OnthePlane - b_OnthePlane;
        // �� ������ ��鿡 �����Ǵ� ������ ���ϰ� �� ������ �ٸ� �����ΰ��� ���͸� ���Ѵ�

        return Vector3.Dot(Vector3.Cross(p, q), normal);
        // ���͵��� �����ѵڿ� ����� normal���Ϳ� ������ �Ͽ� ������ �ð����, ������� �ݽð������ ���´�
    }

    void Start()
    {
        /*Vector3 d1, d2, d3;

        d1 = new Vector3(0, 0, 0);
        d2 = new Vector3(1, 0, 1);
        d3 = new Vector3(0, 0, 1);

        // �� 3���� Plane�� ���鶧�� ����� �����ϴ�
        Plane p1 = new Plane(d1, d2, d3);
        Plane p2 = new Plane(d1, d3, d2);

        // Plane�� ����� ���� ���� ����� ����� ����.
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

    // �� ���� ����� ��鿡 mapping ��Ű�� �� ��鿡�� �ð�����̸� ���� ������ �ƴ϶�� �Ķ������� ����
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
