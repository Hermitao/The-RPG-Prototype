using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_RPG_Prototype
{
    class GameObject
    {
        public Actor playerActor;

        public enum objectToInstantiate
        {
            Actor,
            Player
        }

        public GameObject(objectToInstantiate objToInst)
        {
            if (objToInst == objectToInstantiate.Player)
            {
                playerActor = new Actor();
            }
        }
    }
}
