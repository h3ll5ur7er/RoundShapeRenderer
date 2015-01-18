using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public class DebugOverlay
    {
        public static bool Active = false;

        private static readonly DebugOverlay instance = new DebugOverlay();
        private static readonly object WriteLock = new object();
        private readonly Dictionary<string, object> entries = new Dictionary<string, object>();
        private SpriteFont font;
        private StringBuilder s = new StringBuilder();

        private DebugOverlay()
        {
        }

        public static void Set(string key, object value)
        {
            lock (WriteLock)
            {
                if (instance.entries.ContainsKey(key))
                {
                    instance.entries[key] = value;

                    instance.s = new StringBuilder();

                    foreach (var entry in instance.entries)
                    {
                        instance.s.AppendLine(string.Format("{0}\t\t: {1}", entry.Key, entry.Value));
                    }
                }

                else
                {
                    instance.entries.Add(key, value);
                    instance.s.AppendLine(string.Format("{0}\t\t: {1}", key, value));
                }

            }
        }

        public static void LoadContent(SpriteFont font)
        {
            instance.font = font;
        }

        public static void Draw(GameTime gameTime, SpriteBatch sb, float layer)
        {
            if(Active)
                for (int i = 0; i < instance.entries.Count; i++)
                {
                    sb.DrawString(instance.font, instance.s, new Vector2(10), Color.White, 0, Vector2.Zero, Vector2.One, SpriteEffects.None, layer);
                }
        }
    }
}