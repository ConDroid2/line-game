using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointOfInterest : MonoBehaviour
{
    public enum POIType { Lock, Key, Shoot, Grapple, Rotate, Firewall, McGuffin }

    public POIType Type;

    public string FlagName;
}
