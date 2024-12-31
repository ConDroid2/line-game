using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public CircleCollider2D Collider;

    public List<Collider2D> AffectedDarknesses = new List<Collider2D>();

    private void Awake()
    {
        Collider = GetComponent<CircleCollider2D>();
    }

    //private void OnTriggerExit2D(Collider2D collision)
    //{
    //    if(collision.gameObject.GetComponent<Player>() != null)
    //    {
    //        foreach(Collider2D darkness in AffectedDarknesses)
    //        {
    //            Physics2D.queriesHitTriggers = true;
    //            List<Collider2D> hits = new List<Collider2D>(Physics2D.OverlapCircleAll(Collider.bounds.center, Collider.bounds.extents.x));
    //            Physics2D.queriesHitTriggers = false;

    //            bool darknessAffectedByOtherSafeZone = false;

    //            foreach(Collider2D hit in hits)
    //            {
    //                if (hit.CompareTag("SafeZone"))
    //                {
    //                    SafeZone otherSafeZone = hit.GetComponent<SafeZone>();

    //                    //Debug.Log(otherSafeZone.AffectedDarknesses.Count);

    //                    if(otherSafeZone.AffectedDarknesses.Contains(darkness) == true)
    //                    {
    //                        darknessAffectedByOtherSafeZone |= true;
    //                    }
    //                }
    //            }    
                
    //            if(!darknessAffectedByOtherSafeZone)
    //            {
    //                darkness.GetComponent<KillOnTouch>().Active = true;
    //            }
    //        }
    //        AffectedDarknesses = new List<Collider2D>();
    //    }
    //}

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.GetComponent<Player>() == null) return;

    //    foreach(Collider2D collider in AffectedDarknesses)
    //    {
    //        collider.GetComponent<KillOnTouch>().Active = true;
    //    }

    //    List<Vector3> playerCorners = new List<Vector3>();
    //    playerCorners.Add(new Vector3(collision.bounds.min.x, collision.bounds.max.y, 0f));
    //    playerCorners.Add(new Vector3(collision.bounds.max.x, collision.bounds.max.y, 0f));
    //    playerCorners.Add(new Vector3(collision.bounds.max.x, collision.bounds.min.y, 0f));
    //    playerCorners.Add(new Vector3(collision.bounds.min.x, collision.bounds.min.y, 0f));

    //    Physics2D.queriesHitTriggers = true;
    //    List<Collider2D> hits = new List<Collider2D>(Physics2D.OverlapCircleAll(Collider.bounds.center, Collider.bounds.extents.x));
    //    Physics2D.queriesHitTriggers = false;

    //    AffectedDarknesses = new List<Collider2D>();
    //    List<Collider2D> overlappingSafeZones = new List<Collider2D>();

    //    foreach (Collider2D hit in hits)
    //    {
    //        if (hit.CompareTag("Darkness"))
    //        {
    //            hit.GetComponent<KillOnTouch>().Active = false;
    //            AffectedDarknesses.Add(hit);
    //            //Debug.Log($"{gameObject.name} is affecting the dark zone");
    //        }
    //        else if (hit.CompareTag("SafeZone") && hit.gameObject != gameObject)
    //        {
    //            overlappingSafeZones.Add(hit);
    //        }
    //    }


    //    // Check if player should die
    //    foreach(Collider2D darkness in AffectedDarknesses)
    //    {
    //        // If player corner is in darkness and not in this safe zone or any other safe zone

    //        foreach(Vector3 corner in playerCorners)
    //        {
    //            bool cornerInDarkness = darkness.OverlapPoint(corner);
    //            bool cornerInThisSafeZone = Collider.OverlapPoint(corner);

    //            bool cornerInAnotherSafeZone = false;

    //            foreach(Collider2D safeZone in overlappingSafeZones)
    //            {
    //                cornerInAnotherSafeZone |= safeZone.OverlapPoint(corner);
    //            }

    //            if(cornerInDarkness == true && cornerInThisSafeZone == false && cornerInAnotherSafeZone == false)
    //            {
    //                Player.Instance.GetKilled(Enums.KillType.Darkness);
    //                //Debug.Log(gameObject.name);
    //            }
    //        }
    //    }

    //    //Debug.Log($"Number of overlapping safe zones: {overlappingSafeZones.Count}");
    //}
}
