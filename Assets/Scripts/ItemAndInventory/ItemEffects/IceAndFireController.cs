using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IceAndFireController : MonoBehaviour
{
    private Rigidbody2D rb;
    private Player player;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Enemy enemy=collision.GetComponent<Enemy>();
        if (enemy!=null)
        {
            enemy.characterStats.TakeDamage((int)(player.characterStats.fireDamage.getValue()+player.characterStats.iceDamage.getValue()));
        }
    }
    public void SetUp(Player _player,float _xVelocity)
    {
        rb = GetComponent<Rigidbody2D>();
        rb.velocity = new Vector2(_xVelocity, 0);
        player =_player;
        transform.right=player.transform.right;
        Destroy(gameObject, 1);
    }
    

}
