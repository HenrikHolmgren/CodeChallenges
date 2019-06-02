using System;

namespace WindowsillSoft.CodeChallenges.Core
{
    public interface IIOProvider
    {
        string? RequestInput(string header);
        T? RequestChoice<T>(string header, params T[] options) where T : class;
        string? RequestFile(string header);

        void LogLine(object line);
        void LogIfAttached(Func<object> logResolver);
    }
}
