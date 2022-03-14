using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;
    [SerializeField] private float speed = 3f;
    private bool switchPos = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovingBetweenPoints();
        SwitchingPosition();
    }

    void MovingBetweenPoints()
    {
        if (switchPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos2.position, speed * Time.deltaTime);
        }
        else if (!switchPos)
        {
            transform.position = Vector3.MoveTowards(transform.position, pos1.position, speed * Time.deltaTime);
        }
    }

    void SwitchingPosition()
    {
        if(transform.position == pos1.position)
        {
            switchPos = true;
        }
        else if (transform.position == pos2.position)
        {
            switchPos = false;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = this.transform;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            other.transform.parent = null;
        }
    }

}
