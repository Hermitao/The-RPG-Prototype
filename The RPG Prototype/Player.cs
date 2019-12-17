using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace The_RPG_Prototype
{
    class Player
    {
        public Vector2 playerPosition;
        float playerSpeed;
        AnimatedSprite idleAnim;
        AnimatedSprite runningAnim;

        KeyboardState keyboardState;
        Keys myLeftKey;
        Keys myRightKey;
        Keys myDownKey;
        Keys myJumpKey;

        private bool isMovingRight;
        private bool isMovingLeft;
        private bool isIdle;

        public Player(float playerXPosition, float playerYPosition, Keys leftKey, Keys rightKey, Keys downKey, Keys jumpKey)
        {
            playerPosition = new Vector2(playerXPosition, playerYPosition);
            playerSpeed = 400f;

            myLeftKey = leftKey;
            myRightKey = rightKey;
            myDownKey = downKey;
            myJumpKey = jumpKey;

            isMovingRight = false;
            isMovingLeft = false;
            isIdle = true;
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
                playerPosition.X -= playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            } else { isMovingLeft = false; }
            if (keyboardState.IsKeyDown(myRightKey))
            {
                isMovingRight = true;
                playerPosition.X += playerSpeed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            } else { isMovingRight = false; }

            if (isMovingRight || isMovingLeft)
            {
                runningAnim.Update();
                runningAnim.timeRemainingThisFrame -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }

            if (!isMovingRight && !isMovingLeft)
            {
                isIdle = true;
            } else
            {
                isIdle = false;
            }

            if (isIdle)
            {
                idleAnim.Update();
                idleAnim.timeRemainingThisFrame -= (float)gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            if (isMovingRight)
            {
                runningAnim.Draw(spriteBatch, playerPosition);

            }
            else if (isMovingLeft)
            {
                runningAnim.Draw(spriteBatch, playerPosition, true);
            }
            else if (isIdle)
            {
                idleAnim.Draw(spriteBatch, playerPosition);
            }
        }

    }
}
