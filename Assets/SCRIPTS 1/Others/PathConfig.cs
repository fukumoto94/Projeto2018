using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PathConfig : MonoBehaviour
{

    public GameObject pointPrefab;
    Transform mySphere;
    public Color rayColor = Color.white;
    public List<Transform> path_objs = new List<Transform>();
    private Transform[] theArray;
    public int numberOfNodes;
    private int nodesCount = 0;
    private float distNode = 2f;
    private int mainNodes = 0;
    private int temp = 0;




    void Update()
    {
        InstantiatePoints();
    }

    void InstantiatePoints()
    {

        if (nodesCount < ((numberOfNodes - 1) * 2 + numberOfNodes))
        {
            GameObject newPoint = Instantiate(pointPrefab, new Vector3(transform.position.x + distNode, transform.position.y, transform.position.z), Quaternion.identity) as GameObject;
            newPoint.transform.parent = this.gameObject.transform;
            if (mainNodes == nodesCount)
            {
                newPoint.transform.name = "MainNode";
                mainNodes += 3;
            }

            distNode += 2;
            nodesCount++;
        }


    }

    void OnDrawGizmos()
    {
        Gizmos.color = rayColor;
        theArray = GetComponentsInChildren<Transform>();
        path_objs.Clear();

        foreach (Transform path_obj in theArray)
        {
            if (path_obj != this.transform)
            {
                path_objs.Add(path_obj);
            }
        }



        for (int i = 0; i < path_objs.Count; i++)
        {
            Vector3 position = path_objs[i].position;
            if (i > 0)
            {
                Vector3 previous = path_objs[i - 1].position;
                Gizmos.DrawLine(previous, position);
                Gizmos.DrawWireSphere(position, 0.3f);

            }
        }
    }



}
