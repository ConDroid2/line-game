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

    public SaveSlot(string name, Dictionary<string, bool> flags, HashSet<string> rooms, string currentRoomName, RoomPort currentPort)
    {
        Name = name;
        Flags = flags;
        RoomsVisited = rooms;
        CurrentRoomName = currentRoomName;
        CurrentPort = currentPort;
    }

    public class VolumeSettings
    {
        public int MainVolume;
        public int MusicVolume;
        public int SoundEffectVolume;
    }
}
