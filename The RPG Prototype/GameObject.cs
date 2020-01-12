using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    class GameObject
    {
        public Actor actor;
        public Tile tile;

        public enum actorToInstantiate
        {
            Player
        }

        public enum objectToInstantiate
        {
            Tile
        }

        public GameObject(actorToInstantiate actorToInst, Vector2 position)
        {
            if (actorToInst == actorToInstantiate.Player)
            {
                actor = new Actor(Actor.TypesOfActor.Player, position);
            }
        }

        public GameObject (objectToInstantiate objToInst, Vector2 position)
        {
            if (objToInst == objectToInstantiate.Tile)
            {
                tile = new Tile(position);
            }
        }

        public void Update(GameTime gameTime)
        {
            if (actor != null)
            {
                actor.Update(gameTime);
            }
            else if (tile != null)
            {
                tile.Update();
            }
        }

        public void InitializeObject()
        {
            if (actor != null)
            {
                actor.InitializeObject();
            }
            else if (tile != null)
            {
                tile.InitializeObject();
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (actor != null)
            {
                actor.Draw(spriteBatch);
            }
            else if (tile != null)
            {
                tile.Draw(spriteBatch);
            }
        }
    }
}
