using System.Collections.Generic;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public abstract class UndoableMacroCommandBase : UndoableCommandBase, IUndoableMacroCommand
    {
        List<IUndoableCommand> commands = new List<IUndoableCommand>();
        public void AddCommand(IUndoableCommand command)
        {
            commands.Add(command);
        }

        public override void Execute()
        {
            for (int i = 0; i < commands.Count; i++)
            {
                commands[i].Execute();
            }
        }
        public override void Undo()
        {
            for (int i = commands.Count-1; i > -1; i++)
            {
                commands[i].Execute();
            }
        }
    }
}