using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    //µ¥ÀýÄ£Ê½
    public static PlayerManager instance;
    public Player player;
    
    private void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Player>();
        if(instance != null){
            Destroy(instance.gameObject);
        }
        instance= this;
    }
    
}
