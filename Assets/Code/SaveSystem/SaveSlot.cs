using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveSlot
{
    public string Name;
    public Dictionary<string, bool> Flags;
    public HashSet<string> RoomsVisited;
    public string CurrentRoomName;
    public RoomPort CurrentPort;
    public string version;
    public WwiseSwitchData TrackPrimary;
    public WwiseSwitchData TrackSecondary;

    public SaveSlot(string name, Dictionary<string, bool> flags, HashSet<string> rooms, string currentRoomName, RoomPort currentPort, WwiseSwitchData trackPrimary, WwiseSwitchData trackSecondary)
    {
        Name = name;
        Flags = flags;
        RoomsVisited = rooms;
        CurrentRoomName = currentRoomName;
        CurrentPort = currentPort;
        TrackPrimary = trackPrimary;
        TrackSecondary = trackSecondary;
    }

    public class VolumeSettings
    {
        public int MainVolume;
        public int MusicVolume;
        public int SoundEffectVolume;
    }

    public class WwiseSwitchData
    {
        public string SwitchGroup;
        public string SwitchState;
    }
}
