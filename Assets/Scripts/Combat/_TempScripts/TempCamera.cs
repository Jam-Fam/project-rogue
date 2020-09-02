using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCamera : MonoBehaviour
{
    [SerializeField]
    private Transform target;
    [SerializeField]
    private float drag = 3;
    // Start is called before the first frame update
    [SerializeField]
    private float minDis = 3;

    private float followCounter;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Vector3.Distance(target.position,transform.position) > minDis)
        {
            followCounter = 1;
        } 
        if(followCounter > 0)
        {
            followCounter -= Time.deltaTime;
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * drag);
        }
    }
}
