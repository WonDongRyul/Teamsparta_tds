using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TowerManager : MonoBehaviour
{
    public static TowerManager Instance;
    public List<Tower> towers = new List<Tower>();

    private void Awake()
    {
        Instance = this;
        for (int i = 0; i < towers.Count; i++)
        {
            towers[i].towerIndex = i;
        }
    }

    public Tower GetHighestIndexTower()
    {
        if(towers.Count == 0)
        {
            return null;
        }

        return towers.OrderByDescending(x => x.towerIndex).First();
    }
    
    public Tower GetTowerByIndex(int index)
    {
        if (index < 0 || index >= towers.Count)
            return null;
        return towers[index];
    }
    
    public bool IsHisghestTower(Tower tower)
    {
        return tower.towerIndex == towers.Max(x => x.towerIndex);
    }
}
