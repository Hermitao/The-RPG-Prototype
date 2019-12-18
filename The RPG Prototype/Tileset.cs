using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_RPG_Prototype
{
    class Tileset
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        public int TileSize { get; set; }
        public int width;
        public int height;
        public int currentTile;
        public int chunkSize;

        public Tileset(Texture2D texture, int rows, int columns, int tileSize)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            TileSize = tileSize;
            currentTile = 0;
            chunkSize = 17;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            width = TileSize;
            height = TileSize;
            int row = (int)((float)currentTile / (float)Columns);
            int column = currentTile % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle;
            

            float originX = (float)width;
            float originY = (float)height;

            float startXLocation = location.X;
            for (int y = 0; y < chunkSize; y++)
            {
                for (int x = 0; x < chunkSize; x++)
                {
                    destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
                    spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, new Vector2(0f, 0f), SpriteEffects.None, 0f);
                    location.X += TileSize;
                }
                location.X = startXLocation;
                location.Y += TileSize;
            }
        }
    }
}
