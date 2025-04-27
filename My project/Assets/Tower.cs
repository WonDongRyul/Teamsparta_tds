using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Start is called before the first frame update
    public float stackoffsetY = 1f;
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
