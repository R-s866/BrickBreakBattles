using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

public enum Shape{
    Circle, Square
}

namespace PinBallBattles
{
    class CollisionObject
    {
        private Shape shape;
        private Vector2 position;
        private Texture2D texture;
        private bool isPlayer;

        private double cooldownTimer = 0;

        public Shape Shape { get => shape; }
        public Vector2 Position { get => position; set => position = value; }
        public Texture2D Texture { get => texture; set => texture = value; }
        public bool IsPlayer { get => isPlayer; }

        #region Create

        public CollisionObject(Shape shape, Vector2 position, bool isPlayer)
        {
            this.shape = shape;
            this.position = position;
            this.isPlayer = isPlayer;
        }

        public virtual void CreateTexture(GraphicsDevice graphics)
        {

        }

        #endregion

        #region Forces

        public virtual Vector2 RepelDirection(Vector2 testObject, float distance)
        {
            return new Vector2();
        }

        #endregion

        #region Collision

        public virtual bool HasCollidedCircle(float distance, float otherRaduis)
        {
            return false;
        }

        public virtual bool HasCollidedSquare(float distance)
        {
            return false;
        }

        #endregion

        #region PositionalChecks
        // just square

        public virtual Vector2 ClosestEdge(Vector2 testObject)
        {
            return new Vector2();
        }

        // cricle + square
        public float GetDistance(Vector2 testObject)
        {
            float distX = Position.X - testObject.X;
            float distY = Position.Y - testObject.Y;

            float distance = (float)Math.Sqrt((distX * distX) + (distY * distY));

            return distance;
        }

        #endregion
        
        public Vector2 SetNormal(Vector2 velocity, float distance)
        {
            Vector2 normal = velocity / distance;
            return normal;
        }

        public bool CooldownTimer(GameTime gameTime, double duration)
        {
            cooldownTimer += gameTime.ElapsedGameTime.TotalMilliseconds;
            if (cooldownTimer >= duration)
            {
                cooldownTimer = 0;
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
