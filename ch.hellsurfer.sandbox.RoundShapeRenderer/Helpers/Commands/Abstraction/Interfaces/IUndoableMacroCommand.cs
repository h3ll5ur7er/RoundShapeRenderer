namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public interface IUndoableMacroCommand : IUndoableCommand
    {
        void AddCommand(IUndoableCommand command);
    }
}