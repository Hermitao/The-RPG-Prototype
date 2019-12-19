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
        private bool previousIsGrounded;

        public enum ForceMode
        {
            Force,
            Impulse
        }

        public Rigidbody(float mass = 1f, float gravityScale = 1f)
        {
            isEnabled = true;
            transform = new Transform();
            gravity = new Vector2(Game1.gravityX * gravityScale, Game1.gravityY * gravityScale);
            isGrounded = false;
            myMass = mass;
            gravityForce = myMass * gravity;
        }

        public void Update(GameTime gameTime)
        {
            //acceleration = receivedForce + gravityForce / myMass; // F = ma - Newton's second law of motion

            //check if should apply gravity
            if (!isGrounded)
            {
                AddForce(gravityForce, ForceMode.Force);
            }
            if (isGrounded && !previousIsGrounded)
            {
                velocity = new Vector2(velocity.X, 0f);
            }

            transform.position += velocity * Game1.deltaTime;

            if (transform.position.Y >= 0f && !previousIsGrounded)
            {
                transform.position.Y = 0f;
                velocity = new Vector2(velocity.X, 0f);
                isGrounded = true;
            } else if (transform.position.Y < 0f)
            {
                isGrounded = false;
            }

            previousIsGrounded = isGrounded;
        }
        public void AddForce(Vector2 appliedForce, ForceMode forceMode = ForceMode.Force)
        {
            if (forceMode == ForceMode.Force)
            {
                velocity += (appliedForce / myMass) * Game1.deltaTime;
            }
            if (forceMode == ForceMode.Impulse)
            {
                velocity += appliedForce;
            }
        }
    }
}
