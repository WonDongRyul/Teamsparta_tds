using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Start is called before the first frame update
    public float stackoffsetY = 0.3f;
    private List<MonsterAI> monstersOnTower = new List<MonsterAI>();
    public Transform stackStartPoint;
    
    public void AddMonster(MonsterAI monster)
    {
        if (!monstersOnTower.Contains(monster))
        {
            monstersOnTower.Add(monster);
            NotifyMonsters();
        }
    }

    public Vector3 GetStackPostion(MonsterAI monster)
    {
        Vector3 pos = stackStartPoint.position;
        int index = monstersOnTower.IndexOf(monster);
        if (index >= 0)
        {
            pos.y += index * stackoffsetY;
        }
        return pos;
    }

    public int GetMonsterCount()
    {
        return monstersOnTower.Count;
    }

    public void RemoveMonster(MonsterAI monster)
    {
        monstersOnTower.Remove(monster);
        NotifyMonsters();
    }
    public Vector3 GetStackPositionCollider(MonsterAI monster)
    {
        int index = monstersOnTower.IndexOf(monster);
        if (index == 0)
        {
            return stackStartPoint.position;
        }
        else
        {
            MonsterAI topMonster = monstersOnTower[index - 1];

            BoxCollider2D topCollider = topMonster.GetComponent<BoxCollider2D>();
            if (topCollider != null)
            {
                Vector3 topPosition = topCollider.bounds.max;
                return new Vector3(topPosition.x, topPosition.y + stackoffsetY, topMonster.transform.position.z);
            }
            else
            {
                return stackStartPoint.position;
            }
        }
    }
    private void NotifyMonsters()
    {
        if (monstersOnTower.Count > 0)
        {
            monstersOnTower[monstersOnTower.Count - 1].RecheckTower();
        }
    }
    public bool CanMonsterMove(MonsterAI monster)
    {
        return monstersOnTower.Count > 0 && monstersOnTower[monstersOnTower.Count - 1] == monster;
    }
    
}
