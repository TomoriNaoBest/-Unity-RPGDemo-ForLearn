using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_StatToolTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text_description;
    private void Start()
    {
        gameObject.SetActive(false);
    }
    public void ShowStatToolTip(string _text,Vector2 _slotPosition)
    {
        if (_text!=null)
        {
            text_description.text=_text;
            float xOffset = 40;
            transform.position = new Vector2(_slotPosition.x + xOffset, _slotPosition.y + 450);
            gameObject.SetActive(true);
        }
    }
    public void CloseStatToolTip()
    {
        gameObject.SetActive(false);
    }
}
