using System;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public class GenericUndoableCommand : UndoableCommandBase
    {
        private readonly Action doit;
        private readonly Action undoit;

        public GenericUndoableCommand(Action doit, Action undoit)
        {
            this.doit = doit;
            this.undoit = undoit;
        }

        public override void Execute()
        {
            doit();
        }

        public override void Undo()
        {
            undoit();
        }
    }
}