namespace ProjectRogue.Maps
{
    public struct RoomTemplate
    {
        public bool[,] Tiles;

        public RoomTemplate(int roomWidth, int roomHeight)
        {
            Tiles = new bool[roomWidth, roomHeight];
        }
    }
}
