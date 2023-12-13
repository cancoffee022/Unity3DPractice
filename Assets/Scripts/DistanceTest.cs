using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DistanceTest : MonoBehaviour
{
    public GameObject plane;
    public GameObject sphere;
    // Start is called before the first frame update

    float distancePlaneToPoint(Vector3 normal, Vector3 planeDot, Vector3 point)
    {
        Plane plane = new Plane(normal, planeDot);
        return plane.GetDistanceToPoint(point);
    }
    void Start()
    {
        Debug.Log(
            distancePlaneToPoint(plane.transform.up, plane.transform.position, sphere.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawLine(plane.transform.position, sphere.transform.position,Color.red);
    }
}
