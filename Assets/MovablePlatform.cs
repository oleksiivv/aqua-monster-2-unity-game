using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovablePlatform : MonoBehaviour
{
    public Vector3 axis;
    private Vector3 startPos;
    public float speed=1;
    int dir=1;

    void Start(){
        if(Random.Range(0,2)==0){
            dir=1;
        }
        else{
            dir=-1;
        }
        startPos=transform.position;

        transform.position=transform.position+axis*dir;

        StartCoroutine(changeDirection());
    }

    void Update(){
        transform.position=Vector3.MoveTowards(transform.position, transform.position-axis*dir, 0.1f*Time.timeScale*speed);
    }



    IEnumerator changeDirection(){
        while(true){
            yield return new WaitForSeconds(1.5f);
            dir*=-1;
        }
    }
}
