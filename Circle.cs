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
    class Circle : CollisionObject
    {
        private int radius;

        public int Radius { get => radius; }

        #region Create

        public Circle(Shape shape, Vector2 position, int radius, bool isPlayer)
            : base(shape, position, isPlayer)
        {
            this.radius = radius;
        }

        public override void CreateTexture(GraphicsDevice graphics)
        {
            base.CreateTexture(graphics);

            int diameter = radius * 2;
            float diamsq = radius * radius;

            Texture = new Texture2D(graphics, diameter, diameter);
            Color[] colorData = new Color[diameter * diameter];


            for (int x = 0; x < diameter; x++)
            {
                for (int y = 0; y < diameter; y++)
                {
                    int index = x * diameter + y;
                    Vector2 pos = new Vector2(x - radius, y - radius);
                    if (pos.LengthSquared() <= diamsq)
                    {
                        colorData[index] = Color.White;
                    }
                    else
                    {
                        colorData[index] = Color.Transparent;
                    }
                }
            }
            Texture.SetData(colorData);
        }

        #endregion

        #region Forces

        public override Vector2 RepelDirection(Vector2 testObject, float distance)
        {
            Vector2 direction = new Vector2();  

            direction.X = Position.X - testObject.X;
            direction.Y = Position.Y - testObject.Y;
            
            return direction / distance; 
        }

        #endregion

        #region Collision

        public override bool HasCollidedSquare(float distance)
        {
            // should be able to get the radius of the thing calling this
            if (distance <= radius)
            {
                return true;
            }
            return false;
        }
        
        public override bool HasCollidedCircle(float distance, float otherRadius)
        {
            // should be able to get the radius of the thing calling this
            if (distance <= radius + otherRadius)
            {
                return true;
            }
            return false;
        }

        #endregion
    }
}
