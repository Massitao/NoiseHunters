using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimations : MonoBehaviour
{
    private Animator myAnimator;
    private NavMeshAgent myAgent;

    private Vector3 previousPosition;
    private float currentSpeed;


    // Start is called before the first frame update
    void Start()
    {
        myAnimator = GetComponent<Animator>();
        myAgent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update()
    {
        GetCurrentSpeed();
        myAnimator.SetFloat("CurrentSpeed", currentSpeed);
    }

    public void GetCurrentSpeed()
    {
        Vector3 currentMove = transform.position - previousPosition;
        currentSpeed = currentMove.magnitude / Time.deltaTime;
        previousPosition = transform.position;
    }
}
