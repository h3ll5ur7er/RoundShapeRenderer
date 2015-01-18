namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public interface IUndoableCommand : ICommand
    {
        void Undo();
    }
}