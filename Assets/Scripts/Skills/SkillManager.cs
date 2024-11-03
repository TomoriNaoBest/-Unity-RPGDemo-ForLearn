using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillManager : MonoBehaviour
{
    public static SkillManager instance;
    public Skill_Dash sk_dash {  get; private set; }
    public Skill_Clone sk_clone { get; private set; }
    public Skill_ThrowSword sk_sword { get; private set; }
    public Skill_BlackHole sk_blackhole { get; private set; }
    public Skill_Crystal sk_crystal { get; private set; }
    public Skill_Counter sk_counter { get; private set; }
    public Skill_Doge sk_doge { get; private set; }
    
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        else
        {
            instance = this;
        }
    }
    private void Start()
    {
        sk_dash= GetComponent<Skill_Dash>();
        sk_clone= GetComponent<Skill_Clone>();
        sk_sword= GetComponent<Skill_ThrowSword>();
        sk_blackhole= GetComponent<Skill_BlackHole>();
        sk_crystal= GetComponent<Skill_Crystal>();
        sk_counter= GetComponent<Skill_Counter>();
        sk_doge = GetComponent<Skill_Doge>();
    }
}
