namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public abstract class UndoableCommandBase : CommandBase, IUndoableCommand
    {
        public abstract void Undo();
    }
}