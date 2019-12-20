using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace The_RPG_Prototype
{
    class Actor
    {
        public enum TypesOfActor
        {
            Player
        }

        public Player player;

        public Actor(TypesOfActor typeOfActor, Vector2 position)
        {
            if (typeOfActor == TypesOfActor.Player)
            {
                player = new Player(position.X, position.Y, Keys.A, Keys.D, Keys.S, Keys.Space);
            }
        }

        public void Update(GameTime gameTime)
        {
            player.Update(gameTime, Game1.keyboardState, Game1.previousKeyboardState);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            player.Draw(spriteBatch);
        }
    }
}
