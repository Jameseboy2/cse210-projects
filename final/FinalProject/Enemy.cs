using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;

namespace SimpleGame
{
    public abstract class Enemy
    {
        protected Texture2D _texture;
        protected Texture2D _outlineTexture;
        
        public Vector2 Position { get; protected set; }
        public Vector2 Velocity { get; protected set; }
        public bool IsActive { get; set; }

        protected float _speed;
        protected float _respawnTimer;
        protected float _respawnDelay;
        protected Random _random;

        public Enemy(GraphicsDevice graphicsDevice)
        {
            _random = new Random();
            _respawnDelay = 2f;
            _respawnTimer = _respawnDelay;
        }

        public virtual void Update(float deltaTime, int screenWidth, int screenHeight)
        {
            if (!IsActive)
            {
                _respawnTimer -= deltaTime;
                if (_respawnTimer <= 0)
                {
                    Spawn(screenWidth, screenHeight);
                }
                return;
            }

            Position += Velocity * deltaTime;

            // Check if enemy is out of screen
            if (Position.X < 0 || Position.X > screenWidth ||
                Position.Y < 0 || Position.Y > screenHeight)
            {
                IsActive = false;
                _respawnTimer = _respawnDelay;
            }
        }

        protected virtual void Spawn(int screenWidth, int screenHeight)
        {
            int spawnEdge = _random.Next(4);

            switch (spawnEdge)
            {
                case 0: // Top edge
                    Position = new Vector2(_random.Next(screenWidth), 0);
                    break;
                case 1: // Right edge
                    Position = new Vector2(screenWidth, _random.Next(screenHeight));
                    break;
                case 2: // Bottom edge
                    Position = new Vector2(_random.Next(screenWidth), screenHeight);
                    break;
                case 3: // Left edge
                    Position = new Vector2(0, _random.Next(screenHeight));
                    break;
            }

            IsActive = true;
        }

        public virtual void Draw(SpriteBatch spriteBatch)
        {
            if (!IsActive) return;

            float rotation = (float)Math.Atan2(Velocity.Y, Velocity.X);

            spriteBatch.Draw(_outlineTexture, Position, null, Color.White, rotation, 
                new Vector2(_outlineTexture.Width / 2, _outlineTexture.Height / 2), 1f, SpriteEffects.None, 0f);
            spriteBatch.Draw(_texture, Position, null, Color.White, rotation, 
                new Vector2(_texture.Width / 2, _texture.Height / 2), 1f, SpriteEffects.None, 0f);
        }

        public Texture2D Texture => _texture;
    }

    // Dart enemy
    public class Dart : Enemy
    {
        public Dart(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            _speed = 800f;
            InitializeTextures(graphicsDevice);
        }

        private void InitializeTextures(GraphicsDevice graphicsDevice)
        {
            _texture = new Texture2D(graphicsDevice, 30, 10);
            Color[] dartData = new Color[30 * 10];
            for (int i = 0; i < dartData.Length; ++i) dartData[i] = Color.Red;
            _texture.SetData(dartData);

            _outlineTexture = new Texture2D(graphicsDevice, 34, 14);
            Color[] dartOutlineData = new Color[34 * 14];
            for (int i = 0; i < dartOutlineData.Length; ++i) dartOutlineData[i] = Color.Black;
            _outlineTexture.SetData(dartOutlineData);
        }

        protected override void Spawn(int screenWidth, int screenHeight)
        {
            base.Spawn(screenWidth, screenHeight);
            Velocity = GenerateRandomDirection(screenWidth, screenHeight);
        }

        private Vector2 GenerateRandomDirection(int screenWidth, int screenHeight)
        {
            Vector2 centerScreen = new Vector2(screenWidth / 2, screenHeight / 2);
            Vector2 direction = centerScreen - Position;
            
            float angle = (float)(_random.NextDouble() * Math.PI / 4 - Math.PI / 8);
            direction = Vector2.Transform(direction, Matrix.CreateRotationZ(angle));
            
            return Vector2.Normalize(direction) * _speed;
        }
    }

    // Bar enemy
    public class Bar : Enemy
    {
        public Bar(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            _speed = 400f;
            InitializeTextures(graphicsDevice);
        }

        private void InitializeTextures(GraphicsDevice graphicsDevice)
        {
            _texture = new Texture2D(graphicsDevice, 100, 20);
            Color[] barData = new Color[100 * 20];
            for (int i = 0; i < barData.Length; ++i) barData[i] = Color.Green;
            _texture.SetData(barData);

            _outlineTexture = new Texture2D(graphicsDevice, 104, 24);
            Color[] barOutlineData = new Color[104 * 24];
            for (int i = 0; i < barOutlineData.Length; ++i) barOutlineData[i] = Color.Black;
            _outlineTexture.SetData(barOutlineData);
        }

        protected override void Spawn(int screenWidth, int screenHeight)
        {
            base.Spawn(screenWidth, screenHeight);
            Velocity = GenerateLinearPath(screenWidth, screenHeight);
        }

        private Vector2 GenerateLinearPath(int screenWidth, int screenHeight)
        {
            Vector2 centerScreen = new Vector2(screenWidth / 2, screenHeight / 2);
            Vector2 direction = centerScreen - Position;
            
            return Vector2.Normalize(direction) * _speed;
        }
    }

    // Bomb enemy
    public class Bomb : Enemy
    {
        private float _explosionRadius;

        public Bomb(GraphicsDevice graphicsDevice) : base(graphicsDevice)
        {
            _speed = 200f;
            _explosionRadius = 100f;
            InitializeTextures(graphicsDevice);
        }

        private void InitializeTextures(GraphicsDevice graphicsDevice)
        {
            _texture = new Texture2D(graphicsDevice, 40, 40);
            Color[] bombData = new Color[40 * 40];
            for (int i = 0; i < bombData.Length; ++i) bombData[i] = Color.Purple;
            _texture.SetData(bombData);

            _outlineTexture = new Texture2D(graphicsDevice, 44, 44);
            Color[] bombOutlineData = new Color[44 * 44];
            for (int i = 0; i < bombOutlineData.Length; ++i) bombOutlineData[i] = Color.Black;
            _outlineTexture.SetData(bombOutlineData);
        }

        protected override void Spawn(int screenWidth, int screenHeight)
        {
            base.Spawn(screenWidth, screenHeight);
            Velocity = GenerateCurvedPath(screenWidth, screenHeight);
        }

        private Vector2 GenerateCurvedPath(int screenWidth, int screenHeight)
        {
            Vector2 centerScreen = new Vector2(screenWidth / 2, screenHeight / 2);
            Vector2 direction = centerScreen - Position;
            
            // Add some randomness to the path
            float angle = (float)(_random.NextDouble() * Math.PI / 2 - Math.PI / 4);
            direction = Vector2.Transform(direction, Matrix.CreateRotationZ(angle));
            
            return Vector2.Normalize(direction) * _speed;
        }

        // Optional method to handle explosion logic
        public bool CheckExplosion(Vector2 playerPosition)
        {
            float distance = Vector2.Distance(Position, playerPosition);
            return distance <= _explosionRadius;
        }
    }
}