using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using WindowsillSoft.CodeChallenges.Core;

namespace WindowsillSoft.CodeChallenges.Driver.Models
{
    public class SolverCategoryVM
    {
        public SolverCategoryVM(Type categoryRootType)
            => CategoryName = categoryRootType
                .GetCustomAttributes()
                .OfType<SolverCategoryAttribute>()
                .SingleOrDefault()?.CategoryName ?? categoryRootType.Name;

        public int SolverCount => ChildCategories.Sum(p => p.SolverCount) + Solvers.Count;

        public SolverCategoryVM(Type root, ILookup<Type, Type> problemSolvers, IIOProvider iOProvider) : this(root)
        {
            Debug.WriteLine($"Category for {root.FullName}");

            var children = problemSolvers.Where(p => p.Key == root || p.Key.IsGenericType && p.Key.GetGenericTypeDefinition() == root)
                .Where(p=>p!=root)
                .SelectMany(p => p);

            Debug.WriteLine($"{children.Count()} children found..");
            foreach (var child in children.Distinct())
            {
                if (child.IsAbstract)
                    ChildCategories.Add(new SolverCategoryVM(child, problemSolvers, iOProvider));
                else
                    Solvers.Add(new SolverInstanceVM(child, iOProvider));
            }
        }

        public string CategoryName { get; set; }
        public ObservableCollection<SolverCategoryVM> ChildCategories { get; set; } = new ObservableCollection<SolverCategoryVM>();
        public ObservableCollection<SolverInstanceVM> Solvers { get; set; } = new ObservableCollection<SolverInstanceVM>();
    }
}
