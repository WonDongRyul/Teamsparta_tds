using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MonsterState
{
    Walk,
    Wait,
    StackUp,
    Cycle,
    Attack,
    Die
}
public class MonsterAI : MonoBehaviour
{
    public float movwSpeed = 1f;
    public Transform targetTower;

    private Animator animator;
    private MonsterState state = MonsterState.Walk;
    private int stackIndex = -1;
    private Transform center;
    // Start is called before the first frame update
    private void Start()
    {
        animator = this.GetComponent<Animator>();
        SetState(MonsterState.Walk);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case MonsterState.Walk:
                MoveToTower();
                break;
            case MonsterState.Wait:
                break;
            case MonsterState.StackUp:

                break;
            case MonsterState.Cycle:
                if (center != null)
                    transform.RotateAround(center.position, Vector3.forward, 30f * Time.deltaTime);
                break;
            case MonsterState.Attack:

                break;
            case MonsterState.Die:

                break;
        }
    }
    void MoveToTower()
    {
        Vector3 dir = (targetTower.position - transform.position).normalized;
        transform.position += dir * movwSpeed * Time.deltaTime;

        if(Vector3.Distance(transform.position, targetTower.position)< 0.1f)
        {
            SetState(MonsterState.Wait);
            Tower.Instance.RequestStack(this);
        }
    }

    public void StackTo(Vector3 position, int index)
    {
        Debug.Log("aaaa");
        stackIndex = index;
        //center = rotationCenter;
        SetState(MonsterState.StackUp);
        StartCoroutine(MoveToPosition(position, () =>
        {
            SetState(MonsterState.Cycle);
        }));

    }
    IEnumerator MoveToPosition(Vector3 target, System.Action onCompletr)
    {
        while (Vector3.Distance(transform.position,target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, movwSpeed * Time.deltaTime);
            yield return null;
        }
        onCompletr?.Invoke();
    }
    public void SetState(MonsterState newState)
    {
        state = newState;
        UpdateAnimaator();

    }

    private void UpdateAnimaator()
    {
        if (animator == null) animator = GetComponent<Animator>();
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", false);
        animator.SetBool("IsIdle", false);
        switch (state)
        {
            case MonsterState.Wait:
                break;
            case MonsterState.Cycle:
                animator.SetBool("IsIdle", true);
                break;
            case MonsterState.Attack:
                animator.SetBool("IsAttacking", true);
                break;
            case MonsterState.Die:
                animator.SetBool("IsDead", true);
                break;
            case MonsterState.Walk:
                break;
            case MonsterState.StackUp:
                break;
        }
    }
     public void Die()
    {
        if (state == MonsterState.Die) return;
        SetState(MonsterState.Die);
    }

    public void Awake()
    {
        if (state == MonsterState.Attack) return;
        SetState(MonsterState.Attack);
    }
}
