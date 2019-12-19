using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace The_RPG_Prototype
{
    class AnimatedSprite
    {
        public Texture2D Texture { get; set; }
        public int Rows { get; set; }
        public int Columns { get; set; }
        int currentFrame;
        int totalFrames;
        float animationFrameTime;
        public float timeRemainingThisFrame;
        public int width;
        public int height;
        public float originX;
        public float originY;

        public Rectangle sourceRectangle;
        public Rectangle destinationRectangle;

        public AnimatedSprite(Texture2D texture, int rows, int columns, float frameRate)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            animationFrameTime = 1 / frameRate;
            timeRemainingThisFrame = animationFrameTime;
        }

        public void Update()
        {
            timeRemainingThisFrame -= Game1.deltaTime;
            if (timeRemainingThisFrame <= 0f)
            {
                
                currentFrame++;
                if (currentFrame == totalFrames)
                {
                    currentFrame = 0;
                }
                timeRemainingThisFrame += animationFrameTime;
            }
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, bool flipX = false)
        {
            width = Texture.Width / Columns;
            height = Texture.Height / Rows;
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            sourceRectangle = new Rectangle(width * column, height * row, width, height);
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            originX = (float)width / 2f;
            originY = (float)height / 2f;

            if (!flipX)
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, new Vector2(originX, originY), SpriteEffects.None, 0f);
            } else
            {
                spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, 0f, new Vector2(originX, originY), SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}
