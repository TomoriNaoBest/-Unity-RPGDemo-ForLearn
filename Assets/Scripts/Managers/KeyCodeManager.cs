using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCodeManager : MonoBehaviour
{
    public static KeyCodeManager instance;
    public KeyCode use_crystal;
    public KeyCode use_BlackHole;
    public KeyCode use_counter;
    public KeyCode use_Flask;
    public KeyCode open_characterUI;
    private void Awake()
    {
        if (instance != null)
        {
            Destroy(instance.gameObject);
        }
        instance = this;
    }
}
