using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    private CircleCollider2D _collider;

    private List<Collider2D> _affectedDarknesses = new List<Collider2D>();

    private void Awake()
    {
        _collider = GetComponent<CircleCollider2D>();
        Debug.Log($"Extents: {_collider.bounds.extents}");
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject.GetComponent<Player>() != null)
        {
            foreach(Collider2D collider in _affectedDarknesses)
            {
                collider.GetComponent<KillOnTouch>().Active = true;
            }
            _affectedDarknesses = new List<Collider2D>();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() == null) return;

        foreach(Collider2D collider in _affectedDarknesses)
        {
            collider.GetComponent<KillOnTouch>().Active = true;
        }

        List<Vector3> playerCorners = new List<Vector3>();
        playerCorners.Add(new Vector3(collision.bounds.min.x, collision.bounds.max.y, 0f));
        playerCorners.Add(new Vector3(collision.bounds.max.x, collision.bounds.max.y, 0f));
        playerCorners.Add(new Vector3(collision.bounds.max.x, collision.bounds.min.y, 0f));
        playerCorners.Add(new Vector3(collision.bounds.min.x, collision.bounds.min.y, 0f));

        Physics2D.queriesHitTriggers = true;
        List<Collider2D> hits = new List<Collider2D>(Physics2D.OverlapCircleAll(_collider.bounds.center, _collider.bounds.extents.x));
        Physics2D.queriesHitTriggers = false;

        _affectedDarknesses = new List<Collider2D>();
        List<Collider2D> overlappingSafeZones = new List<Collider2D>();

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Darkness"))
            {
                hit.GetComponent<KillOnTouch>().Active = false;
                _affectedDarknesses.Add(hit);
            }
            else if (hit.CompareTag("SafeZone") && hit.gameObject != gameObject)
            {
                overlappingSafeZones.Add(hit);
            }
        }


        // Check if player should die
        foreach(Collider2D darkness in _affectedDarknesses)
        {
            // If player corner is in darkness and not in this safe zone or any other safe zone

            foreach(Vector3 corner in playerCorners)
            {
                bool cornerInDarkness = darkness.OverlapPoint(corner);
                bool cornerInThisSafeZone = _collider.OverlapPoint(corner);

                bool cornerInAnotherSafeZone = false;

                foreach(Collider2D safeZone in overlappingSafeZones)
                {
                    cornerInAnotherSafeZone |= safeZone.OverlapPoint(corner);
                }

                if(cornerInDarkness == true && cornerInThisSafeZone == false && cornerInAnotherSafeZone == false)
                {
                    Player.Instance.GetKilled(Enums.KillType.Darkness);
                }
            }
        }

        Debug.Log($"Number of overlapping safe zones: {overlappingSafeZones.Count}");
    }
}
