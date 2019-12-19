using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    class Player
    {
        public Transform transform;
        public Rigidbody rigidbody;

        AnimatedSprite idleAnim;
        AnimatedSprite runningAnim;
        AnimatedSprite crouchAnim;
        AnimatedSprite jumpAnim;
        AnimatedSprite fallAnim;
        AnimatedSprite crouchMoveAnim;
        
        Keys myLeftKey;
        Keys myRightKey;
        Keys myDownKey;
        Keys myJumpKey;

        private bool isMovingRight;
        private bool isMovingLeft;
        private bool isFacingRight;
        private bool isIdle;
        private bool isCrouching;
        private bool isJumping;
        private bool isFalling;
        private float movementMaxForce;
        private float movementForce;
        private float movementMultiplier;
        float maxSpeed;
        private float jumpSpeed;

        private float resistance;
        private float density;
        private float dragCoefficient;
        private float area;

        public bool isGrounded;

        public Player(float playerXPosition, float playerYPosition, Keys leftKey, Keys rightKey, Keys downKey, Keys jumpKey)
        {
            rigidbody = new Rigidbody(60f, 1f);
            
            if (rigidbody == null)
            {
                transform = new Transform();
            } else
            {
                transform = rigidbody.transform;
            }

            transform.position = new Vector2(playerXPosition, playerYPosition);
            maxSpeed = 160f;

            myLeftKey = leftKey;
            myRightKey = rightKey;
            myDownKey = downKey;
            myJumpKey = jumpKey;

            isMovingRight = false;
            isMovingLeft = false;
            isFacingRight = true;
            isIdle = true;
            isCrouching = false;
            isJumping = false;
            isFalling = false;

            movementMultiplier = 0f;
            jumpSpeed = 180f;

            density = 1f;
            dragCoefficient = .8f;
            area = .5f;
        }

        public void LoadContent(Texture2D idleTexture, Texture2D runningTexture, Texture2D crouchTexture, Texture2D jumpTexture, Texture2D fallTexture)
        {
            idleAnim = new AnimatedSprite(idleTexture, 1, 3, 4f);
            runningAnim = new AnimatedSprite(runningTexture, 1, 6, 9f);
            crouchAnim = new AnimatedSprite(crouchTexture, 1, 4, 4f);
            jumpAnim = new AnimatedSprite(jumpTexture, 1, 1, 1f);
            fallAnim = new AnimatedSprite(fallTexture, 1, 2, 8f);
        }

        public void Update(GameTime gameTime, KeyboardState myKeyboardState, KeyboardState myPreviousKeyboardState)
        {
            isGrounded = rigidbody.isGrounded;

            if (myKeyboardState.IsKeyDown(myLeftKey) && !isCrouching)
            {
                isMovingLeft = true;
                isFacingRight = false;
                movementMultiplier = -1f;
            } else { isMovingLeft = false; }
            if (myKeyboardState.IsKeyDown(myRightKey) && !isCrouching)
            {
                isMovingRight = true;
                isFacingRight = true;
                movementMultiplier = 1f;
                
            } else { isMovingRight = false; }

            if (myKeyboardState.IsKeyDown(myDownKey) && isGrounded)
            {
                isCrouching = true;
            } else
            {
                isCrouching = false;
            }

            if (myKeyboardState.IsKeyDown(myJumpKey) && myPreviousKeyboardState.IsKeyUp(myJumpKey) && isGrounded)
            {
                isJumping = true;
                rigidbody.AddForce(new Vector2(0f, -jumpSpeed), Rigidbody.ForceMode.Impulse);
            }

            if (isMovingRight || isMovingLeft)
            {
                runningAnim.Update();
            }

            if (!isMovingRight && !isMovingLeft && !isCrouching)
            {
                isIdle = true;
            } else
            {
                isIdle = false;
            }

            if (isIdle)
            {
                movementMultiplier = 0f;
                idleAnim.Update();
            }
            if (isCrouching)
            {
                movementMultiplier = 0f;
                crouchAnim.Update();
            }

            // air resistance calculation - might be wrong
            resistance = (density * dragCoefficient * area) / 2f * (float)Math.Pow(rigidbody.velocity.X, 2);
            
            rigidbody.velocity.X = maxSpeed * movementMultiplier;
            
            rigidbody.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isMovingRight && isGrounded)
            {
                runningAnim.Draw(spriteBatch, transform.position);
            }
            else if (isMovingLeft && isGrounded)
            {
                runningAnim.Draw(spriteBatch, transform.position, true);
            }
            else if (isIdle && isGrounded)
            {
                if (isFacingRight)
                {
                    idleAnim.Draw(spriteBatch, transform.position);
                } else
                {
                    idleAnim.Draw(spriteBatch, transform.position, true);
                }
            }
            else if (isCrouching)
            {
                if (isFacingRight)
                {
                    crouchAnim.Draw(spriteBatch, transform.position);
                }
                else
                {
                    crouchAnim.Draw(spriteBatch, transform.position, true);
                }
            }
            else if (!isGrounded)
            {
                if (rigidbody.velocity.Y < 20f)
                {
                    if (isFacingRight)
                    {
                        jumpAnim.Draw(spriteBatch, transform.position);
                    }
                    else
                    {
                        jumpAnim.Draw(spriteBatch, transform.position, true);
                    }
                }
                else
                {
                    if (isFacingRight)
                    {
                        fallAnim.Draw(spriteBatch, transform.position);
                    }
                    else
                    {
                        fallAnim.Draw(spriteBatch, transform.position, true);
                    }
                }
            }

        }
    }
}
