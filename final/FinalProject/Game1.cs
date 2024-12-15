using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;

namespace SimpleGame
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;
        private SpriteBatch _spriteBatch;
        private Player _player;
        private Dart _dart;
        private Bar _bar;
        private Bomb _bomb;
        
        private Texture2D _textTexture;
        private enum GameState
        {
            Playing,
            GameOver
        }
        private GameState _currentState;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;

            _graphics.IsFullScreen = true;
            _graphics.PreferredBackBufferWidth = 1920;
            _graphics.PreferredBackBufferHeight = 1080;
        }

        protected override void Initialize()
        {
            _currentState = GameState.Playing;
            base.Initialize();
        }

        protected override void LoadContent()
        {
            _spriteBatch = new SpriteBatch(GraphicsDevice);
            
            _player = new Player(GraphicsDevice);
            _dart = new Dart(GraphicsDevice);
            _bar = new Bar(GraphicsDevice);
            _bomb = new Bomb(GraphicsDevice);

            // Create game over text texture
            _textTexture = CreateTextTexture("GAME OVER", Color.Red, 72);
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
            var keyboardState = Keyboard.GetState();

            switch (_currentState)
            {
                case GameState.Playing:
                    UpdatePlayingState(deltaTime, keyboardState);
                    break;
                case GameState.GameOver:
                    UpdateGameOverState(keyboardState);
                    break;
            }

            base.Update(gameTime);
        }

        private void UpdatePlayingState(float deltaTime, KeyboardState keyboardState)
        {
            _player.Update(deltaTime, keyboardState, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            
            _dart.Update(deltaTime, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _bar.Update(deltaTime, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            _bomb.Update(deltaTime, _graphics.PreferredBackBufferWidth, _graphics.PreferredBackBufferHeight);
            
            CheckCollisions();
        }

        private void CheckCollisions()
        {
            if (!_dart.IsActive && !_bar.IsActive && !_bomb.IsActive) return;

            Vector2 playerCenter = _player.Position;
            float playerRadius = Math.Min(_player.Texture.Width, _player.Texture.Height) / 2;

            // Check Dart Collision
            if (_dart.IsActive)
            {
                float dartRadius = Math.Min(_dart.Texture.Width, _dart.Texture.Height) / 2;
                float dartDistance = Vector2.Distance(playerCenter, _dart.Position);

                if (dartDistance < playerRadius + dartRadius)
                {
                    if (_player.IsDashing)
                    {
                        // Optional: Deflect or deactivate dart when dashing
                        _dart.IsActive = false;
                    }
                    else
                    {
                        _currentState = GameState.GameOver;
                    }
                }
            }

            // Check Bar Collision
            if (_bar.IsActive)
            {
                float barRadius = Math.Min(_bar.Texture.Width, _bar.Texture.Height) / 2;
                float barDistance = Vector2.Distance(playerCenter, _bar.Position);

                if (barDistance < playerRadius + barRadius)
                {
                    if (_player.IsDashing)
                    {
                        // Optional: Deflect or deactivate bar when dashing
                        _bar.IsActive = false;
                    }
                    else
                    {
                        _currentState = GameState.GameOver;
                    }
                }
            }

            // Check Bomb Collision
            if (_bomb.IsActive)
            {
                float bombRadius = Math.Min(_bomb.Texture.Width, _bomb.Texture.Height) / 2;
                float bombDistance = Vector2.Distance(playerCenter, _bomb.Position);

                if (bombDistance < playerRadius + bombRadius)
                {
                    if (_player.IsDashing)
                    {
                        // Optional: Deflect or deactivate bomb when dashing
                        _bomb.IsActive = false;
                    }
                    else if (_bomb.CheckExplosion(playerCenter))
                    {
                        _currentState = GameState.GameOver;
                    }
                }
            }
        }

        private void UpdateGameOverState(KeyboardState keyboardState)
        {
            if (keyboardState.IsKeyDown(Keys.R))
            {
                RestartGame();
            }
        }

        private void RestartGame()
        {
            _player.ResetPosition();
            _dart = new Dart(GraphicsDevice);
            _bar = new Bar(GraphicsDevice);
            _bomb = new Bomb(GraphicsDevice);
            _currentState = GameState.Playing;
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            _spriteBatch.Begin();

            switch (_currentState)
            {
                case GameState.Playing:
                    DrawPlayingState();
                    break;
                case GameState.GameOver:
                    DrawGameOverState();
                    break;
            }

            _spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawPlayingState()
        {
            _player.Draw(_spriteBatch);
            _dart.Draw(_spriteBatch);
            _bar.Draw(_spriteBatch);
            _bomb.Draw(_spriteBatch);
        }

        private void DrawGameOverState()
        {
            // Draw the game over text (same as original implementation)
            Vector2 screenCenter = new Vector2(_graphics.PreferredBackBufferWidth / 2, _graphics.PreferredBackBufferHeight / 2);
            Vector2 textPosition = new Vector2(screenCenter.X - _textTexture.Width / 2, screenCenter.Y - _textTexture.Height / 2);
            
            _spriteBatch.Draw(_textTexture, textPosition, Color.White);

            // Optional: Add a simple "Press R to Restart" text
            Texture2D restartTexture = CreateTextTexture("PRESS R TO RESTART", Color.White, 36);
            Vector2 restartPosition = new Vector2(
                screenCenter.X - restartTexture.Width / 2, 
                screenCenter.Y + _textTexture.Height
            );
            _spriteBatch.Draw(restartTexture, restartPosition, Color.White);
        }


        private Texture2D CreateTextTexture(string text, Color color, int fontSize)
        {
                        // Create a bitmap font manually
            int width = text.Length * (fontSize / 2);
            int height = fontSize;

            Texture2D texture = new Texture2D(GraphicsDevice, width, height);
            Color[] colorData = new Color[width * height];

            // Simple bitmap font rendering
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                // Character rendering
                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < (fontSize / 2); x++)
                    {
                        bool drawPixel = RenderCharacter(c, x, y, fontSize);
                        if (drawPixel)
                        {
                            int index = y * width + (i * (fontSize / 2) + x);
                            if (index >= 0 && index < colorData.Length)
                            {
                                colorData[index] = color;
                            }
                        }
                    }
                }
            }

            texture.SetData(colorData);
            return texture;
        }

        private bool RenderCharacter(char c, int x, int y, int fontSize)
        {
            switch (c)
            {
                case 'G':
                    return (x == 0 || x == fontSize / 2 - 1 || y == 0 || y == fontSize - 1);
                case 'A':
                    return (x == 0 || x == fontSize / 2 - 1 || y == 0);
                case 'M':
                    return (x == 0 || x == fontSize / 2 - 1 || (x == fontSize / 4 && y < fontSize / 2));
                case 'E':
                    return (x == 0 || y == 0 || y == fontSize / 2 || y == fontSize - 1);
                case 'O':
                    return (x == 0 || x == fontSize / 2 - 1 || y == 0 || y == fontSize - 1);
                case 'V':
                    return (x == 0 && y < fontSize / 2) || (x == fontSize / 2 - 1 && y < fontSize / 2);
                case 'R':
                    return (x == 0 || x == fontSize / 2 - 1 || y == 0 || (x == 0 && y > fontSize / 2));
                default:
                    return false;
            }
        }
    }
}