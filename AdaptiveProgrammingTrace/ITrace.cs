namespace AdaptiveProgrammingTrace
{
    public interface ITrace
    {
        void ErrorLog(string message, string source);

        void WarningLog(string message, string source);

        void InfoLog(string message, string source);
    }
}