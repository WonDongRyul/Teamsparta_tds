using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    // Start is called before the first frame update
    public static Tower Instance;

    public Transform stacakBasePoint;
    public float verticlaSpacing = 1f;
    private List<MonsterAI> stackedMonsters = new List<MonsterAI>();

    private void Awake()
    {
        Instance = this;
    }
    public void RequestStack(MonsterAI monster)
    {
        if (stackedMonsters.Count >= 8)
        {
            return;
        }
        int index = stackedMonsters.Count;
        stackedMonsters.Add(monster);
        Vector3 targetPos = stacakBasePoint.position + Vector3.up * verticlaSpacing * index;
        monster.StackTo(targetPos, index);
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
