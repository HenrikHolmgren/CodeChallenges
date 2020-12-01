using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;

namespace WindowsillSoft.CodeChallenges.Driver.Dialogs
{
    /// <summary>
    /// Interaction logic for ChoiceDialog.xaml
    /// </summary>
    public partial class ChoiceDialog : Window
    {
        private ChoiceDialogVM? _model;
        private ChoiceDialogVM? Model { get => _model; set => DataContext = _model = value; }

        private ChoiceDialog(string header, IEnumerable<object> choices)
        {
            InitializeComponent();
            Model = new ChoiceDialogVM(header, choices);
        }

        private object? Result => Model?.SelectedChoice;
        public static T? ShowDialog<T>(string header, IEnumerable<T> choices) where T : class
        {
            var dlg = new ChoiceDialog(header, choices);
            dlg.ShowDialog();
            return (T?)dlg.Result;
        }
    }

    public class ChoiceDialogVM
    {
        public string Header { get; set; }
        public ObservableCollection<ChoiceDialogChoiceVM> Choices { get; set; } = new ObservableCollection<ChoiceDialogChoiceVM>();
        public object? SelectedChoice => Choices.FirstOrDefault(p => p.Selected)?.Value;

        public ChoiceDialogVM(string header, IEnumerable<object> choices)
        {
            Header = header;
            Choices = new ObservableCollection<ChoiceDialogChoiceVM>(choices.Select(p => new ChoiceDialogChoiceVM(p)));
            if (Choices.Any())
                Choices.First().Selected = true;
        }

    }

    public class ChoiceDialogChoiceVM : INotifyPropertyChanged
    {
        public ChoiceDialogChoiceVM(object value)
        {
            Value = value;
            Description = value.ToString() ?? string.Empty;
        }

        public object Value { get; }
        public string Description { get; }

        private bool _selected = false;
        public bool Selected
        {
            get => _selected;
            set
            {
                _selected = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(Selected)));
            }
        }

        public event PropertyChangedEventHandler? PropertyChanged;
    }
}
