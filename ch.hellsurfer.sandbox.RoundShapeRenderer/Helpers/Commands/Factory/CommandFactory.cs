using System;

namespace ch.hellsurfer.sandbox.RoundShapeRenderer
{
    public class CommandFactory
    {
        public static ICommand CreateCommand(Action action)
        {
            return new GenericCommand(action);
        }

        public static IUndoableCommand CreateUndoableCommand(Action action, Action undo)
        {
            return new GenericUndoableCommand(action, undo);
        }

        public static IUndoableCommand CreateToggleCommand(Action toggle)
        {
            return new GenericUndoableCommand(toggle, toggle);
        }

        public static ICommand CreateMacro(params ICommand[] commands)
        {
            var macro = new GenericMacroCommand();
            foreach (var command in commands)
            {
                macro.AddCommand(command);
            }
            return macro;
        }

        public static IUndoableCommand CreateUndoableMacro(params IUndoableCommand[] commands)
        {
            var macro = new GenericUndoableMacroCommand();
            foreach (var command in commands)
            {
                macro.AddCommand(command);
            }
            return macro;
        }
    }
}