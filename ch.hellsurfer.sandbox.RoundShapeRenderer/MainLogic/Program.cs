namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    static class Program
    {
        static void Main(string[] args)
        {
            using (var game = new Engine())
            {
                game.Run();
            }
        }
    }
}

