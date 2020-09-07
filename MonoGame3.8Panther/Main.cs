using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Panther;
using System;

namespace MonoGame38Test
{
    public class Main : Game
    {
        public static GameLogic instance;
        GraphicsDeviceManager _graphics;
        SpriteBatch _spriteBatch;
        GameLogic _game;
        Camera _camera;

        Timer _FPSTimer;
        Timer _FPSDesplayTimer;
        TimeSpan elapsedTime = TimeSpan.Zero;
        float _FPSFrames = 0;
        float _FPSTotal = 0;
        float _FPSCount = 0;
        float frameRate = 0;
        float frameCounter = 0;

        public Main()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.IsFullScreen = false;
            _graphics.GraphicsProfile = GraphicsProfile.HiDef;
            _graphics.PreferMultiSampling = true;
            _graphics.ApplyChanges();
            _graphics.PreparingDeviceSettings += SetMultiSampling;
            _graphics.GraphicsDevice.RasterizerState = new RasterizerState(); //Must be after Apply Changes.
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 960;
            _graphics.SynchronizeWithVerticalRetrace = false; //When true, 60FPS refresh rate locked.
            _graphics.ApplyChanges();
            //IsFixedTimeStep = true;//When true, 60FPS refresh rate locked.
            Content.RootDirectory = "Content";
            // Positive Y is Up. Positive X is Right.
            // Create a new SpriteBatch, which can be used to draw textures.
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            Core.Initialize(this, _graphics, _spriteBatch);

            // Screen resolution is 1200 X 900.
            // Y positive is Up.
            // (X) positive is to the right when camera is at rotation zero.
            // Z positive is towards the camera when at rotation zero.
            // Rotation on object rotates CCW. Zero has front facing X positive. Pi/2 on Y faces Z negative.
            _camera = new Camera(this, new Vector3(0, 0, 50), new Vector3(0, MathHelper.Pi, 0),
                GraphicsDevice.Viewport.AspectRatio, 1f, 1000f);

            _FPSTimer = new Timer(this, 1);
            _FPSDesplayTimer = new Timer(this, 30);
            _game = new GameLogic(this, _camera);
            IsFixedTimeStep = false;
        }

        private void SetMultiSampling(object sender, PreparingDeviceSettingsEventArgs eventArgs)
        {
            PresentationParameters PresentParm = eventArgs.GraphicsDeviceInformation.PresentationParameters;
            PresentParm.MultiSampleCount = 8;
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            if (instance == null)
            {
                instance = _game;
            }

            // Window Title
            Window.Title = "MonoGame 3.8 Panther Engine Version Alpha 1"; // Has to be in Initialize.
            // Setup lighting.
            Core.ScreenHeight = (uint)_graphics.PreferredBackBufferHeight;
            Core.ScreenWidth = (uint)_graphics.PreferredBackBufferWidth;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();
            _game.LoadContent();
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// game-specific content.
        /// </summary>
        protected override void UnloadContent()
        {
            base.UnloadContent();
            _game.UnloadContent();
        }

        protected override void BeginRun()
        {
            base.BeginRun();
            _game.BeginRun();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed ||
                Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            if (_game.CurrentMode != GameState.Pause)
            {
                base.Update(gameTime);
            }
            else
            {
                _game.GetKeys();
            }

            Core.UpdateKeys();

            elapsedTime += gameTime.ElapsedGameTime;

            _FPSFrames++;

            if (elapsedTime > TimeSpan.FromSeconds(1))
            {
                elapsedTime -= TimeSpan.FromSeconds(1);
                frameRate = frameCounter;
                frameCounter = 0;
            }


            if (_FPSTimer.Elapsed)
            {
                _FPSTimer.Reset();
                _FPSTotal += _FPSFrames;
                _FPSCount++;
                float average = _FPSTotal / _FPSCount;

                if (_FPSDesplayTimer.Elapsed)
                {
                    _FPSDesplayTimer.Reset();
                    System.Diagnostics.Debug.WriteLine("FPS " + _FPSFrames.ToString() + " Average " +
                        average.ToString());

                    System.Diagnostics.Debug.WriteLine("Real FPS " + frameRate);
                }

                _FPSFrames = 0;
            }
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            frameCounter++;

            GraphicsDevice.Clear(new Color(0.05f, 0, 0.1f));
            base.Draw(gameTime);
            _game.Draw();
        }
    }
}
