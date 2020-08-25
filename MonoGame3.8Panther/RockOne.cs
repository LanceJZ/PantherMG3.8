using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Panther;

namespace MonoGame38Test
{
    public class RockOne : Vector
    {
        public RockOne(Game game, Camera camera) : base (game, camera)
        {
            PO.Radius = InitializePoints(ReadFile("RockOne"), new Color(170, 170, 255));

        }

        public override void Initialize()
        {
            base.Initialize();


        }
    }
}
