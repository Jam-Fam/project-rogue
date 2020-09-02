using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace ProjectRogue.Maps
{
    [CreateAssetMenu(fileName = "MapGenerationSettings", menuName = "ScriptableObjects/MapGenerationSettings", order = 1)]
    public class MapGenerationSettings : ScriptableObject
    {
        public int RoomWidth, RoomHeight;
        public int NumRoomsX, NumRoomsY;
        public int WallWidth, FloorHeight;
        public string TemplatesRootDir;
    }
}
