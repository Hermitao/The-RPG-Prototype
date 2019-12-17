using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    class Player
    {
        Transform transform;
        Rigidbody rigidbody;

        float maxSpeed;
        AnimatedSprite idleAnim;
        AnimatedSprite runningAnim;
        
        KeyboardState keyboardState;
        Keys myLeftKey;
        Keys myRightKey;
        Keys myDownKey;
        Keys myJumpKey;

        private bool isMovingRight;
        private bool isMovingLeft;
        private bool isFacingRight;
        private bool isIdle;
        private float movementMaxForce;
        private float movementForce;
        private float movementMultiplier;
        private float jumpSpeed;

        private float resistance;
        private float density;
        private float dragCoefficient;
        private float area;

        public bool isGrounded;

        public Player(float playerXPosition, float playerYPosition, Keys leftKey, Keys rightKey, Keys downKey, Keys jumpKey)
        {
            rigidbody = new Rigidbody(60f, 0f);
            if (rigidbody == null)
            {
                transform = new Transform();
            } else
            {
                transform = rigidbody.transform;
            }

            transform.position = new Vector2(playerXPosition, playerYPosition);
            maxSpeed = 500f;

            myLeftKey = leftKey;
            myRightKey = rightKey;
            myDownKey = downKey;
            myJumpKey = jumpKey;

            isMovingRight = false;
            isMovingLeft = false;
            isFacingRight = true;
            isIdle = true;

            movementMultiplier = 0f;
            jumpSpeed = 10f;

            density = 1f;
            dragCoefficient = .8f;
            area = .5f;
        }

        public void LoadContent(Texture2D idleTexture, Texture2D runningTexture)
        {
            idleAnim = new AnimatedSprite(idleTexture, 1, 4, 6f);
            runningAnim = new AnimatedSprite(runningTexture, 1, 8, 12f);
        }

        public void Update(GameTime gameTime)
        {
            keyboardState = Keyboard.GetState();

            if (keyboardState.IsKeyDown(myLeftKey))
            {
                isMovingLeft = true;
                isFacingRight = false;
                movementMultiplier = -1f;
            } else { isMovingLeft = false; }
            if (keyboardState.IsKeyDown(myRightKey))
            {
                isMovingRight = true;
                isFacingRight = true;
                movementMultiplier = 1f;
                
            } else { isMovingRight = false; }

            if (isMovingRight || isMovingLeft)
            {
                runningAnim.Update();
                runningAnim.timeRemainingThisFrame -= Game1.deltaTime;
            }

            if (!isMovingRight && !isMovingLeft)
            {
                movementMultiplier = 0f;
                isIdle = true;
            } else
            {
                isIdle = false;
            }

            if (isIdle)
            {
                idleAnim.Update();
                idleAnim.timeRemainingThisFrame -= Game1.deltaTime;
            }

            // air resistance calculation - might be wrong
            resistance = (density * dragCoefficient * area) / 2f * (float)Math.Pow(rigidbody.velocity.X, 2);
            
            rigidbody.velocity = new Vector2(maxSpeed * movementMultiplier, rigidbody.velocity.Y);

            rigidbody.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isMovingRight)
            {
                runningAnim.Draw(spriteBatch, transform.position);
            }
            else if (isMovingLeft)
            {
                runningAnim.Draw(spriteBatch, transform.position, true);
            }
            else if (isIdle)
            {
                if (isFacingRight)
                {
                    idleAnim.Draw(spriteBatch, transform.position);
                } else
                {
                    idleAnim.Draw(spriteBatch, transform.position, true);
                }
            }
        }
    }
}
