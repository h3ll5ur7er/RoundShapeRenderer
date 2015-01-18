using System.Collections.Generic;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public abstract class MacroCommandBase : CommandBase, IMacroCommand
    {
        Queue<ICommand> commands = new Queue<ICommand>();
        public void AddCommand(ICommand command)
        {
            commands.Enqueue(command);
        }

        public override void Execute()
        {
            for (int i = 0; i < commands.Count; i++)
            {
                commands.Dequeue().Execute();
            }
        }
    }
}