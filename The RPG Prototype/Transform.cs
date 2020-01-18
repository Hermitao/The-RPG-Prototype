using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_RPG_Prototype
{
    public class Transform
    {
        public Vector2 position;

        public Transform(Vector2 startPosition = new Vector2())
        {
            position = startPosition;
        }
    }
}
