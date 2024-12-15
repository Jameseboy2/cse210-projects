using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;

namespace SimpleGame
{
    public class Player
    {
        private Texture2D _texture;
        private Texture2D _outlineTexture;
        
        private Vector2 _position;
        public Vector2 Position 
        { 
            get { return _position; } 
            set { _position = value; }
        }

        public Vector2 Velocity { get; private set; }
        
        private float _maxSpeed;
        private float _acceleration;
        private float _friction;
        
        // Dash-related properties
        private bool _isDashing;
        private float _dashDuration;
        private float _dashTimer;
        private float _dashCooldown;
        private float _cooldownTimer;
        private bool _isCooldown;
        
        public float Rotation { get; private set; }

        public Player(GraphicsDevice graphicsDevice)
        {
            _maxSpeed = 500f;
            _acceleration = 20000f;
            _friction = 0.2f;
            _dashDuration = 0.2f;
            _dashCooldown = 0.2f;

            InitializeTextures(graphicsDevice);
            ResetPosition();
        }

        private void InitializeTextures(GraphicsDevice graphicsDevice)
        {
            // Player textures
            _texture = new Texture2D(graphicsDevice, 50, 50);
            Color[] data = new Color[50 * 50];
            for (int i = 0; i < data.Length; ++i) data[i] = Color.Blue;
            _texture.SetData(data);

            _outlineTexture = new Texture2D(graphicsDevice, 54, 54);
            Color[] outlineData = new Color[54 * 54];
            for (int i = 0; i < outlineData.Length; ++i) outlineData[i] = Color.Black;
            _outlineTexture.SetData(outlineData);
        }

        public void Update(float deltaTime, KeyboardState keyboardState, int screenWidth, int screenHeight)
        {
            HandleDashing(deltaTime, keyboardState);
            HandleMovement(deltaTime, keyboardState, screenWidth, screenHeight);
        }

        private void HandleDashing(float deltaTime, KeyboardState keyboardState)
        {
            // Dash logic remains the same as in the original implementation
            if (keyboardState.IsKeyDown(Keys.Space) && !_isDashing && !_isCooldown)
            {
                _isDashing = true;
                _dashTimer = _dashDuration;
            }

            if (_isDashing)
            {
                _dashTimer -= deltaTime;
                if (_dashTimer <= 0)
                {
                    _isDashing = false;
                    _isCooldown = true;
                    _cooldownTimer = _dashCooldown;
                }
            }

            if (_isCooldown)
            {
                _cooldownTimer -= deltaTime;
                if (_cooldownTimer <= 0)
                {
                    _isCooldown = false;
                }
            }
        }

        private void HandleMovement(float deltaTime, KeyboardState keyboardState, int screenWidth, int screenHeight)
        {
            Vector2 input = Vector2.Zero;

            if (keyboardState.IsKeyDown(Keys.Up))
                input.Y -= 1;
            if (keyboardState.IsKeyDown(Keys.Down))
                input.Y += 1;
            if (keyboardState.IsKeyDown(Keys.Left))
                input.X -= 1;
            if (keyboardState.IsKeyDown(Keys.Right))
                input.X += 1;

            if (input != Vector2.Zero)
            {
                input.Normalize();
                
                Velocity += input * _acceleration * deltaTime;
                Rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);
            }

            float currentMaxSpeed = _isDashing ? _maxSpeed * 10 : _maxSpeed;
            
            if (Velocity.Length() > currentMaxSpeed)
            {
                Velocity = Vector2.Normalize(Velocity) * currentMaxSpeed;
            }

            if (input == Vector2.Zero)
            {
                Velocity *= (float)Math.Pow(_friction, deltaTime * 10);
            }

            _position += Velocity * deltaTime;

            // Clamp position to screen
            _position.X = MathHelper.Clamp(_position.X, 0, screenWidth - _texture.Width);
            _position.Y = MathHelper.Clamp(_position.Y, 0, screenHeight - _texture.Height);
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            spriteBatch.Draw(_outlineTexture, _position - new Vector2(2, 2), null, Color.White, Rotation, 
                new Vector2(_outlineTexture.Width / 2, _outlineTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_texture, _position, null, Color.White, Rotation, 
                new Vector2(_texture.Width / 2, _texture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public void ResetPosition()
        {
            _position = new Vector2(430, 520);
            Velocity = Vector2.Zero;
            _isDashing = false;
            _dashTimer = 0f;
            _cooldownTimer = 0f;
            _isCooldown = false;
            Rotation = 0f;
        }

        public Texture2D Texture => _texture;
        public bool IsDashing => _isDashing;
    }
}