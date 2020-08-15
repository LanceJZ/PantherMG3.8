using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using System.Collections.Generic;
using System.Linq;
using System;
using Panther;

public enum GameState
{
    Over,
    InPlay,
    HighScore,
    MainMenu
};

namespace MonoGame38Test
{
    public class GameLogic : GameComponent
    {
        NumberGenerator _score;
        TextGrid _text;
        Camera _camera;
        List<ShapeGenerater> _cubes;
        ShapeGenerater _cube;
        ShapeGenerater _cubeSub;
        ShapeGenerater _cubeSubSub;
        ModelEntity _box;

        GameState _gameMode = GameState.MainMenu;
        KeyboardState _oldKeyState;

        public GameState CurrentMode { get => _gameMode; }

        public GameLogic(Game game, Camera camera) : base(game)
        {
            VertexPositionNormalTexture[] genMLine = GeneratedText.MiddleLine(2.0f);

            _camera = camera;
            int cubePrims = 12;
            VertexPositionNormalTexture[] genCube = GeneratedShapes.Cube();
            _box = new ModelEntity(game, camera, "Core/Cube");
            _cubes = new List<ShapeGenerater>();
            _cube = new ShapeGenerater(game, camera, genCube, cubePrims);
            _cubeSub = new ShapeGenerater(game, camera, genCube, cubePrims);
            _cubeSubSub = new ShapeGenerater(game, camera, genCube, cubePrims);

            _text = new TextGrid(game, camera, 1.0f, Vector3.One);

            _score = new NumberGenerator(game);

            for (int i = 0; i < 1000; i++)
            {
                _cubes.Add(new ShapeGenerater(Game, _camera, genCube, cubePrims));
            }

            game.Components.Add(this);
        }

        public override void Initialize()
        {
            base.Initialize();

            _score.Position.Y = 300;
            _score.Position.X = -125;
        }

        public void BeginRun()
        {
            _cube.PO.RotationVelocity = new Vector3(0, 0.1f, 1.1f);
            _cube.PO.Position.X = -15;
            _cube.DiffuseColor = new Vector3(0.3f, 0, 0.85f);
            _cubeSub.AddAsChildOf(_cube);
            _cubeSub.PO.Position.X = 6.15f;
            _cubeSub.PO.RotationVelocity.Y = 1.25f;
            _cubeSub.TheScale = new Vector3(0.5f);
            _cubeSub.DiffuseColor = new Vector3(0.4f, 0, 0.95f);
            _cubeSubSub.AddAsChildOf(_cubeSub);
            _cubeSubSub.PO.Position.X = 3.25f;
            _cubeSubSub.PO.RotationVelocity.X = 1.5f;
            _cubeSubSub.TheScale = new Vector3(0.25f);
            _cubeSubSub.DiffuseColor = new Vector3(0.2f, 0, 0.6f);

            _score.Number = 125027849061203;
            //_score.RotationVelocity.X = 1;
            //_score.SetRotationVolicity();
            //_score.ChangeNumber(666);

            _box.PO.Position.Y = -5;
            _box.PO.Z = -11;

            foreach(ShapeGenerater cube in _cubes)
            {
                cube.Position = new Vector3(Helper.RandomMinMax(-250, 250), Helper.RandomMinMax(-190, 190), Helper.RandomMinMax(-350, -550));
                cube.PO.RotationVelocity = new Vector3(Helper.RandomMinMax(-2, 2), Helper.RandomMinMax(-2, 2), Helper.RandomMinMax(-2, 2));
                cube.DiffuseColor = new Vector3(Helper.RandomMinMax(0.05f, 0.95f), Helper.RandomMinMax(0.05f, 0.95f), Helper.RandomMinMax(0.05f, 0.95f));
            }
        }

        public override void Update(GameTime gameTime)
        {
            KeyboardState KBS = Keyboard.GetState();

            if (KBS != _oldKeyState)
            {
                if (KBS.IsKeyDown(Keys.Space))
                {
                    //_boxs[0].Enabled = !_boxs[0].Enabled;
                }
            }

            _oldKeyState = Keyboard.GetState();

            if (KBS.IsKeyDown(Keys.Up))
            {
                _cube.PO.Velocity.Y += 1;
            }
            else if (KBS.IsKeyDown(Keys.Down))
            {
                _cube.PO.Velocity.Y -= 1;
            }
            else
            {
                _cube.PO.Velocity.Y = 0;
            }

            if (KBS.IsKeyDown(Keys.Left))
            {
                _cube.PO.Velocity.X -= 1;
            }
            else if (KBS.IsKeyDown(Keys.Right))
            {
                _cube.PO.Velocity.X += 1;
            }
            else
            {
                _cube.PO.Velocity.X = 0;
            }

            base.Update(gameTime);
        }
    }
}
