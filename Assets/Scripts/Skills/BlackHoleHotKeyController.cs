using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BlackHoleHotKeyController : MonoBehaviour
{
    private KeyCode keycode;
    private TextMeshProUGUI textGui;
    private Enemy enemy;
    private BlackHoleController blackHoleController;
    // Start is called before the first frame update
    void Start()
    {  
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keycode)&&!blackHoleController.GetisAttackStarted())
        {
            blackHoleController.AddTargetTransFrom(enemy.transform);
            Destroy(this.gameObject);
        }
    }
    public void SetUp(KeyCode _keycode,Enemy _enemy,BlackHoleController _blackHoleController)
    {
        textGui = GetComponentInChildren<TextMeshProUGUI>();
        keycode = _keycode;
        textGui.text = keycode.ToString();
        enemy= _enemy;
        blackHoleController = _blackHoleController;

    }
}
