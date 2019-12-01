using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WindowsillSoft.CodeChallenges.AdventOfCode;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Driver.Models;
using WindowsillSoft.CodeChallenges.ProjectEuler;
using WpfAsyncPack.Command;

namespace WindowsillSoft.CodeChallenges.Driver
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            //Stupid fucking workaround because MS doesn't have a version of GetReferencedAssemblies which actually gives REFERENCED assemblies instead of 'optimizing' to only give loaded assemblies due to a concrete reference to a class from the assembly.
            var workaround = new[]
            {
                typeof(AdventOfCodeSolverBase),
                typeof(ProjectEulerSolverBase),
            };

            InitializeComponent();
            var uiProvider = new DelegateGUIIOProvider(_ => { }, Dispatcher.Invoke);
            MainPanel.DataContext = new MainWindowVM(uiProvider);

        }
    }

    public class MainWindowVM : INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<SolverCategoryVM> Solvers { get; }


        private SolverCategoryVM? _selectedCategory;
        public SolverCategoryVM? SelectedCategory
        {
            get => _selectedCategory;
            set
            {
                _selectedCategory = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(SelectedCategory)));
            }
        }

        public ICommand UpdateSelection { get; }

        public MainWindowVM(IIOProvider iOProvider)
        {

            var allSolvers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(p => p.GetExportedTypes())
                .Where(p => typeof(ProblemSolverBase).IsAssignableFrom(p))
                .ToList();

            var roots = allSolvers.Where(p => p.IsGenericType && p.GetGenericTypeDefinition() == typeof(ProblemSolverBase<>));

            Solvers = new ObservableCollection<SolverCategoryVM>(
                roots.Select(p => new SolverCategoryVM(p, allSolvers.ToLookup(q => q.GetAnnotatedBase<SolverCategoryAttribute>()), iOProvider)));

            UpdateSelection = new SyncCommand<SolverCategoryVM>(SetSelection);
        }
                
        private void SetSelection(SolverCategoryVM solver)
        {
            if (solver.Solvers.Any())
                SelectedCategory = solver;
        }
    }

    public static class TypeExtensions
    {
        public static Type GetAnnotatedBase<T>(this Type type) where T : Attribute
        {
            var res = type.BaseType;
            while (res != typeof(object) && !res.GetCustomAttributes(false).All(p => p is T))
                res = res.BaseType;
            return res;
        }
    }
}
