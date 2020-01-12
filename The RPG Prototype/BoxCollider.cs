using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace The_RPG_Prototype
{
    class BoxCollider
    {
        public Vector2 _min;
        public Vector2 _max;

        public float width;
        public float height;

        public float x;
        public float y;

        public static List<BoxCollider> AllBoxColliders = new List<BoxCollider>();

        /// <summary> Axis-Aligned Bounding Box's two coordinates. </summary>
        public BoxCollider(Vector2 min, Vector2 max)
        {
            _min = min;
            _max = max;

            width = _max.X - _min.X;
            height = _max.Y - _min.Y;

            x = 0f;
            y = 0f;
        }

        public void Update(Vector2 position)
        {
            x = position.X;
            y = position.Y;
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
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}
