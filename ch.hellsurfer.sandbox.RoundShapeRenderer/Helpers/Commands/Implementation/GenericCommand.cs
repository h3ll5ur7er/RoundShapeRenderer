using System;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public class GenericCommand : CommandBase
    {
        private readonly Action a;

        public GenericCommand(Action a)
        {
            this.a = a;
        }

        public override void Execute()
        {
            a();
        }
    }
}