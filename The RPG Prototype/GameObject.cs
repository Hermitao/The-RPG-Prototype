using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    class GameObject
    {
        public Actor actor;

        public enum objectToInstantiate
        {
            Enemy,
            Player
        }

        public GameObject(objectToInstantiate objToInst, Vector2 position)
        {
            if (objToInst == objectToInstantiate.Player)
            {
                actor = new Actor(Actor.TypesOfActor.Player, position);
            }
        }

        public void Update(GameTime gameTime)
        {
            actor.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            actor.Draw(spriteBatch);
        }
    }
}
