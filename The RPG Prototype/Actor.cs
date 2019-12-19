using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    class Actor
    {
        public Player player;

        public Actor()
        {
            player = new Player(0f, 0f, Keys.A, Keys.D, Keys.S, Keys.Space);
        }
    }
}
