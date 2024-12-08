using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace SimpleGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Texture2D _squareTexture;
        private Vector2 _position;
        private float _speed;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.IsFullScreen = true;
        }

        protected override void Initialize()
        {
            _position = new Vector2(100, 100);
            _speed = 100f;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);

            // Create a 50x50 square texture
            _squareTexture = new Texture2D(GraphicsDevice, 50, 50);
            Color[] data = new Color[50 * 50];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Blue;
            _squareTexture.SetData(data);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            var keyboardState = Keyboard.GetState();
            if (keyboardState.IsKeyDown(Keys.Up))
                _position.Y -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Down))
                _position.Y += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Left))
                _position.X -= _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (keyboardState.IsKeyDown(Keys.Right))
                _position.X += _speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();
            _spriteBatch.Draw(_squareTexture, _position, Color.White);
            _spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
