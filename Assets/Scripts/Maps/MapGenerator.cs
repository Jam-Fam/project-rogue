
using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

namespace ProjectRogue.Maps
{
    public class MapGenerator : MonoBehaviour
    {
        // Settings
        private MapGenerationSettings mapGenSettings;

        // State
        private RoomData[,] rooms;
        private List<RoomTemplate> roomTemplatesClosedBottom, roomTemplatesOpenBottom;

        public RoomData[,] GenerateMap(MapGenerationSettings mapGenSettings)
        {
            this.mapGenSettings = mapGenSettings;

            loadRoomTemplates();

            rooms = rooms = new RoomData[mapGenSettings.NumRoomsX, mapGenSettings.NumRoomsY];

            generateRooms();

            Resources.UnloadUnusedAssets();

            return rooms;
        }

        private void loadRoomTemplates()
        {
            roomTemplatesClosedBottom = RoomTemplateLoader.LoadRoomsSolidBottom(mapGenSettings);
            roomTemplatesOpenBottom = RoomTemplateLoader.LoadRoomsOpenBottom(mapGenSettings);
        }

        private void generateRooms()
        {
            for (int ix = 0; ix < mapGenSettings.NumRoomsX; ix++)
            {
                for (int iy = 0; iy < mapGenSettings.NumRoomsY; iy++)
                {
                    rooms[ix, iy] = generateRoom(new Vector2Int(ix, iy), roomTemplatesClosedBottom);
                }
            }

            // Replace one room on each floor with an opened one.
            for (int i = 1; i < mapGenSettings.NumRoomsY; i++)
            {
                int randomRoom = UnityEngine.Random.Range(0, mapGenSettings.NumRoomsX);
                rooms[randomRoom, i] = generateRoom(new Vector2Int(randomRoom, i), roomTemplatesOpenBottom);
            }
        }

        private RoomData generateRoom(Vector2Int position, List<RoomTemplate> templates)
        {
            RoomTemplate chosenTemplate = templates[UnityEngine.Random.Range(0, templates.Count)];

            RoomData room = new RoomData(position, chosenTemplate);

            return room;
        }
    }

    public struct RoomData
    {
        public bool Occupied;
        public bool IsStartingRoom;
        public Vector2Int Position;
        public RoomTemplate Data;

        public RoomData(Vector2Int position, RoomTemplate data)
        {
            this.Position = position;
            this.Data = data;
            this.Occupied = true;
            this.IsStartingRoom = false;
        }
    }
}
