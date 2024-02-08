using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillOnTouch : MonoBehaviour
{
    public Enums.KillType killType = Enums.KillType.Default;
    public bool Active;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.gameObject.TryGetComponent(out Player player))
        {    
            if (Active)
            {
                player.GetKilled(killType);
            }
        }
    }
}
