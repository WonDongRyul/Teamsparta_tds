using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterHp : MonoBehaviour
{ 
    public int MaxHp = 20;
    public int currentHp;
    public Slider hpSlider;

    private MonsterAI monsterAI;
    // Start is called before the first frame update
    private void Awake()
    {
        currentHp = MaxHp;
        monsterAI = GetComponent<MonsterAI>();
        if (hpSlider == null)
        {
            hpSlider = GetComponentInChildren<Slider>();
        }

        if (hpSlider != null)
        {
            hpSlider.value = 1;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        UpdateHpBar();
    }

    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
            monsterAI.OnDeath();
        }
    }

    private void UpdateHpBar()
    {
        if (hpSlider != null)
        {
            hpSlider.value = (float)currentHp / MaxHp;
        }
    }

    public void ResetHP()
    {
        currentHp = MaxHp;
    }
}
