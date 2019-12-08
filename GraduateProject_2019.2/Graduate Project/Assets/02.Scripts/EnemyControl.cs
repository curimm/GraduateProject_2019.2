using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyControl : MonoBehaviour
{
    [SerializeField]
    private GameObject target;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateAnimation();


    }

    void UpdateAnimation()
    {
        if (null == target || false == agent.SetDestination(target.transform.position))
        {
            animator.SetBool("isMoving", false);
            return;
        }

        float distance = Vector3.Distance(target.transform.position, transform.position);
        if (distance <= 5.0f) // 공격 범위 안
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", true);
        }
        else // 공격 범위 바깥
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("positionY", 1.0f);
            animator.SetBool("isAttacking", false);
        }

    }

}
