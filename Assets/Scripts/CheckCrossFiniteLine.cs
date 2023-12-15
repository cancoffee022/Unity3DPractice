using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckCrossFiniteLine : MonoBehaviour
{
    public GameObject go1, go2, go3, go4, cube;
    public LineRenderer lr1, lr2;

    bool CheckDotInLine(Vector3 a, Vector3 b, Vector3 dot)
    {
        float epsilon = 0.0001f;
        float dAB = Vector3.Distance(a, b);
        float dADot = Vector3.Distance(a, dot);
        float dBDot = Vector3.Distance(b, dot);

        return ((dAB + epsilon) >= dADot + dBDot);
    }

    // 교차하는지 검사하는 함수 (벡터의 외적- 값이 0이라면 두 벡터는 평행함)
    bool CrossCheck2D(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        // (x, 0, z)
        float x1, x2, x3, x4, z1, z2, z3, z4, X, Z;

        x1 = a.x; z1 = a.z;
        x2 = b.x; z2 = b.z;
        x3 = c.x; z3 = c.z;
        x4 = d.x; z4 = d.z;

        float cross = ((x1 - x2) * (z3 - z4) - (z1 - z2) * (x3 - x4));

        if (cross == 0 /* parallel */) return false;
        X = ((x1 * z2 - z1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * z4 - z3 * x4)) / cross;
        Z = ((x1 * z2 - z1 * x2) * (z3 - z4) - (z1 - z2) * (x3 * z4 - z3 * x4)) / cross;

        return
            CheckDotInLine(a, b, new Vector3(X, 0, Z))
            && CheckDotInLine(c, d, new Vector3(X, 0, Z));
    }
    Vector3 CrossCheck2DVector(Vector3 a, Vector3 b, Vector3 c, Vector3 d)
    {
        // (x, 0, z)
        float x1, x2, x3, x4, z1, z2, z3, z4, X, Z;

        x1= a.x; z1 = a.z;
        x2= b.x; z2 = b.z;
        x3= c.x; z3 = c.z;
        x4= d.x; z4 = d.z;

        // 벡터의 외적식과 동일
        float cross = ((x1 - x2) * (z3 - z4) - (z1 - z2) * (x3 - x4));

        //평행일때
        if (cross == 0) return new Vector3(10000,10000,10000);

        X = ((x1 * z2 - z1 * x2) * (x3 - x4) - (x1 - x2) * (x3 * z4 - z3 * x4)) / cross;
        Z = ((x1 * z2 - z1 * x2) * (z3 - z4) - (z1 - z2) * (x3 * z4 - z3 * x4)) / cross;

        return new Vector3(X, 0, Z);
    }

    

    private void Start()
    {
        lr1.startWidth = lr1.endWidth = .1f;
        lr1.material.color = Color.blue;
        lr2.startWidth = lr2.endWidth = .1f;
        lr2.material.color = Color.red;

        lr1.positionCount = lr2.positionCount = 2;
    }

    private void Update()
    {
        Vector3 pos1, pos2, pos3, pos4;

        pos1 = go1.transform.position;
        pos2 = go2.transform.position;
        pos3 = go3.transform.position;
        pos4 = go4.transform.position;

        lr1.SetPosition(0, pos1);
        lr1.SetPosition(1, pos2);

        lr2.SetPosition(0, pos3);
        lr2.SetPosition(1, pos4);

        if (CrossCheck2D(pos1, pos2, pos3, pos4))
        {
            cube.transform.position = CrossCheck2DVector(pos1, pos2, pos3, pos4);
        }
        else
        {
            cube.transform.position = new Vector3(0, 10000, 0);
        }
    }
}
