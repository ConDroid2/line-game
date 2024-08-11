using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimController : MonoBehaviour
{
    [SerializeField] private float _aimingSpeed = 4f;
    [SerializeField] private List<AbilityComponentMapping> _abilityMappings;
    private bool _active = false;

    public void SetAim(Vector2 aimVector)
    {
        if(_active == false)
        {
            return;
        }

        // This is sort of the effect we want but it behaves a little strangely, maybe just use a straight up Lerp or something
        // Vector2 newUp = Vector2.MoveTowards(transform.up, aimVector, _aimingSpeed * Time.deltaTime);

       //  Vector2 newUp = Vector3.Slerp(transform.up, aimVector, _aimingSpeed * Time.deltaTime);
        Vector2 newUp = aimVector;

        // Debug.Log($"Current up: {transform.up} \n Current goal: {aimVector} \n Next frame up: {newUp}");

        transform.up = newUp;
    }

    public void Activate(Player.AbilityEnum abilityUsed)
    {
        foreach(AbilityComponentMapping mapping in _abilityMappings)
        {
            mapping.component.enabled = false;
            if(mapping.ability == abilityUsed)
            {
                mapping.component.enabled = true;
            }
        }
        _active = true;
        gameObject.SetActive(true);
    }

    public void Deactivate()
    {
        foreach(AbilityComponentMapping mapping in _abilityMappings)
        {
            mapping.component.enabled = false;
        }
        _active = false;
        gameObject.SetActive(false);
    }

    [System.Serializable]
    public struct AbilityComponentMapping
    {
        public Player.AbilityEnum ability;
        public MonoBehaviour component;
    }
}
