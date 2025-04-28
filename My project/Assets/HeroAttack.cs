using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroAttack : MonoBehaviour
{
    public int attackDamage = 5;
    public float attackInterval = 1f;
    public LayerMask monsterLayer;

    private float attackTimer = 0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        attackTimer += Time.deltaTime;

        if (attackTimer >= attackInterval)
        {
            if (IsMonsterVisible())
            {
                AttackNearestMonster();
                attackTimer = 0f;
            }
        }
    }

    private void AttackNearestMonster()
    {
        MonsterHp nearestMonster = FindNearestVisibleMonster();

        if(nearestMonster != null)
        {
            FaceToTarget(nearestMonster.transform);
            nearestMonster.TakeDamage(attackDamage);

        }
    }

    private MonsterHp FindNearestVisibleMonster()
    {
        MonsterHp[] allMonsters = GameObject.FindObjectsOfType<MonsterHp>();
        MonsterHp nearest = null;
        float minDistance = Mathf.Infinity;

        foreach (var monster in allMonsters)
        {
            if (IsVisible(monster.transform))
            {
                float dist = Vector2.Distance(transform.position, monster.transform.position);
                if (dist < minDistance)
                {
                    minDistance = dist;
                    nearest = monster;
                }
            }
        }
        return nearest;
    }

    private bool IsVisible(Transform target)
    {
        Vector3 viewportPos = Camera.main.WorldToViewportPoint(target.position);

        return viewportPos.x >= 0 && viewportPos.x <= 1 && viewportPos.y >= 0 && viewportPos.y <=1 && viewportPos.z > 0;
    }

    private bool IsMonsterVisible()
    {
        MonsterHp[] allMonsters = GameObject.FindObjectsOfType<MonsterHp>();

        foreach (var monster in allMonsters)
        {
            if (IsVisible(monster.transform))
                return true;
        }
        return false;
    }

    private void FaceToTarget(Transform target)
    {
        Vector2 direction = target.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
