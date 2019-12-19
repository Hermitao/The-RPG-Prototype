﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;

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
        AnimatedSprite jumpChargeAnim;
        AnimatedSprite fallAnim;
        AnimatedSprite crouchMoveAnim;

        SoundEffect footstep;
        
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
        private bool isChargingJump;
        private bool previousIsChargingJump;
        public float maxJumpTimer;
        private float jumpTimer;
        
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
            isChargingJump = false;
            previousIsChargingJump = isChargingJump;

            movementMultiplier = 0f;
            jumpSpeed = 180f;
            maxJumpTimer = .15f;
            jumpTimer = 0f;

            density = 1f;
            dragCoefficient = .8f;
            area = .5f;
        }

        public void LoadContent(Texture2D idleTexture, Texture2D runningTexture, Texture2D crouchTexture, Texture2D jumpTexture, Texture2D jumpChargeTexture, Texture2D fallTexture)
        {
            idleAnim = new AnimatedSprite(idleTexture, 1, 3, 4f);
            runningAnim = new AnimatedSprite(runningTexture, 1, 6, 9f);
            crouchAnim = new AnimatedSprite(crouchTexture, 1, 4, 4f);
            jumpAnim = new AnimatedSprite(jumpTexture, 1, 1, 1f);
            jumpChargeAnim = new AnimatedSprite(jumpChargeTexture, 1, 2, 12f);
            fallAnim = new AnimatedSprite(fallTexture, 1, 2, 12f);
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

            //if (myKeyboardState.IsKeyDown(myJumpKey) && myPreviousKeyboardState.IsKeyUp(myJumpKey) && isGrounded)
            //{
            //    isJumping = true;
            //    rigidbody.AddForce(new Vector2(0f, -jumpSpeed), Rigidbody.ForceMode.Impulse);
            //}

            if (myKeyboardState.IsKeyDown(myJumpKey) && myPreviousKeyboardState.IsKeyUp(myJumpKey) && isGrounded)
            {
                isChargingJump = true;
            }
            if (isChargingJump)
            {
                jumpTimer += Game1.deltaTime;

                if (myKeyboardState.IsKeyUp(myJumpKey))
                {
                    isJumping = true;
                    rigidbody.AddForce((new Vector2(0f, -jumpSpeed) * jumpTimer * 10f) + new Vector2(0, -30f), Rigidbody.ForceMode.Impulse);

                    isChargingJump = false;
                    jumpTimer = 0f;
                }

                if (jumpTimer >= maxJumpTimer && isGrounded)
                {
                    isJumping = true;
                    rigidbody.AddForce((new Vector2(0f, -jumpSpeed) * maxJumpTimer * 10f) + new Vector2(0, -30f), Rigidbody.ForceMode.Impulse);

                    isChargingJump = false;
                    jumpTimer = 0f;
                }
            }

            if (!isGrounded)
            {
                isChargingJump = false;
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

            jumpAnim.Update();
            fallAnim.Update();
            jumpChargeAnim.Update();

            previousIsChargingJump = isChargingJump;

            // air resistance calculation - might be wrong
            resistance = (density * dragCoefficient * area) / 2f * (float)Math.Pow(rigidbody.velocity.X, 2);
            
            rigidbody.velocity.X = maxSpeed * movementMultiplier;
            rigidbody.Update(gameTime);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isMovingRight && isGrounded && !isChargingJump)
            {
                runningAnim.Draw(spriteBatch, transform.position);
            }
            else if (isMovingLeft && isGrounded && !isChargingJump)
            {
                runningAnim.Draw(spriteBatch, transform.position, true);
            }
            else if (isIdle && isGrounded && !isChargingJump)
            {
                if (isFacingRight)
                {
                    idleAnim.Draw(spriteBatch, transform.position);
                } else
                {
                    idleAnim.Draw(spriteBatch, transform.position, true);
                }
            }
            else if (isCrouching && !isChargingJump)
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

            if (isChargingJump)
            {
                if (isFacingRight)
                {
                    jumpChargeAnim.Draw(spriteBatch, transform.position);
                }
                else
                {
                    jumpChargeAnim.Draw(spriteBatch, transform.position, true);
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
