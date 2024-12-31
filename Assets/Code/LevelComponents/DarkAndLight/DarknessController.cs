using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarknessController : MonoBehaviour
{
    // New Darkness Stuff
    // Settings
    [SerializeField] private float _darknessKillTime = 0f;


    private SafeZone[] _safeZones = { };
    private Collider2D _playerCollider = null;

    // Player stuff
    private bool _playerInDarkness = false;
    private float _timeSincePlayerEnteredDarkness = 0f;


    private void Start()
    {
        Debug.Log(Player.Instance == null);
        _safeZones = FindObjectsOfType<SafeZone>();

        if (Player.Instance != null)
        {
            _playerCollider = Player.Instance.GetComponent<Collider2D>();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerInDarkness)
        {
            if(_timeSincePlayerEnteredDarkness >= _darknessKillTime)
            {
                Player.Instance.GetKilled(Enums.KillType.Default);
            }
            else
            {
                _timeSincePlayerEnteredDarkness += Time.deltaTime;
            }
        }
    }

    private void FixedUpdate()
    {
        // Check if any part of the player is not outside a light zone
        // If that check is true, start countdown to player deathk

        bool isPlayerOutsideSafeZoneThisFrame = CheckIfPlayerIsOutsideSafeZone();

        if(isPlayerOutsideSafeZoneThisFrame && _playerInDarkness == false)
        {
            _playerInDarkness = true;
        }
        else if(isPlayerOutsideSafeZoneThisFrame == false && _playerInDarkness)
        {
            _playerInDarkness = false;
            _timeSincePlayerEnteredDarkness = 0f;
        }
    }

    private bool CheckIfPlayerIsOutsideSafeZone()
    {
        if(_playerCollider == null)
        {
            _playerCollider = Player.Instance.GetComponent<Collider2D>();
        }

        List<Vector3> playerCorners = new List<Vector3>();
        playerCorners.Add(new Vector3(_playerCollider.bounds.min.x, _playerCollider.bounds.max.y, 0f));
        playerCorners.Add(new Vector3(_playerCollider.bounds.max.x, _playerCollider.bounds.max.y, 0f));
        playerCorners.Add(new Vector3(_playerCollider.bounds.max.x, _playerCollider.bounds.min.y, 0f));
        playerCorners.Add(new Vector3(_playerCollider.bounds.min.x, _playerCollider.bounds.min.y, 0f));

        foreach(Vector3 corner in playerCorners)
        {
            bool cornerInSafeZone = false;

            foreach(SafeZone safeZone in _safeZones)
            {
                cornerInSafeZone |= safeZone.Collider.OverlapPoint(corner);
            }

            if (cornerInSafeZone == false) return true;

            
        }

        Debug.Log("All corners in safeZone");

        return false;
    }

    
}
