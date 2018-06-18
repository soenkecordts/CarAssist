namespace CarAssist.Utils
{
    public interface ILogger
    {
        void WriteLine(object obj);
        bool Overwrite { get; set; }
    }
}
