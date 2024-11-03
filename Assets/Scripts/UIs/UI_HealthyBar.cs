using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_HealthyBar : MonoBehaviour
{
    private Entity entity;
    private RectTransform rectTransform;
    private CharacterStats characterStats;
    private Slider healthSlider;
    // Start is called before the first frame update
    void Start()
    {
        entity=GetComponentInParent<Entity>();
        rectTransform=GetComponent<RectTransform>();
        characterStats=GetComponentInParent<CharacterStats>();
        healthSlider=GetComponentInChildren<Slider>();
        entity.OnFlipped+=FlipUI;
        characterStats.OnHealthChanged += FreshHpbar;
        FreshHpbar();
    }

    // Update is called once per frame
    void Update()
    {
       
    }
    private void FlipUI()
    {
        //if (characterStats as PlayerStats)
        //{
        //    Debug.Log("玩家血条翻转");
        //}
        rectTransform.Rotate(0, 180, 0);
    }
        
       
    private void FreshHpbar()
    {
        //if(characterStats as PlayerStats)
        //{
        //    Debug.Log("玩家血条刷新");
        //}
        healthSlider.maxValue=characterStats.getMaxHpValue();
        healthSlider.value = characterStats.getCurrentHp();
    }
    public void doDestroy()
    {
        Destroy(gameObject);
    }
}
