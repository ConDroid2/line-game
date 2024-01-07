using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enums : MonoBehaviour
{
    /**
     * Different types of slopes
     * NOTE: Ascending refers to slopes with either both positive x,y or both negative x,y
     * NOTE: Descending refers to slopes with either x or y being positive, and the other being negative
     */
    public enum SlopeType { Horizontal, Vertical, Ascending, Descending, None};

    public enum LineType { Static, Shifting, Rotating };

    public enum LinePoints { A, B, Default };

    public enum RoomSides { Left, Top, Right, Bottom, Default };

    public enum ObjectType { Player, Pushable, BlockObstacle, UniversalObstacle, Default}
}
