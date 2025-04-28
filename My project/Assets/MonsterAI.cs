using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MonsterAI : MonoBehaviour
{
    public float moveSpeed = 1f;
    private Animator animator;
    public Transform spawnPoint;
    private MonsterHp monsterHp;
    private Tower targetTower;
    private bool isMove = false;
    private bool stacked = false;
    // Start is called before the first frame update
    private void Start()
    {
        
        monsterHp = GetComponent<MonsterHp>();
        animator = this.GetComponent<Animator>();
        if (TowerManager.Instance != null)
        {
            Tower initialTower = TowerManager.Instance.GetAvailableTower(this);
            if (initialTower != null )
            {
                targetTower = TowerManager.Instance.GetFinalTargetTower(initialTower);
                targetTower.AddMonster(this);
                stacked = true;
                CheckIfCanMove();
            }
        }
        
    }
    // Update is called once per frame
    private void Update()
    {
        if (isMove && targetTower != null)
            MoveToTarger();
    }
    private void TryMoveToFrontTower()
    {
        Tower frontTower =TowerManager.Instance.GetBetterAvailableTower(targetTower);
        if (frontTower != null && targetTower.CanMonsterMove(this))
        {
            targetTower.RemoveMonster(this);
            targetTower = frontTower;
            targetTower.AddMonster(this);
            stacked = true;
            CheckIfCanMove();
        }

    }
    private void MoveToTarger()
    {
        Vector3 targetPos = targetTower.GetStackPositionCollider(this);
        Vector3 direction = (targetPos - transform.position).normalized;
        transform.position += direction * moveSpeed * Time.deltaTime;

        if (Vector2.Distance(transform.position, targetPos) < 0.1f)
        {
            isMove = false;
            ArriveAtTower();
        }
    }

    private void ArriveAtTower()
    {
        
        transform.position = targetTower.GetStackPositionCollider(this);
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", false);
        TryMoveToFrontTower();
    }
    
    public void RecheckTower()
    {
        if (!isMove && stacked && targetTower !=null)
        {
            TryMoveToFrontTower();
            CheckIfCanMove();
        }
    }
    private void CheckIfCanMove()
    {
        if (targetTower!= null && targetTower.CanMonsterMove(this))
        {
            isMove = true;
            animator.SetBool("IsIdle", false);
            animator.SetBool("IsAttacking", false);
            animator.SetBool("IsDead", false);
        }
    }
    public void OnIdle()
    {
        animator.SetBool("IsIdle", true);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", false);
    }
    public void OnDeath()
    {
        moveSpeed = 0;
        animator.SetBool("IsIdle", false);
        animator.SetBool("IsAttacking", false);
        animator.SetBool("IsDead", true);
        if (targetTower != null)
        {
            targetTower.RemoveMonster(this);
        }
        Invoke(nameof(Respawn), 2f);
    }

    private void Respawn()
    {
        if(targetTower != null)
        {
            targetTower.RemoveMonster(this);
        }

        transform.position = spawnPoint.position;
        moveSpeed = 1;
        if (monsterHp != null)
        {
            monsterHp.ResetHP();
        }
        if (TowerManager.Instance != null)
        {
            targetTower = TowerManager.Instance.GetAvailableTower(this);

            if (targetTower != null)
            {
                targetTower.AddMonster(this);
                stacked = true;
                CheckIfCanMove();
            }
        }
        
        OnIdle();
    }
}
