using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    public int towerIndex;
    private List<MonsterAI> stackeMonsters = new List<MonsterAI>();
    private float stackGep = 1f;
    private const int ABSOLUTE_MAX = 8;
    
    public void RegisterMonster(MonsterAI monster)
    {
        if (!CanAccpeptMoreMonsters())
        {
            Debug.Log("ÃÒ°ú");
            return;
        }
        foreach(Tower tower in TowerManager.Instance.towers)
            tower.RemoveMonster(monster);

        stackeMonsters.Add(monster);
        monster.SetStackIndex(stackeMonsters.IndexOf(monster));
        monster.UpdatePosition(transform.position);

        ReceiveMonsterCountFromBehind(GatAliveMonsterCount());
        
    }

    public void RemoveMonster(MonsterAI monster)
    {
        if (stackeMonsters.Contains(monster))
        {
            stackeMonsters.Remove(monster);
            for (int i = 0; i < stackeMonsters.Count; i++)
            {
                stackeMonsters[i].SetStackIndex(i);
                stackeMonsters[i].UpdatePosition(transform.position);
            }
        }
    }

    public void ReceiveMonsterCountFromBehind(int behindCount)
    {
        TrySendTopMonsterToFront();
    }

    private void TrySendTopMonsterToFront()
    {
        if (stackeMonsters.Count == 0)
        {
            return;
        }
        for (int i = stackeMonsters.Count-1;i>=0; i--)
        {
            MonsterAI candidate = stackeMonsters[i];
            if (candidate != null && !candidate.IsDead && !candidate.IsJustArrived())
            {
                Tower frontTower = TowerManager.Instance.GetTowerByIndex(towerIndex - 1);
                if (frontTower == null || !frontTower.CanAccpeptMoreMonsters())
                {
                    return;
                }
                Vector3 targetPos = frontTower.GetStackPositionByIndex(frontTower.GatAliveMonsterCount());
                candidate.ReceiveMoveCommand(targetPos, frontTower, frontTower.GatAliveMonsterCount());
                return;
            }
        }
        
    }
    
    public int GatAliveMonsterCount()
    {
        return stackeMonsters.Count;
    }

    public Vector3 GetStackPositionByIndex(int index)
    {
        return transform.position + Vector3.up * (index* stackGep);
    }

    public bool CanAccpeptMoreMonsters()
    {
        if (TowerManager.Instance.IsHisghestTower(this))
        {
            return true;
        }
        Tower behindTower = TowerManager.Instance.GetTowerByIndex(towerIndex + 1);
        int allowedCount = ABSOLUTE_MAX;

        if(behindTower != null)
        {
            allowedCount = Mathf.Min(behindTower.GatAliveMonsterCount() + 1, ABSOLUTE_MAX);
        }
        else
            allowedCount = 0;
        return GatAliveMonsterCount() < allowedCount;
    }
}
