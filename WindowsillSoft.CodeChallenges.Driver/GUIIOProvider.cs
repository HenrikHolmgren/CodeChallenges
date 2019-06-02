using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Windows.Forms;
using WindowsillSoft.CodeChallenges.Core;
using WindowsillSoft.CodeChallenges.Driver.Dialogs;

namespace WindowsillSoft.CodeChallenges.Driver
{
    public class DelegateGUIIOProvider : IIOProvider
    {
        private Action<string> _logLine;
        private Action<Action> _dispatcher;

        public DelegateGUIIOProvider(Action<string> logLine, Action<Action> dispatcher)
        {
            _logLine = logLine;
            _dispatcher = dispatcher;
        }

        public bool ActivateLogging { get; set; } = false;

        public void LogIfAttached(Func<object> logResolver)
        {
            if (ActivateLogging)
            {
                var logEntry = logResolver?.Invoke()?.ToString();
                if (logEntry != null)
                    _logLine?.Invoke(logEntry);
            }
        }

        public void LogLine(object line) => LogIfAttached(() => line);

        public T? RequestChoice<T>(string header, params T[] options) where T : class
        {
            T? result = default;
            _dispatcher.Invoke(() => result = ShowChoiceDialogInternal(header, options));
            return result;
        }

        public string? RequestFile(string header)
        {
            string? fileContent = default;
            _dispatcher.Invoke(() => fileContent = GetFileContentInternal(header));
            return fileContent;
        }

        public string? RequestInput(string header)
        {
            string answerContent = string.Empty;
            _dispatcher.Invoke(() => answerContent = GetAnswerContentInternal(header));
            return answerContent;
        }


        private T? ShowChoiceDialogInternal<T>(string header, T[] options) where T : class
            => ChoiceDialog.ShowDialog(header, options);

        private string? GetFileContentInternal(string header)
        {
            var dialog = new OpenFileDialog() { Title = header };
            if (dialog.ShowDialog() == DialogResult.OK)
                return File.ReadAllText(dialog.FileName);
            return null;
        }

        private string GetAnswerContentInternal(string header)
        {
            MessageBox.Show("Whoops, not supported yet :(");
            return "256";
        }
    }
}
