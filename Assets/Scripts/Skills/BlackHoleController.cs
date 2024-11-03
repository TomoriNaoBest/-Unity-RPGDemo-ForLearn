using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class BlackHoleController : MonoBehaviour
{
    [SerializeField] private Transform home;
    [SerializeField] private GameObject hotkeyPrefab;

    private float maxSize;
    private float growSpeed;
    private float shrinkSpeed;
    private int attackCounts;
    private Transform playerTransform;

    [SerializeField]private List<Transform> targetTransFromList=new List<Transform>();
   private List<KeyCode> KeyList;
    private List<Transform> HotKeyTransFormList=new List<Transform>();
    private bool isGrowFinished=false;
    private bool isAttackFinished=false;
    private bool isAttackStarted = false;
    private float CloneDuration;
    private float AttackTimer;
    private int EnemyCount=0;
    private float ExistDuration;
    public void SetUp(Transform _playerTransform,float _maxSize,float _growSpeed,float _shrinkSpeed,int _attackTimes, List<KeyCode> _KeyList,float _ExistDuration)
    {
        playerTransform = _playerTransform;
        maxSize = _maxSize;
        growSpeed = _growSpeed;
        shrinkSpeed = _shrinkSpeed;
        attackCounts = _attackTimes;
        transform.position= _playerTransform.position;
        KeyList=_KeyList;
        ExistDuration=_ExistDuration;
    }

    
    
    // Start is called before the first frame update
    void Start()
    {
        CloneDuration=SkillManager.instance.sk_clone.getCloneDurationInBlackHole();
        AttackTimer = 0;
        home = GameObject.Find("BlackHoleHome").transform;
    }

    // Update is called once per frame
    void Update()
    {
        
        if (!isGrowFinished)
        {
            transform.localScale = Vector2.Lerp(transform.localScale,new Vector2(maxSize,maxSize), growSpeed * Time.deltaTime);
            if (transform.localScale.x >= maxSize-0.1)
            {
                isGrowFinished = true;
            }
        }
        if(isGrowFinished)
        {
            if (ExistDuration >= 0&&!isAttackStarted)
            {
                ExistDuration-=Time.deltaTime;
            }
            if (EnemyCount==0||ExistDuration<0)
            {
                BlackHoleOver();
            }
            if (!isAttackFinished)
            {
                if (Input.GetKeyDown(KeyCodeManager.instance.use_BlackHole)&&!isAttackStarted)
                {
                    if (targetTransFromList.Count <= 0)
                    {
                        BlackHoleOver();
                        return;
                    }
                    isAttackStarted= true;
                    PlayerManager.instance.player.entityFx.MakeTrasnParent(true);
                }
                if (AttackTimer >= 0&&isAttackStarted)
                {
                    AttackTimer-= Time.deltaTime;
                }
                if (AttackTimer < 0)
                {
                    Vector3 offset = new Vector3();
                    int randomNum = Random.Range(1, 101);
                    if (randomNum <= 50)
                    {
                        offset = new Vector3(1, 0, 0);
                    }
                    else
                    {
                        offset = new Vector3(-1, 0, 0);
                    }
                    int i = Random.Range(0,targetTransFromList.Count);
                    SkillManager.instance.sk_clone.UseByblackHole(targetTransFromList[i], true, offset, home);
                    AttackTimer = CloneDuration/10;
                    attackCounts--;
                }
                if (attackCounts <= 0)
                {
                    isAttackFinished = true;
                }
            }
            else
            {
                if (CloneDuration >= 0)
                {
                    CloneDuration -= Time.deltaTime;
                }
                if (CloneDuration < 0)
                {
                    transform.localScale = Vector2.Lerp(transform.localScale, new Vector2(0, 0), shrinkSpeed * Time.deltaTime);
                }
                if (transform.localScale.x < 0.1)
                {
                    BlackHoleOver();
                }
            }

        }

        void BlackHoleOver()
        {
            for (int i = 0; i < HotKeyTransFormList.Count; i++) {
                if (HotKeyTransFormList[i] != null)
                {
                    Destroy(HotKeyTransFormList[i].gameObject);
                }
            }
            PlayerManager.instance.player.ExitBlackHoleState();
            Destroy(this.gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isGrowFinished)
        {
            return;
        }
        if (KeyList.Count <= 0)
        {
            Debug.LogWarning("热键数量不足！");
            return;
        }
        Enemy enemy = collision.GetComponent<Enemy>();
        if (enemy != null && !enemy.nowState.Equals("Die"))
        {
            EnemyCount++;
            enemy.FreezeTime(true);
            GameObject newHotkey=Instantiate(hotkeyPrefab,enemy.transform.position+new Vector3(0,2),Quaternion.identity,home);
            HotKeyTransFormList.Add(newHotkey.transform);
            BlackHoleHotKeyController hotkeyScript=newHotkey.GetComponent<BlackHoleHotKeyController>();
            KeyCode keycode = KeyList[Random.Range(0,KeyList.Count)];
            KeyList.Remove(keycode);
            hotkeyScript.SetUp(keycode,enemy,this);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)=>collision.GetComponent<Enemy>()?.FreezeTime(false);

    public void AddTargetTransFrom(Transform _enemyTransForm)
    {
        targetTransFromList.Add(_enemyTransForm);
    }
    public bool GetisAttackStarted()
    {
        return isAttackStarted;
    }
}
