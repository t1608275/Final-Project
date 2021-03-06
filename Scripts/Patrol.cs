﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{

    [SerializeField] private Animator _animator;
    public float movementSpeed = 5f;
    public float rotationSpeed = 90F;

    private bool Patrolling = false;
    private bool RotatingLeft = false;
    private bool RotatingRight = false;
    private bool walking = false;
    private bool _isChatOpen;
    private static readonly int WalkAnimHash = Animator.StringToHash("Forward");


    // Use this for initialization
    void Start() {
        gamemanager.Instance.OpenedChat+=OnOpenedChat;
        gamemanager.Instance.ClosedChat+=OnClosedChat;
    }

    private void OnClosedChat()
    {
        if (!_isChatOpen) return;
        _isChatOpen = false;
        _animator.SetFloat(WalkAnimHash, 0.5f);
    }

    private void OnOpenedChat()
    {
        _isChatOpen = true;
        _animator.SetFloat(WalkAnimHash,0.0f);
    }

    // Update is called once per frame
    void Update() {
        
        if(_isChatOpen)
            return;
        
        if (Patrolling == false)
        {
            StartCoroutine(Wander());

        }
        if (RotatingRight)
        {
            transform.Rotate(transform.up * Time.deltaTime * rotationSpeed);
        }
        if (RotatingLeft)
        {
            transform.Rotate(transform.up * Time.deltaTime * -rotationSpeed);
        }
        
        if (walking)
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
