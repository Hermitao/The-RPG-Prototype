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
        public bool _overrideOrigin;
        public float myRotation;
        public Vector2 overriddenOrigin;

        public Rectangle sourceRectangle;
        public Rectangle destinationRectangle;

        public AnimatedSprite(Texture2D texture, int rows, int columns, float frameRate, bool overrideOrigin = false, float originOverrideX = 0f, float originOverrideY = 0f, float rotation = 0f)
        {
            Texture = texture;
            Rows = rows;
            Columns = columns;
            currentFrame = 0;
            totalFrames = Rows * Columns;
            animationFrameTime = 1 / frameRate;
            timeRemainingThisFrame = animationFrameTime;
            _overrideOrigin = overrideOrigin;
            originX = originOverrideX;
            originY = originOverrideY;
            overriddenOrigin = new Vector2(originX, originY);
            myRotation = rotation;

            width = Texture.Width / Columns;
            height = Texture.Height / Rows;
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
            int row = (int)((float)currentFrame / (float)Columns);
            int column = currentFrame % Columns;

            sourceRectangle = new Rectangle(width * column, height * row, width, height);
            destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);

            if (!_overrideOrigin)
            {
                originX = (float)width / 2f;
                originY = (float)height / 2f;
            }

            if (!flipX)
            {
                //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, myRotation, new Vector2(originX, originY), SpriteEffects.None, 0f);
                spriteBatch.Draw(Texture, location, sourceRectangle, Color.White, myRotation, new Vector2(originX, originY), 1f, SpriteEffects.None, 0f);
                //spriteBatch.Draw(Texture, null, sourceRectangle, Color.White, myRotation, new Vector2(originX, originY), SpriteEffects.None, 0f);
            } else
            {
                //spriteBatch.Draw(Texture, destinationRectangle, sourceRectangle, Color.White, myRotation, new Vector2(originX, originY), SpriteEffects.FlipHorizontally, 0f);
                spriteBatch.Draw(Texture, location, sourceRectangle, Color.White, myRotation, new Vector2(originX, originY), 1f, SpriteEffects.FlipHorizontally, 0f);
            }
        }
    }
}
