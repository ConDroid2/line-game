using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Consts
{
    public static class RoomInfo
    {
        public const int RoomHeight = 110;
        public const int RoomWidth = 200;
    }

    public static class MapRoomInfo
    {
        public const int PreviewRoomHeight = 11;
        public const int PreviewRoomWidth = 20;
        public const int MapRoomConnectionDefaultX = 95;
        public const int MapRoomConnectionDefaultY = 50;

        public static class PointOfInterest
        {
            public const string Key = "POI_Key Variant";
            public const string Lock = "POI_Lock Variant";
            public const string Shoot = "POI_Shoot Variant";
            public const string Grapple = "POI_Grapple Variant";
            public const string Rotate = "POI_Rotate Variant";
            public const string Firewall = "POI_Firewall Variant";
            public const string McGuffin = "POI_McGuffin Variant";
        }
    }

    public static class VolumeSettings
    {
        public const string VolumePref = "Master_Volume";
        public const string MusicPref = "Music_Volume";
        public const string SfxPref = "Sfx_Volume";
    }
}
