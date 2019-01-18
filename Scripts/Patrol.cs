using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour {

    public float movementSpeed = 5f;
    public float rotationSpeed = 90F;

    private bool Patrolling = false;
    private bool RotatingLeft = false;
    private bool RotatingRight = false;
    private bool walking = false;



    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {

        if (Patrolling == false)
        {
            StartCoroutine(Wander());

        }

        if (RotatingRight == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if (RotatingLeft == true)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }
        
        if (walking == true)
        {
            transform.position += transform.forward * movementSpeed * Time.deltaTime;
        }

    }

    IEnumerator Wander()
    {
        int rotateTime = Random.Range(1, 5);
        int rotateWait = Random.Range(1, 4);
        int rotateLeftorRight = Random.Range(1, 5);
        int walkwait = Random.Range(1, 4);
        int walkTime = Random.Range(1, 4);


        Patrolling = true;


        yield return new WaitForSeconds(walkwait);
        walking = true;
        yield return new WaitForSeconds(walkTime);
        walking = false;

      yield return new WaitForSeconds(rotateWait);

        if (rotateLeftorRight == 1)
        {

            RotatingRight = true;
            yield return new WaitForSeconds(rotateTime);
            RotatingRight = false;
        }

        if (rotateLeftorRight == 2)
        {

            RotatingLeft = true;
            yield return new WaitForSeconds(rotateTime);
            RotatingLeft = false;
        }
        Patrolling = false; 
    }


}
