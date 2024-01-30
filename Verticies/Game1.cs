using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Verticies
{
    public class Game1 : Game
    {
        private GraphicsDeviceManager _graphics;

        Matrix view;
        Matrix projection;

        VertexPositionTexture[] colorvertices;
        BasicEffect colorEffects;
        Matrix colorWorld;
        short[] indices;
        VertexBuffer vertexBuffer;
        IndexBuffer indexBuffer;

        public Game1()
        {
            _graphics = new GraphicsDeviceManager(this);
            _graphics.PreferredBackBufferWidth = 1280;
            _graphics.PreferredBackBufferHeight = 720;
            _graphics.SynchronizeWithVerticalRetrace = true;
            _graphics.ApplyChanges();

            Content.RootDirectory = "Content";
            IsMouseVisible = true;
        }

        protected override void Initialize()
        {
            //Create Camera
            view = Matrix.CreateLookAt
                (new Vector3(0, 0, 1),
                 new Vector3(0, 0, -1),
                 Vector3.Up);

            projection = Matrix.CreatePerspectiveFieldOfView
                (
                 MathHelper.ToRadians(90),
                 GraphicsDevice.Adapter.CurrentDisplayMode.AspectRatio,
                 0.1f,
                 1000f);

            CreateColorVertices();

            base.Initialize();
        }

        void CreateColorVertices()
        {
            colorvertices = new VertexPositionTexture[4];

            colorvertices[0] = new VertexPositionTexture(
                new Vector3(1, 0, 0),
                new Vector2(1, 1));
            colorvertices[1] = new VertexPositionTexture(
                new Vector3(-1, 0, 0),
                new Vector2(0, 1));
            colorvertices[2] = new VertexPositionTexture(
                new Vector3(-1, 2, 0),
                new Vector2(0, 0));

            colorvertices[3] = new VertexPositionTexture(
                new Vector3(1, 2, 0),
                new Vector2(1, 0));

            indices = new short[6];

            //tringle1
            indices[0] = 0;
            indices[1] = 1;
            indices[2] = 2;
            //triangle2
            indices[3] = 0;
            indices[4] = 2;
            indices[5] = 3;


            vertexBuffer = new VertexBuffer(GraphicsDevice, VertexPositionColorTexture.VertexDeclaration, colorvertices.Length, BufferUsage.WriteOnly);
            indexBuffer = new IndexBuffer(GraphicsDevice, IndexElementSize.SixteenBits, indices.Length, BufferUsage.WriteOnly);

            vertexBuffer.SetData(colorvertices);
            indexBuffer.SetData(indices);

            colorEffects = new BasicEffect(GraphicsDevice);
            colorEffects.TextureEnabled = true;
            colorEffects.VertexColorEnabled = false;
            colorEffects.Texture = Content.Load<Texture2D>("uv_texture");

            colorWorld = Matrix.Identity * Matrix.CreateTranslation(0, 0, -2);
        }

        void DrawColorVertices()
        {
            colorEffects.View = view;
            colorEffects.Projection = projection;
            colorEffects.World = colorWorld;

            GraphicsDevice.SetVertexBuffer(vertexBuffer);
            GraphicsDevice.Indices = indexBuffer;

            foreach (EffectPass pass in colorEffects.CurrentTechnique.Passes)
            {
                pass.Apply();

                GraphicsDevice.DrawUserIndexedPrimitives<VertexPositionTexture>
                     (PrimitiveType.TriangleList,
                     colorvertices,
                     0,
                     colorvertices.Length,
                     indices,
                     0,
                     indices.Length / 3);
            }
        }

        protected override void LoadContent()
        {
            // TODO: use this.Content to load your game content here
        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();
            

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            DrawColorVertices();

            base.Draw(gameTime);
        }
    }
}
