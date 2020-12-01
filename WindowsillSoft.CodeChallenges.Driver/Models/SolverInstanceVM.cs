using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsillSoft.CodeChallenges.Core;
using WpfAsyncPack.Command;

namespace WindowsillSoft.CodeChallenges.Driver.Models
{
    public class SolverInstanceVM
    {
        public ICommand ExecuteCommand { get; }

        bool IsExecuting { get; set; }

        public string SolverDescription { get; }

        ProblemSolverBase? _problemSolver = null;

        public SolverInstanceVM(Type solverType, IIOProvider iOProvider) : this()
        {
            var solver = (ProblemSolverBase?)Activator.CreateInstance(solverType, iOProvider);
            if (solver != null)
            {
                SolverDescription = solver.Name;
                _problemSolver = solver;
            }
            else
                throw new InvalidOperationException($"Somehow Activator.CreateInstance for type {solverType} returned null.");
        }

        protected SolverInstanceVM()
        {
            SolverDescription = "Unknown solver!";
            ExecuteCommand = new AsyncCommand(OnExecuteCommand, CanExecuteCommand);
            IsExecuting = false;
        }


        private async Task OnExecuteCommand(object arg1, CancellationToken arg2)
        {
            if (_problemSolver == null)
                return;

            IsExecuting = true;
            try
            {
                await Task.Run(_problemSolver.Initialize);
                var result = await Task.Run(_problemSolver.ExecuteObject);
                MessageBox.Show(result.ToString());
            }
            finally
            {
                IsExecuting = false;
            }
        }

        private bool CanExecuteCommand(object arg) => !IsExecuting && _problemSolver != null;
    }
}
