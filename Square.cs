using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PinBallBattles
{
    class Square : CollisionObject
    {
        private int width;
        private int height;

        public int Width { get => width; }
        public int Height { get => height; }

        #region Create

        public Square(Shape shape, Vector2 position, int width, int height, bool isPlayer)
            : base(shape, position, isPlayer)
        {
            this.width = width;
            this.height = height;
        }

        public override void CreateTexture(GraphicsDevice graphics)
        {
            Texture = new Texture2D(graphics, Width, Height);
            Color[] colorData = new Color[Width * Height];

            int index = 0;
            for (int x = 0; x < Width; x++)
            {
                for (int y = 0; y < Height; y++)
                {
                    colorData[index] = Color.White;
                    index++;
                }
            }

            Texture.SetData(colorData);

            base.CreateTexture(graphics);
        }

        #endregion

        #region Forces

        #endregion

        #region Collision

        #endregion

        #region PositionalChecks

        public override Vector2 ClosestEdge(Vector2 testObject)
        {
            Vector2 testVector = testObject;
            if (testObject.X < Position.X - Width / 2)
            {
                testVector.X = Position.X - Width / 2;
            }
            else if (testObject.X > Position.X + Width / 2)
            {
                testVector.X = Position.X + Width / 2;
            }
            if (testObject.Y < Position.Y - Height / 2)
            {
                testVector.Y = Position.Y - Height / 2;
            }
            else if (testObject.Y > Position.Y + Height / 2)
            {
                testVector.Y = Position.Y + Height / 2;
            }

            return testVector;
        }

        #endregion
    }
}
