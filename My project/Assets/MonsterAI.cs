using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;


public class MonsterAI : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Animator animator;
    public Transform spawnPoint;
    private MonsterHp monsterHp;
    public Tower currentTower;
    private Tower nextTower;
    private bool isDead = false;
    private bool isMove = false;
    private Vector3 moveTarger;
    private float arrivalTime;
    private const float sefArrivalBuffer = 0.1f;
    private Coroutine moveCoroutine;
    public int stackIndex {  get; private set; }
    // Start is called before the first frame update
    private void Start()
    {
        
        monsterHp = GetComponent<MonsterHp>();
        animator = this.GetComponent<Animator>();
        TryStartInitialMove();
    }
    
    private void TryStartInitialMove()
    {
        Tower startTower = TowerManager.Instance.GetHighestIndexTower();
        if (startTower != null)
        {
            Vector3 targetPos = startTower.GetStackPositionByIndex(startTower.GatAliveMonsterCount());
            ReceiveMoveCommand(targetPos, startTower, startTower.GatAliveMonsterCount());
        }
    }

    public void ReceiveMoveCommand(Vector3 targetPos, Tower targetTower, int index)
    {
        if (isMove)
        {
            return;
        }
        moveTarger = targetPos;
        nextTower = targetTower;
        stackIndex = index;
        isMove = true;

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    private IEnumerator MoveCoroutine()
    {
        while(Vector3.Distance(transform.position, moveTarger) > 0.05f)
        {
            transform.position = Vector3.MoveTowards(transform.position, moveTarger, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = moveTarger;
        isMove = false;
        ReportArrival();
    }

    private void ReportArrival()
    {
        if (nextTower != null)
        {
            if (nextTower.CanAccpeptMoreMonsters())
            {
                currentTower = nextTower;
                nextTower = null;
                arrivalTime = Time.time;
                currentTower.RegisterMonster(this);

                Invoke(nameof(RequestRecheckMove), 0.2f);
            }
            else
                nextTower = null;
        }
    }
    private void RequestRecheckMove()
    {
        if (currentTower != null)
        {
            currentTower.ReceiveMonsterCountFromBehind(currentTower.GatAliveMonsterCount());
        }
    }
    public void ReportDeath()
    {
        if (currentTower != null)
        {
            currentTower.RemoveMonster(this);
            currentTower = null;
        }
    }
    public void OnDeath()
    {
        if (isDead) return;
        if (!IsJustArrived())
        {
            moveSpeed = 0;
            isDead = true;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsDead", true);
            ReportDeath();
            Invoke(nameof(Respawn), 2f);
        }
    }

    private void Respawn()
    {
        isDead = false;
        moveSpeed = 1;
        animator.SetBool("IsDead", false);
        animator.SetBool("IsIdle", true);
        if (monsterHp != null)
        {
            monsterHp.ResetHP();
        }
        transform.position = spawnPoint.position;
        Tower startTower = TowerManager.Instance.GetHighestIndexTower();
        if (startTower != null)
        {
            Vector3 targetPos = startTower.GetStackPositionByIndex(startTower.GatAliveMonsterCount());
            ReceiveMoveCommand(targetPos, startTower, startTower.GatAliveMonsterCount());
        }

    }

    public void SetStackIndex(int index)
    {
        stackIndex = index;
    }

    public void UpdatePosition(Vector3 towerPosition)
    {
        /*if (TowerManager.Instance.towers.Count == nextTower.towerIndex)
        {
            stackIndex = 0;
        }*/
        float yOffset = stackIndex * 1f;
        Vector3 targetPos = new Vector3(towerPosition.x, towerPosition.y + yOffset, towerPosition.z);

        moveTarger = targetPos;
        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }
        moveCoroutine = StartCoroutine(MoveCoroutine());
    }

    public bool IsDead { get { return isDead; } }
    public bool IsJustArrived()
    {
        return Time.time - arrivalTime < sefArrivalBuffer;
    }
}
