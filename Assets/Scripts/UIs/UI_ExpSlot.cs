using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_ExpSlot : MonoBehaviour
{
    private TextMeshProUGUI text_exp;
    // Start is called before the first frame update
    void Start()
    {
        text_exp=GetComponentInChildren<TextMeshProUGUI>();
        UpdateExpSlot();
    }

    public void UpdateExpSlot()
    {
        text_exp.text = (PlayerManager.instance.player.characterStats as PlayerStats).GetExp().ToString();
    }
}
