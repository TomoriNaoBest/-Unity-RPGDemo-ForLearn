using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_SkillToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_SkillName;
    [SerializeField]private TextMeshProUGUI text_SkillDescription;

    public void showSkillToolTip(string _skillName,string _skillDescription,Vector2 _slotPosition)
    {
        text_SkillName.text = _skillName;
        text_SkillDescription.text = _skillDescription;

       
        float xOffset = 0;
        if (_slotPosition.x > Screen.width / 2)
        {
            xOffset = -150;
        }
        else
        {
            xOffset = 150;
        }
        transform.position = new Vector2(_slotPosition.x+xOffset,_slotPosition.y+120);

        gameObject.SetActive(true);
    }
    public void CloseSkillToolTip()
    {
       gameObject.SetActive(false);
    }
}
