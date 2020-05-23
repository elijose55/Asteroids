using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shrek : MonoBehaviour
{
    public GameObject target;
    private Vector3 wayPointPos;

    public float speed = 30.0f;


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        wayPointPos = new Vector3(target.transform.position.x, target.transform.position.y, target.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, wayPointPos, speed * Time.deltaTime);
        transform.LookAt(target.transform);
    }
}
