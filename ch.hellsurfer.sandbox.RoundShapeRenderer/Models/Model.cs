using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public abstract class Model<T> where T: struct, IVertexType
    {
        internal List<T> Vertices = new List<T>(); 
        internal List<short> Indices = new List<short>();

        private VertexBuffer vBuffer;
        private IndexBuffer iBuffer;

        protected void SetUpIndexed(Game game)
        {
            vBuffer = new VertexBuffer(game.GraphicsDevice, typeof(T), Vertices.Count, BufferUsage.WriteOnly);
            iBuffer = new IndexBuffer(game.GraphicsDevice, typeof(short), Indices.Count, BufferUsage.WriteOnly);

            vBuffer.SetData(Vertices.ToArray());
            iBuffer.SetData(Indices.ToArray());
        }

        protected void SetUp(Game game)
        {
            vBuffer = new VertexBuffer(game.GraphicsDevice, typeof(T), Vertices.Count, BufferUsage.WriteOnly);

            vBuffer.SetData(Vertices.ToArray());
        }

        protected virtual void Render(BasicEffect effect, GraphicsDevice device, Matrix world, bool indexed)
        {
            effect.View = FirstPersonCamera.View;
            effect.Projection = FirstPersonCamera.Projection;
            effect.World = world;

            effect.CurrentTechnique.Passes[0].Apply();

            device.SetVertexBuffer(vBuffer);
            if (indexed)
            {
                device.Indices = iBuffer;

                device.DrawIndexedPrimitives(PrimitiveType.TriangleList, 0, 0, vBuffer.VertexCount, 0, iBuffer.IndexCount/3);
            }
            else
            {
                device.DrawPrimitives(PrimitiveType.TriangleList, 0, vBuffer.VertexCount/3);
            }
        }
    }
}