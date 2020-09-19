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
    Pause,
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
        PlayerShip player;
        RockOne rockOne;
        RockOne rockTwo;
        RockOne rockThree;
        VectorModel cross;
        SpriteFont hyper20Font;

        GameState _gameMode = GameState.InPlay;

        public GameState CurrentMode { get => _gameMode; }

        public GameLogic(Game game) : base(game)
        {
            // Screen resolution is 1200 X 900.
            // Y positive is Up.
            // (X) positive is to the right when camera is at rotation zero.
            // Z positive is towards the camera when at rotation zero.
            // Rotation on object rotates CCW. Zero has front facing X positive. Pi/2 on Y faces Z negative.
            _camera = new Camera(game, new Vector3(0, 0, 50), new Vector3(0, MathHelper.Pi, 0),
                Core.Graphics.Viewport.AspectRatio, 1f, 1000f);

            VertexPositionNormalTexture[] genMLine = GeneratedText.MiddleLine(2.0f);

            int cubePrims = 12;
            VertexPositionNormalTexture[] genCube = GeneratedShapes.Cube();
            _box = new ModelEntity(game, _camera, "Core/Cube");
            _cubes = new List<ShapeGenerater>();
            _cube = new ShapeGenerater(game, _camera, genCube, cubePrims);
            _cubeSub = new ShapeGenerater(game, _camera, genCube, cubePrims);
            _cubeSubSub = new ShapeGenerater(game, _camera, genCube, cubePrims);

            _text = new TextGrid(game, _camera, 0.10f, Vector3.One);

            _score = new NumberGenerator(game);

            for (int i = 0; i < 1000; i++)
            {
                _cubes.Add(new ShapeGenerater(Game, _camera, genCube, cubePrims));
            }

            player = new PlayerShip(Game, _camera);
            rockOne = new RockOne(Game, _camera);
            rockTwo = new RockOne(Game, _camera);
            rockThree = new RockOne(Game, _camera);
            cross = new VectorModel(Game, _camera);

            game.Components.Add(this);
        }
        #region Public Methods
        public override void Initialize()
        {
            base.Initialize();

            _score.Position.Y = 300;
            _score.Position.X = -125;

            _text.Position.Y = 5;

            float crossSize = 0.5f;
            Vector3[] crossVertex = { new Vector3(crossSize, 0, 0), new Vector3(-crossSize, 0, 0),
                new Vector3(0, crossSize, 0), new Vector3(0, -crossSize, 0) };
            cross.InitializePoints(crossVertex, Color.White, "Cross");
        }

        public void LoadContent()
        {
            hyper20Font = Game.Content.Load<SpriteFont>("Hyperspace20");
        }

        public void UnloadContent()
        {

        }

        public void BeginRun()
        {
            _cube.PO.RotationVelocity = new Vector3(0, 0.1f, 1.1f);
            _cube.PO.Position.X = -15;
            _cube.DiffuseColor = new Vector3(0.3f, 0, 0.85f);
            _cubeSub.AddAsChildOf(_cube);
            _cubeSub.PO.Position = new Vector3(6.15f, 0, 0);
            _cubeSub.PO.RotationVelocity.Y = 1.25f;
            _cubeSub.Scale = 0.5f;
            _cubeSub.DiffuseColor = new Vector3(0.4f, 0, 0.95f);
            _cubeSubSub.AddAsChildOf(_cubeSub);
            _cubeSubSub.PO.Position = new Vector3(3.25f, 0, 0);
            _cubeSubSub.PO.RotationVelocity.X = 1.5f;
            _cubeSubSub.Scale = 0.25f;
            _cubeSubSub.DiffuseColor = new Vector3(0.2f, 0, 0.6f);

            _score.Number = 125027849061203;
            //_score.RotationVelocity.X = 1;
            //_score.SetRotationVolicity();
            //_score.ChangeNumber(666);

            _box.PO.Position.Y = -5;
            _box.PO.Z = -11;

            foreach(ShapeGenerater cube in _cubes)
            {
                cube.Position = new Vector3(Core.RandomMinMax(-250, 250),
                    Core.RandomMinMax(-190, 190), Core.RandomMinMax(-350, -550));
                cube.PO.RotationVelocity = new Vector3(Core.RandomMinMax(-2, 2),
                    Core.RandomMinMax(-2, 2), Core.RandomMinMax(-2, 2));
                cube.DiffuseColor = new Vector3(Core.RandomMinMax(0.05f, 0.95f),
                    Core.RandomMinMax(0.05f, 0.95f), Core.RandomMinMax(0.05f, 0.95f));
            }

            player.RotationVelocity = new Vector3(0.5f, 0.1f, 0.25f);
            player.Position = new Vector3(20, 0, 0);
            //player.DiffuseColor = new Vector3(0.25f, 0.05f, 1.0f);
            rockOne.RotationVelocity = new Vector3(0, 0, -0.1f);
            rockOne.Position = new Vector3(10, 8, 0);
            rockTwo.AddAsChildOf(rockOne);
            rockTwo.RotationVelocity = new Vector3(0, 0, .25f);
            rockTwo.Scale = 0.5f;
            rockTwo.Position = new Vector3(6.5f, 6.5f, 0);
            rockThree.AddAsChildOf(rockTwo);
            rockThree.Scale = 0.25f;
            rockThree.Position = new Vector3(2.25f, 2.25f, 0);

            cross.Enabled = false;
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            GetKeys();
        }

        public void Draw()
        {
            Core.SpriteBatch.Begin();
            Core.SpriteBatch.DrawString(hyper20Font, "Test", new Vector2(100, 100), Color.White);
            Core.SpriteBatch.End();
        }
                
        public void GetKeys()
        {
            if (Core.KeyPressed(Keys.Space))
            {
                _cubeSubSub.Enabled = !_cubeSubSub.Enabled;
                _cubeSubSub.UpdateMatrix();
            }

            if (Core.KeyPressed(Keys.Enter))
            {
                if (cross.Enabled)
                {
                    System.Diagnostics.Debug.WriteLine("X: " + cross.X.ToString() +
                        " " + "Y: " + cross.Y.ToString());
                }
            }

            if (Core.KeyPressed(Keys.End))
            {
                cross.Enabled = !cross.Enabled;
                cross.Position = Vector3.Zero;
            }

            if (Core.KeyPressed(Keys.Pause))
            {
                if (CurrentMode == GameState.InPlay)
                {
                    _gameMode = GameState.Pause;
                }
                else if (CurrentMode == GameState.Pause)
                {
                    _gameMode = GameState.InPlay;
                }
            }


            if (cross.Enabled)
            {
                if (Core.KeyDown(Keys.W))
                {
                    cross.PO.Velocity.Y += 0.125f;
                }
                else if (Core.KeyDown(Keys.S))
                {
                    cross.PO.Velocity.Y -= 0.125f;
                }
                else
                {
                    cross.PO.Velocity.Y = 0;
                }

                if (Core.KeyDown(Keys.D))
                {
                    cross.PO.Velocity.X += 0.125f;
                }
                else if (Core.KeyDown(Keys.A))
                {
                    cross.PO.Velocity.X -= 0.125f;
                }
                else
                {
                    cross.PO.Velocity.X = 0;
                }
            }

            if (Core.KeyDown(Keys.Up))
            {
                _cube.PO.Velocity.Y += 1;
            }
            else if (Core.KeyDown(Keys.Down))
            {
                _cube.PO.Velocity.Y -= 1;
            }
            else
            {
                _cube.PO.Velocity.Y = 0;
            }

            if (Core.KeyDown(Keys.Left))
            {
                _cube.PO.Velocity.X -= 1;
            }
            else if (Core.KeyDown(Keys.Right))
            {
                _cube.PO.Velocity.X += 1;
            }
            else
            {
                _cube.PO.Velocity.X = 0;
            }
        }
        #endregion
        #region Private Methods
        #endregion
    }
}
