using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PinBallBattles
{
    class Button : GUIElement
    {
        public Button(Vector2 position, int width, int height) 
            : base(position, width, height)
        {

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
                    colorData[index] = Color.DarkGoldenrod;
                    index++;
                }
            }

            Texture.SetData(colorData);

            base.CreateTexture(graphics);
        }

    }
}
