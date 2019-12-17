using Microsoft.Xna.Framework;

namespace The_RPG_Prototype
{
    class Rigidbody
    {
        public bool isEnabled;
        public Transform transform;

        public Vector2 gravity;
        public Vector2 gravityForce;
        public Vector2 velocity;
        public Vector2 acceleration;
        public Vector2 receivedForce;
        public float myMass;
        public bool isGrounded;

        public Rigidbody(float mass = 1f, float gravityScale = 1f)
        {
            isEnabled = true;
            transform = new Transform();
            gravity = new Vector2(Game1.gravityX * gravityScale, Game1.gravityY * gravityScale);
            isGrounded = true;
            myMass = mass;
        }

        public void Update(GameTime gameTime)
        {
            ReceiveGravity(gameTime);
            acceleration = receivedForce + gravityForce / myMass; // F = ma - Newton's second law of motion
            velocity += acceleration * Game1.deltaTime;
            transform.position += velocity * Game1.deltaTime;
        }

        void ReceiveGravity(GameTime gameTime)
        {
            gravityForce = myMass * gravity;
        }

        public void AddForce(Vector2 appliedForce)
        {
            receivedForce = appliedForce;
        }
    }
}
