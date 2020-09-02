
using System;
using System.Collections.Generic;

using UnityEngine;

namespace ProjectRogue.Maps
{
    public class MapGenerator : MonoBehaviour
    {
        // Settings
        private MapGenerationSettings mapGenSettings;

        // State
        private RoomData[,] rooms;
        private List<RoomTemplate> roomTemplatesNoHole, roomTemplatesBottomHole;

        public RoomData[,] GenerateMap(MapGenerationSettings mapGenSettings)
        {
            this.mapGenSettings = mapGenSettings;

            loadRoomTemplates();

            rooms = rooms = new RoomData[mapGenSettings.NumRoomsX, mapGenSettings.NumRoomsY];

            generateRooms();
            //generateMainPath();
            //generateSidePaths();

            Resources.UnloadUnusedAssets();

            return rooms;
        }

        private void loadRoomTemplates()
        {
            roomTemplatesNoHole = RoomTemplateLoader.LoadRoomsSolidBottom(mapGenSettings);
            roomTemplatesBottomHole = RoomTemplateLoader.LoadRoomsOpenBottom(mapGenSettings);
        }

        private void generateRooms()
        {
            for (int ix = 0; ix < mapGenSettings.NumRoomsX; ix++)
            {
                for (int iy = 0; iy < mapGenSettings.NumRoomsY; iy++)
                {
                    rooms[ix, iy] = generateRoom(new Vector2Int(ix, iy), roomTemplatesNoHole);
                }
            }
        }

        /*
        private void generateMainPath()
        {
            Vector2Int currentPosition = new Vector2Int(0, Random.Range(0, numRoomsY));
            List<RoomTemplateData> templates = templatesNoHole;

            // Generate starting room.
            rooms[currentPosition.x, currentPosition.y] = generateRoom(currentPosition, templates);
            rooms[currentPosition.x, currentPosition.y].IsStartingRoom = true;

            int attempts = 0;

            while (true && attempts < 100)
            {
                // Stop if we've reached the opposite wall.
                if (currentPosition.x == numRoomsX - 1) break;

                // Create a list of directions in which we might place the next room.
                List<BranchDirection> branchDirections = new List<BranchDirection>();

                // If we can go right, add that to the list.
                if (checkCanGenerateRoom(currentPosition + new Vector2Int(1, 0)))
                {
                    branchDirections.Add(BranchDirection.Right);
                }

                // Same if we can go down.
                if (checkCanGenerateRoom(currentPosition + new Vector2Int(0, -1)))
                {
                    branchDirections.Add(BranchDirection.Down);
                }

                // And if we can go up.
                if (checkCanGenerateRoom(currentPosition + new Vector2Int(0, 1)))
                {
                    branchDirections.Add(BranchDirection.Up);
                }

                // Now pick a random direction from the list.
                BranchDirection directionChoice = branchDirections[Random.Range(0, branchDirections.Count)];

                switch (directionChoice)
                {
                    case BranchDirection.Right:
                        {
                            templates = templatesNoHole;
                            currentPosition += new Vector2Int(1, 0);
                            break;
                        }
                    case BranchDirection.Down:
                        {
                            // We need to make a hole so we can go down.
                            rooms[currentPosition.x, currentPosition.y] = generateRoom(currentPosition, templatesBottomHole);

                            templates = templatesNoHole;
                            currentPosition += new Vector2Int(0, -1);

                            break;
                        }
                    case BranchDirection.Up:
                        {
                            templates = templatesBottomHole;
                            currentPosition += new Vector2Int(0, 1);
                            break;
                        }
                }

                // Choose a random room from the current template.
                rooms[currentPosition.x, currentPosition.y] = generateRoom(currentPosition, templates);

                attempts++;
            }
        }
        private bool checkCanGenerateRoom(Vector2Int position)
        {
            bool available = (position.x < numRoomsX)
                && (position.x >= 0)
                && (position.y < numRoomsY)
                && (position.y >= 0)
                && !rooms[position.x, position.y].Occupied;

            return available;
        }
        */

        private RoomData generateRoom(Vector2Int position, List<RoomTemplate> templates)
        {
            RoomTemplate chosenTemplate = templates[UnityEngine.Random.Range(0, templates.Count)];

            RoomData room = new RoomData(position, chosenTemplate);

            return room;
        }

        private enum BranchDirection
        {
            Right, Up, Down
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
