
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace ProjectRogue.Maps
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
        [SerializeField] private MapGenerationSettings mapGenSettings;
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
            rooms = mapGenerator.GenerateMap(mapGenSettings);

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
                for (int ix = 0; ix < mapGenSettings.RoomWidth; ix ++)
                {
                    for (int iy = 0; iy < mapGenSettings.RoomHeight; iy++)
                    {
                        if (room.Data.Tiles == null) continue;

                        bool drawTile = room.Data.Tiles[ix, iy];

                        if (drawTile)
                        {
                            Vector3Int position = new Vector3Int(ix + (room.Position.x * (mapGenSettings.RoomWidth + mapGenSettings.WallWidth)), iy + (room.Position.y * (mapGenSettings.RoomHeight + mapGenSettings.FloorHeight)), 0);
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
                    Vector3 spawnPosition = (room.Position * new Vector2(mapGenSettings.RoomWidth, mapGenSettings.RoomHeight)) + playerSpawnOffset;
                    GameObject player = (GameObject)Instantiate(playerPrefab, new Vector3(spawnPosition.x, spawnPosition.y, 0), Quaternion.identity);
                    player.SetActive(true);
                    entities.Add(player);
                }
            }
        }
    }
}