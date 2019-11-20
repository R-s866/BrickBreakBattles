using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;


namespace PinBallBattles
{
    class Brick : Square
    {
        public Brick(Shape shape, Vector2 position, int width, int height, bool isPlayer)
            : base(shape, position, width, height, isPlayer)
        {

        }
    }
}
