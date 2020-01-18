using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_RPG_Prototype
{
    public class Tile
    {
        public BoxCollider boxCollider;

        public Vector2 _position;
        public Texture2D _texture { get; set; }
        
        int spriteWidth;
        int spriteHeight;

        Rectangle destinationRectangle;

        public Tile(Vector2 position)
        {
            _position = position;
        }

        public void LoadContent(Texture2D texture)
        {
            _texture = texture;
            boxCollider = new BoxCollider(
                Vector2.Zero,
                new Vector2(_texture.Width, _texture.Height)
                );
        }

        public void InitializeObject()
        {
            
        }

        public void Update()
        {
            boxCollider.Update(_position);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteWidth = _texture.Width;
            spriteHeight = _texture.Height;

            destinationRectangle = new Rectangle((int)_position.X, (int)_position.Y, spriteWidth, spriteHeight);

            spriteBatch.Draw(_texture, destinationRectangle, Color.White);

            if (Game1.debugShowHitBoxes)
            {
                boxCollider.Draw(spriteBatch, Color.Green.R, Color.Green.G, Color.Green.B);
            }
        }
    }
}
