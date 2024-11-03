
using System.Collections;
using UnityEditor.Build;
using UnityEngine;
public enum StatType
{
    strength,
    agility,
    intelgence,
    vitality,
    damage,
    critChance,
    critDamage,
    maxHp,
    armor,
    evasion,
    magicRes,
    fireDamage,
    iceDamage,
    lightingDamage
}
public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stat strength;//1点力量增加3点攻击力
    public Stat agility;//一点敏捷增加1%闪避
    public Stat intelligence;//一点智力增加3%法抗和3点法伤
    public Stat vitality;// 1点活力增加5点生命上限
    [Header("Offensive stats")]
    public Stat damage;
    public Stat criticalDamage;//暴击伤害 是1.5这种数
    public Stat criticalChance;//暴击率 百分比数0.x
    [Header("Defensive stats")]
    public Stat maxHp;
    public Stat armor;//百分比数 
    public Stat evasion;//回避 闪避 百分比数
    public Stat magicResistance;//百分比魔抗小数
    [Header("Magic stats")]
    public Stat fireDamage;
    public Stat iceDamage;
    public float slowPercent;
    public GameObject lightingPrefab;
    public Stat lightingDamage;
    public float lightingSpeed;
    public bool isIgnited;//V 点燃；(使) 燃烧，着火 逐渐掉血
    public bool isChilled;//冰冻 减防20%外加减速
    public bool isShocked;//减少闪避概率20%
    private float IgnitedDamage;
    private float IgnitedTimer;
    [SerializeField]private float IgnitedDuration;
    private float IgnitedDamageTimer;
    [SerializeField] private float IgnitedDamageDuration;
    private float ChilledTimer;
    [SerializeField] private float ChilledDuration;
     private float ShockedTimer;
    [SerializeField]private float ShockedDuration;


    [SerializeField] private int currentHp;

    public System.Action OnHealthChanged;
    private EntityFx entityFx;
    private Entity entity;
    // Start is called before the first frame update
    protected virtual void Start()
    {
        currentHp = getMaxHpValue();
        criticalDamage.SetBaseValue(1.5f);
        OnHealthChanged += CheckDie;
        entityFx=GetComponentInChildren<EntityFx>();
        entity = GetComponent<Entity>();
    }

    // Update is called once per frame
    protected virtual void Update()
    {
        AlimentsTimerLogic();

    }

    private void CheckDie()
    {
        if (currentHp <= 0)
        {
            Die();
            OnHealthChanged-= CheckDie;
        }
    }

    private void AlimentsTimerLogic()
    {
        if (IgnitedTimer >= 0)
        {
            IgnitedTimer -= Time.deltaTime;
        }
        if (IgnitedTimer < 0 && isIgnited)
        {
            isIgnited = false;
        }
        if (IgnitedDamageTimer >= 0)
        {
            IgnitedDamageTimer -= Time.deltaTime;
        }
        if (IgnitedDamageTimer < 0 && isIgnited)
        {
            TakeDamageBasic((int)IgnitedDamage);
            IgnitedDamageTimer = IgnitedDamageDuration;
        }
        if (ChilledTimer >= 0)
        {
            ChilledTimer -= Time.deltaTime;
        }
        if (ChilledTimer < 0 && isChilled)
        {
            isChilled = false;
        }
        if (ShockedTimer >= 0)
        {
            ShockedTimer -= Time.deltaTime;
        }
        if (ShockedTimer < 0)
        {
            isShocked = false;
        }
    }

    public virtual void DoDamage(CharacterStats _targetStats,bool withMagic)
    {
        if (CanAvoid(_targetStats))
        {
            Debug.Log("Miss!");
            if(_targetStats as PlayerStats)
            {
                if (SkillManager.instance.sk_doge.canClone)
                {
                    SkillManager.instance.sk_clone.CanUseSkill();
                }
            }
            return;
        }
        float totalDamage = getLeastDamage();
        float chillEffect = 0;
        if (isChilled)
        {
            chillEffect = 0.2f;
        }
        totalDamage = totalDamage * (1 - _targetStats.armor.getValue() * (1 - chillEffect));
        if (CanCriticalAttack())
        {
            totalDamage *= criticalDamage.getValue();//暴击在减防后计算
            Debug.Log("Critical Attack!");
        }
        _targetStats.TakeDamage(((int)totalDamage));
        if (withMagic)
        {
            DoMagicDamage(_targetStats);
        }
    }



    public virtual void TakeDamage(int _damage)
    {
        TakeDamageBasic(_damage);
    }
    public void TakeDamageBasic(int _damage)
    {
        currentHp -= _damage;
        if (OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }
    public void IncreaseCurrentHp(int _amount)
    {
        currentHp += _amount;
        if (currentHp > getMaxHpValue())
        {
            currentHp = getMaxHpValue();
        }
        if (OnHealthChanged != null)
        {
            OnHealthChanged();
        }
    }
 
    public virtual void DoMagicDamage(CharacterStats _targetStats)
    {
        float totalMagicDamage=fireDamage.getValue()+iceDamage.getValue()+lightingDamage.getValue()+intelligence.getValue()*3;
        totalMagicDamage*=1-_targetStats.getLeastMagicRes();
        _targetStats.TakeDamage((int)totalMagicDamage);
        
        if (_targetStats.isShocked)
        {
            GameObject lighting = Instantiate(lightingPrefab, _targetStats.GetComponent<Transform>().position, Quaternion.identity);
            thunderForShockController controller = lighting.GetComponent<thunderForShockController>();
            controller.SetUp(_targetStats.GetComponent<Transform>(),(int)lightingDamage.getValue(),lightingSpeed);
        }        
        if (Mathf.Max(fireDamage.getValue(), iceDamage.getValue(), lightingDamage.getValue()) == 0)
        {
            return;
        }
        bool canApplyIgnite=fireDamage.getValue()>iceDamage.getValue()&&fireDamage.getValue()>lightingDamage.getValue();
        bool canApplyChill=iceDamage.getValue()>fireDamage.getValue()&&iceDamage.getValue()>lightingDamage.getValue();
        bool canApplyShock=lightingDamage.getValue()>fireDamage.getValue()&&lightingDamage.getValue()>iceDamage.getValue();
        if (canApplyIgnite)
        {
            CalculateIgnitedDamage(_targetStats);
        }
        if (!canApplyChill && !canApplyIgnite && !canApplyShock)
        {
            int random = Random.Range(1, 4);
            switch (random)
            {
                case 1:
                    canApplyIgnite=true;
                    CalculateIgnitedDamage(_targetStats);
                     break;
                case 2: 
                    canApplyChill=true; break;
                case 3:
                    canApplyShock=true; break;
            }
        }
        _targetStats.ApplyAliments(canApplyIgnite, canApplyChill, canApplyShock);

        void CalculateIgnitedDamage(CharacterStats _targetStats)
        {
            if (fireDamage.getValue() * 0.15f < 1)
            {
                _targetStats.setIgnitedDamage(1);
            }
            else
            {
                _targetStats.setIgnitedDamage(fireDamage.getValue() * 0.15f);
            }
        }
    }
    public void ApplyAliments(bool _ignite,bool _chill,bool _shock) {
        if (isChilled || isIgnited || isShocked)
        {
            return;
        }
        if (_chill)
        {
            isChilled = true;
            ChilledTimer = ChilledDuration;
            entityFx.ChillFxFor(ChilledDuration);
            entity.SlowSpeedBy(slowPercent, ChilledDuration);

        }
       
        if (_ignite)
        {
            isIgnited = true;
            IgnitedTimer = IgnitedDuration;
            entityFx.IgniteFxFor(IgnitedDuration);

        }
        if (_shock)
        {
            isShocked = true;
            ShockedTimer = ShockedDuration;
            entityFx.ShockedFxFor(ShockedDuration);
        }
        
    }
    protected virtual void Die()
    {

    } //空函数
    private bool CanAvoid(CharacterStats _targetStats)
    {
        float totalAvoid = getLeastAvoid(_targetStats);
        int total = (int)(totalAvoid * 100);
        if (Random.Range(1, 101) <= total)
        {
            return true;
        }
        return false;
    }

    private bool CanCriticalAttack()
    {
        float totalChance= criticalChance.getValue();
        int total = (int)(totalChance * 100);
        if (Random.Range(1, 101) <= total)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
    public void setIgnitedDamage(float _damage)
    {
        IgnitedDamage = _damage;
    }
    public int getMaxHpValue()=>(int)(maxHp.getValue() + vitality.getValue() * 5);
    public int getCurrentHp() => currentHp;
    public float getIgnitedDamageDuration()=>IgnitedDamageDuration;

    #region IncreseStatForsometime
    public virtual void ChangeStatFor(float _modifier, float _duration, Stat _targetStat)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _targetStat));
    }
    private IEnumerator StatModCoroutine(float _modifier, float _duration, Stat _targetStat)
    {
        _targetStat.AddModifier(_modifier);
        yield return new WaitForSeconds(_duration);
        _targetStat.RemoveModifier(_modifier);
    }
    #endregion

    public Stat getStat(StatType _statType)
    {
        switch (_statType)
        {
            case StatType.strength:
                return strength;
            case StatType.agility:
                return agility;
            case StatType.intelgence:
                return intelligence;
            case StatType.vitality:
                return vitality;
            case StatType.damage:
                return damage;
            case StatType.critChance:
                return criticalChance;
            case StatType.critDamage:
                return criticalDamage;
            case StatType.maxHp:
                return maxHp;
            case StatType.armor:
                return armor;
            case StatType.evasion:
                return evasion;
            case StatType.fireDamage:
                return fireDamage;
            case StatType.iceDamage:
                return iceDamage;
            case StatType.lightingDamage:
                return lightingDamage;
            case StatType.magicRes:
                return magicResistance;
        }
        return null;
    }
    #region GetValueAfterCalculate
    public float getLeastDamage()
    {
        return damage.getValue() + strength.getValue() * 3;
    }
    public float getLeastAvoid(CharacterStats _targetStats)
    {
        float totalAvoid = _targetStats.evasion.getValue() + _targetStats.agility.getValue() * 0.01f;
        if (isShocked)
        {
            totalAvoid += 0.2f;
        }
        return totalAvoid;
    }
    public float getLeastMagicRes()
    {
        return magicResistance.getValue() + intelligence.getValue() * 0.03f;
    }
    #endregion
}
