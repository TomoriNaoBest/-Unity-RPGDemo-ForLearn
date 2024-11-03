using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Switch : MonoBehaviour
{
    public UI_ItemToolTip itemToolTip;
    public UI_StatToolTip statToolTip;
    public UI_SkillToolTip skillToolTip;
    public UI_CraftWindow ui_CraftWindow;
    public UI_ExpSlot expslot;
    [SerializeField]private GameObject ui_character;
    [SerializeField]private GameObject ui_skilltree;
    [SerializeField]private GameObject ui_craft;
    [SerializeField] private GameObject ui_option;
    
    // Start is called before the first frame update
    void Start()
    {
        SwitchUI(null);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCodeManager.instance.open_characterUI))
        {
            SwitchUIwithKey(ui_character);
        }
    }
    public void SwitchUI(GameObject _menu)
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            transform.GetChild(i).gameObject.SetActive(false);
        }
        if(_menu != null)
        {
            _menu.SetActive(true);
        }
    }
    public void SwitchUIwithKey(GameObject _menu)
    {
        if(_menu != null && _menu.activeSelf)
        {
            itemToolTip.gameObject.SetActive(false);
            statToolTip.gameObject.SetActive(false);
            _menu.SetActive(false);
            return;
        }
        SwitchUI(_menu);
    }

}
