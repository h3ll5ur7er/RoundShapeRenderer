namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public interface IMacroCommand : ICommand
    {
        void AddCommand(ICommand command);
    }
}