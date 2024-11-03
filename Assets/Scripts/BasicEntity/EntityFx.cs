using System.Collections;
using UnityEngine;

public class EntityFx : MonoBehaviour
{
    private SpriteRenderer spriteRenderer;
    private CharacterStats characterStats;
    [Header("Flash FX")]
    [SerializeField] private Material hitMat;
    [SerializeField] private float flashDuration;
    private Material originaMat;

    [Header("Aliments FX")]
    [SerializeField] private Color[] IgnitedColor;
    [SerializeField] private Color ChilledColor;
    [SerializeField] private Color[] ShockedColor;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        originaMat = spriteRenderer.material;
        characterStats=GetComponentInParent<CharacterStats>();
    }
    private IEnumerator FlashFX()
    {
        spriteRenderer.material = hitMat;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.material = originaMat;
    }
    private void RedColorBlink()
    {
        if (spriteRenderer.color != Color.white)
        {
            spriteRenderer.color = Color.white;
        }
        else
        {
            spriteRenderer.color = Color.red;
        }
    }

    private void CancleColorChange()
    {
        CancelInvoke();
        spriteRenderer.color = Color.white;

    }
    public void IgniteFxFor(float _seconds)
    {
        InvokeRepeating("IgniteColorFx",0,characterStats.getIgnitedDamageDuration());
        Invoke("CancleColorChange", _seconds);
    }

    private void IgniteColorFx()
    {
        if (spriteRenderer.color != IgnitedColor[0])
        {
            spriteRenderer.color= IgnitedColor[0];
        }
        else
        {
            spriteRenderer.color = IgnitedColor[1];
        }
    }

    public void ShockedFxFor(float _seconds)
    {
        InvokeRepeating("ShockedColorFx", 0, characterStats.getIgnitedDamageDuration());
        Invoke("CancleColorChange", _seconds);
    }

    private void ShockedColorFx()
    {
        if (spriteRenderer.color != ShockedColor[0])
        {
            spriteRenderer.color = ShockedColor[0];
        }
        else
        {
            spriteRenderer.color = ShockedColor[1];
        }
    }


    public void ChillFxFor(float _seconds)
    {
        spriteRenderer.color = ChilledColor;
        Invoke("CancleColorChange", _seconds);
    }

    public void MakeTrasnParent(bool _isTransParent)
    {
        if (_isTransParent)
        {
            spriteRenderer.color=Color.clear;
        }
        else
        {
            spriteRenderer.color = Color.white;
        }
    }
}
