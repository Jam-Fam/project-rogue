
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Rogue.Maps
{
    public class DungeonGenerator : MonoBehaviour
    {
        // Dependencies 
        [SerializeField] private GameObject tilemapPrefab = null;
        [SerializeField] private GameObject playerPrefab;
        private Transform tileGrid;
        // TEMP
        [SerializeField] private Tile blackTile = null;

        // Components 
        private MapGenerator mapGenerator;

        // Settings 
        [SerializeField] private int numRoomsX = 5;
        [SerializeField] private int numRoomsY = 4;
        [SerializeField] private int roomWidth = 20;
        [SerializeField] private int roomHeight = 16;
        [SerializeField] private int floorHeight = 3;
        [SerializeField] private int wallWidth = 2;
        [SerializeField] private Vector2 playerSpawnOffset = Vector2.zero;

        // State
        private RoomData[,] rooms;
        private List<GameObject> entities;
        private Tilemap currentTilemap;

        void Awake()
        {
            mapGenerator = GetComponent<MapGenerator>();
            entities = new List<GameObject>();

            tileGrid = tilemapPrefab.transform.parent;
        }

        void Start()
        {
            GenerateDungeon();
        }

        public void GenerateDungeon()
        {
            rooms = mapGenerator.GenerateMap(roomWidth, roomHeight, numRoomsX, numRoomsY);

            clearMap();
            drawMap();

            spawnEntities();
        }

        private void clearMap()
        {
            if (currentTilemap != null) Destroy(currentTilemap.gameObject);

            currentTilemap = Instantiate<GameObject>(tilemapPrefab, tileGrid).GetComponent<Tilemap>();
            currentTilemap.gameObject.SetActive(true);
            foreach (GameObject entity in entities) Destroy(entity);

            entities = new List<GameObject>();
        }

        private void drawMap()
        {
            foreach (RoomData room in rooms)
            {
                for (int ix = 0; ix < roomWidth; ix ++)
                {
                    for (int iy = 0; iy < roomHeight; iy++)
                    {
                        if (room.Data.Tiles == null) continue;

                        bool drawTile = room.Data.Tiles[ix, iy];

                        if (drawTile)
                        {
                            Vector3Int position = new Vector3Int(ix + (room.Position.x * (roomWidth + wallWidth)), iy + (room.Position.y * (roomHeight + floorHeight)), 0);
                            //tilemap.SetTile(position, blackTile);
                            currentTilemap.SetTile(position, null);
                        }
                    }
                }
            }
        }

        private void spawnEntities()
        {
            foreach(RoomData room in rooms)
            {
                if (room.IsStartingRoom && playerPrefab != null)
                {
                    // TODO: Scrap 'playerSpawnOffset' for a system that dynamically finds a safe place to spawn entities.
                    Vector3 spawnPosition = (room.Position * new Vector2(roomWidth, roomHeight)) + playerSpawnOffset;
                    GameObject player = (GameObject)Instantiate(playerPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
                    player.SetActive(true);
                    entities.Add(player);
                }
            }
        }
    }
}