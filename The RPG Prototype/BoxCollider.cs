using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace The_RPG_Prototype
{
    public class BoxCollider
    {
        public Vector2 _min;
        public Vector2 _max;

        public float width;
        public float height;

        public float x;
        public float y;

        public static List<BoxCollider> AllBoxColliders;

        public bool collidingDown;
        public bool collidingUp;
        public bool collidingRight;
        public bool collidingLeft;
        

        /// <summary> Axis-Aligned Bounding Box's two coordinates. </summary>
        public BoxCollider(Vector2 min, Vector2 max)
        {
            _min = min;
            _max = max;

            width = _max.X - _min.X;
            height = _max.Y - _min.Y;

            x = 0f;
            y = 0f;

            if (AllBoxColliders == null)
            {
                 AllBoxColliders = new List<BoxCollider>();
            }
            AllBoxColliders.Add(this);
        }

        public void Update(Vector2 position)
        {
            x = position.X + _min.X;
            y = position.Y + _min.Y;

            Game1.collidingDown = collidingDown;
            Game1.collidingUp = collidingUp;
            Game1.collidingRight = collidingRight;
            Game1.collidingLeft = collidingLeft;
        }

        public void Draw(SpriteBatch spriteBatch, int RedValue, int GreenValue, int BlueValue)
        {
            Texture2D Pixel = Game1.pixel;

            //spriteBatch.Draw(Pixel,
            //    new Rectangle((int)x, (int)y, (int)width, (int)height),
            //    new Color((byte)RedValue, (byte)GreenValue, (byte)BlueValue, (byte)120));

            if (Game1.debugOverlay)
            {
                spriteBatch.Draw(Pixel,
                    new Vector2(x, y),
                    new Rectangle((int)x, (int)y, (int)width, (int)height),
                    new Color((byte)RedValue, (byte)GreenValue, (byte)BlueValue, (byte)120) * .7f);
            }
        }

        public bool OnCollisionStay()
        {
            foreach (BoxCollider other in AllBoxColliders)
            {
                if (other != this)
                {
                    if (x < other.x + other.width &&
                    x + width > other.x &&
                    y < other.y + other.height &&
                    y + height > other.y)
                    {
                        if (x + width > other.x)
                        {

                        }

                        return true;
                    }
                }
            }
            return false;
        }
    }
}
