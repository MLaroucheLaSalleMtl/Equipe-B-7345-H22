using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatforms : MonoBehaviour
{
    [SerializeField] private Transform pos1;
    [SerializeField] private Transform pos2;
    [SerializeField] private float speed;
    private bool switchPos = false;

    //private PlayerController player;
    //private Rigidbody rb;
    private Vector3 vecPos1;
    private Vector3 vecPos2;
    //private Vector3 velocity;
    private Vector3 refVel = Vector3.zero;
    private float smoothTime = 0.5f;
    private float offset = 0.1f;
    
    // Start is called before the first frame update
    void Start()
    {
        //player = GetComponent<PlayerController>();
        //rb = GetComponent<Rigidbody>();
        vecPos1 = pos1.position;
        vecPos2 = pos2.position;
        //velocity = rb.velocity;
    }

    private void Update()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        MovingBetweenPoints();
        SwitchingPosition();
        //transform.position = Vector3.Lerp(transform.position, pos2.position, smooth * Time.deltaTime);
    }

    void MovingBetweenPoints()
    {
        if (switchPos)
        {
            //SmoothChangeDirection(pos2);
            //transform.position = Vector3.MoveTowards(transform.position, pos2.position, speed * Time.deltaTime);
            transform.position = Vector3.SmoothDamp(transform.position, vecPos2, ref refVel, smoothTime, speed);
            //newPos = pos2.position;
            //StartCoroutine(SmoothChangeDirection(pos2.position));
            //transform.position = Vector3.SmoothDamp(transform.position, pos2.position, ref currentVelocity, 0.1f, speed * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, pos2.position, smooth * Time.deltaTime);
            //newPos = transform.position;
        }
        else if (!switchPos)
        {
            //SmoothChangeDirection(pos1);
            transform.position = Vector3.SmoothDamp(transform.position, vecPos1, ref refVel, smoothTime, speed);

            //transform.position = Vector3.MoveTowards(transform.position, pos1.position, speed * Time.deltaTime);
            //newPos = pos1.position;
            //StartCoroutine(SmoothChangeDirection(pos1.position));
            //transform.position = Vector3.SmoothDamp(transform.position, pos1.position, ref currentVelocity, 0.1f, speed * Time.deltaTime);
            //transform.position = Vector3.Lerp(transform.position, pos1.position, smooth * Time.deltaTime);
            //newPos = transform.position;
        }
        //Invoke("MovingBetweenPoints", resetTime);
    }

    //IEnumerator SmoothChangeDirection(Transform target)
    //{
    //    while (transform.position != target.position)
    //    {
    //        transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime * speed);
    //        yield return new WaitForEndOfFrame();
    //    }
    //}

    //float SmoothChangeDirection()
    //{
    //    currentLerpTime += Time.deltaTime;
    //    if(currentLerpTime >= lerpTime)
    //    {
    //        currentLerpTime = lerpTime;
    //    }
    //    float perc = currentLerpTime / lerpTime;
    //    return perc;
    //}

    void SwitchingPosition()
    {
        if((Vector3.Distance(transform.position, vecPos1) < offset))
        {
            switchPos = true;
        } 
        else if ((Vector3.Distance(transform.position, vecPos2) < offset))
        {
            switchPos = false;
        }
        //if(transform.position == vecPos1)
        //{
        //    switchPos = true;
        //}
        //else if (transform.position == vecPos2)
        //{
        //    switchPos = false;
        //}
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
            //player.gameObject.GetComponent<Rigidbody>().velocity += rb.velocity;
        }
    }

}
