using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Xna.Framework;
using Panther;

namespace MonoGame38Test
{
    public class PlayerShip : Vector
    {
        Vector ship;


        public PlayerShip(Game game, Camera camera) : base (game, camera)
        {
            PO.Radius = InitializePoints(ReadFile("PlayerShip"), new Color(200, 200, 255));
            ship = new Vector(game, camera);
        }

        public override void Initialize()
        {
            base.Initialize();

            ship.InitializePoints(ship.ReadFile("PlayerShip"), new Color(170, 170, 255));
            ship.Position = new Vector3(-20, 17, 0);
            ship.Rotation = new Vector3(0, 0, (float)Math.PI / 2);

            //float multi = 2.666f;
            //float shipTip = 0.25f * multi; //0.666
            //float shipBWidth = 0.15f * multi; //0.4
            //float shipMid = -0.16f * multi; //-0.427
            //float shipMidWidth = 0.125f * multi; //0.333



            //Vector3[] pointPosition = new Vector3[6];

            //pointPosition[0] = new Vector3(-shipTip, shipBWidth, 0);//Top back tip.
            //pointPosition[1] = new Vector3(shipTip, 0, 0);//Nose pointing to the left of screen.
            //pointPosition[2] = new Vector3(-shipTip, -shipBWidth, 0);//Bottom back tip.
            //pointPosition[3] = new Vector3(shipMid, -shipMidWidth, 0);//Bottom inside back.
            //pointPosition[4] = new Vector3(shipMid, shipMidWidth, 0);//Top inside back.
            //pointPosition[5] = new Vector3(-shipTip, shipBWidth, 0);//Top Back Tip.

            //PO.Radius = InitializePoints(pointPosition);
            //WriteFile(pointPosition, "PlayerShip");
        }

        
    }
}
