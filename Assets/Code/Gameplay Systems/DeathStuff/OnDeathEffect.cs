using Steamworks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class OnDeathEffect : MonoBehaviour
{
    public int Priority = 0;

    public virtual void TriggerEffect()
    {

    }

    public virtual bool IsPlaying()
    {
        return false;
    }
}
