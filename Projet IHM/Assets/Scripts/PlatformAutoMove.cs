using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformAutoMove : MonoBehaviour
{

    [SerializeField]
    public Transform startPoint;
    [SerializeField]
    public Transform endPoint;
    [SerializeField]
    private float moveTime = 2f;
    [SerializeField]
    private float moveSpeed = 1f;
    [SerializeField]
    private bool isArrive = false; // is Arrive at start point or end point  or not

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PlatformMove();
    }

    void PlatformMove()
    {
        if(!isArrive)
        {
            
            transform.position = Vector3.MoveTowards(transform.position, endPoint.position, moveSpeed * Time.deltaTime);
            if (transform.position == endPoint.position)
                isArrive = true;
        }
        else
        {
            transform.position = Vector3.MoveTowards(transform.position, startPoint.position, moveSpeed * Time.deltaTime);
            if (transform.position == startPoint.position)
                isArrive = false;
        }
    }
}
