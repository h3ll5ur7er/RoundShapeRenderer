using System.Linq;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public class SmoothedModel2D : Model<VertexPositionColorNormal>
    {
        private readonly int model = -1, refinement = -1, smoothing = -1, iterations = -1;

        public SmoothedModel2D(Game game, int model, int refineStages, int smoothenStages, int iterations, params Vector3[] points)
        {
            this.model = model;
            this.refinement = refineStages;
            this.smoothing = smoothenStages;
            this.iterations = iterations;

            var newPoints = Step(points.ToList(), refineStages, smoothenStages, iterations);

            for (int i = 0; i < newPoints.Count; i++)
            {
                Vertices.Add(new VertexPositionColorNormal(newPoints[i], Color.Red, Vector3.Up));
                Vertices.Add(new VertexPositionColorNormal(newPoints[(i+1)%newPoints.Count], Color.Red, Vector3.Up));
                Vertices.Add(new VertexPositionColorNormal(Vector3.Zero, Color.White, Vector3.Up));
            }

            SetUp(game);
        }

        private List<Vector3> Step(List<Vector3> points, int refineStages, int smoothenStages, int iterations)
        {
            var newpoints = points;
            
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < refineStages; j++)
                {
                    newpoints = Refine(newpoints);
                }

                for (int j = 0; j < smoothenStages; j++)
                {
                    newpoints = Smoothen(newpoints);
                }
            }

            return newpoints;
        }

        private List<Vector3> Refine(List<Vector3> points)
        {
            var start = points.First();
            var end = points.Last();
            var newPoints = new List<Vector3>();
            for (int i = 0; i < points.Count-1; i++)
            {
                newPoints.Add(points[i]);
                newPoints.Add((points[i]+points[i+1])/2);
            }
            newPoints.Add(end);
            newPoints.Add((start+end)/2);
            return newPoints;
        }

        private List<Vector3> Smoothen(List<Vector3> points)
        {
            var start = points.First();
            var end = points.Last();
            var newPoints = new List<Vector3>();
            newPoints.Add((start+end)/2);
            for (int i = 0; i < points.Count-1; i++)
            {
                newPoints.Add((points[i]+points[i+1])/2);
            }
            return newPoints;
        }

        public virtual void Render(BasicEffect effect, GraphicsDevice device, Matrix world)
        {
            base.Render(effect, device, world, false);
        }

        public override string ToString()
        {
            return string.Format("model={0}, refinement={1}, smoothing={2}, iterations={3}", model, refinement, smoothing, iterations);
        }
    }
}