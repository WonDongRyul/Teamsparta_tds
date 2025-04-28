using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    public List<Tower> towers = new List<Tower>();

    private void Awake()
    {
        Instance = this;
    }

    
    public Tower GetAvailableTower(MonsterAI monster)
    {
       
        for (int i = towers.Count - 1; i >= 0; i--)
        {
            Tower curront = towers[i];
            if (curront.GetMonsterCount()<8) 
            { 
                return curront; 
            }  
        }
        return null;
    }

    public Tower GetBetterAvailableTower(Tower currentTower)
    {
        int currentIndex = towers.IndexOf(currentTower);
        for (int i = currentIndex-1; i >= 0; i--)
        {
            Tower front = towers[i];
            if(front.GetMonsterCount() <= currentTower.GetMonsterCount())
            {
                return front;
            }
        }
        return null;
    }

    public Tower GetFinalTargetTower(Tower starttower)
    {
        int startIndex = towers.IndexOf(starttower);

        for (int i = startIndex -1; i >= 0; i--)
        {
            Tower front = towers[i];
            if(front.GetMonsterCount() <= starttower.GetMonsterCount())
            {
                starttower = front;
            }
        }
        return starttower;
    }
    
}
