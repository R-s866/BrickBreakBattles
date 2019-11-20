using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBallBattles
{
    class GUIElement
    {
        public Vector2 Position { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }
        public Texture2D Texture { get; set; }

        public GUIElement(Vector2 position, int width, int Height)
        {
            this.Position = position;
            this.Width = width;
            this.Height = Height;
        }

        public virtual void CreateTexture(GraphicsDevice graphics)
        {

        }
    }
}
