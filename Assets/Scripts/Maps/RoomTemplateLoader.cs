
using System.Collections.Generic;
using System.IO;
using System.Linq;

using UnityEngine;

namespace ProjectRogue.Maps
{
    static class RoomTemplateLoader
    {
        private const string templateDirectorySolidBottom = "SolidBottom";
        private const string templateDirectoryOpenBottom = "OpenBottom";

        public static List<RoomTemplate> LoadRoomsSolidBottom(MapGenerationSettings mapGenSettings)
        {
            return loadRoomTemplateCategory(templateDirectorySolidBottom, mapGenSettings);
        }

        public static List<RoomTemplate> LoadRoomsOpenBottom(MapGenerationSettings mapGenSettings)
        {
            return loadRoomTemplateCategory(templateDirectoryOpenBottom, mapGenSettings);
        }

        private static List<RoomTemplate> loadRoomTemplateCategory(string directory, MapGenerationSettings mapGenSettings)
        {
            var templates = new List<RoomTemplate>();

            // Load room templates.
            string templatePath = Path.Combine(mapGenSettings.TemplatesRootDir, directory);
            Texture2D[] templateTextures = Resources.LoadAll(templatePath, typeof(Texture2D)).Cast<Texture2D>().ToArray();

            Debug.Log(templatePath);

            foreach (Texture2D templateTexture in templateTextures)
            {
                // Create template for this room;
                var template = new RoomTemplate(mapGenSettings.RoomWidth, mapGenSettings.RoomHeight);

                Color[] pixels = templateTexture.GetPixels();

                for (int ix = 0; ix < mapGenSettings.RoomWidth; ix++)
                {
                    for (int iy = 0; iy < mapGenSettings.RoomHeight; iy++)
                    {
                        Color pixel = pixels[iy * mapGenSettings.RoomWidth + ix];
                        template.Tiles[ix, iy] = pixel == Color.white;
                    }
                }

                templates.Add(template);
            }

            return templates;
        }
    }
}
