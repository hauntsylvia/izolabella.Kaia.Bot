namespace izolabella.Kaia.Bot.Objects.Util
{
    public class KaiaException(string Message, string? ParamName = null) : Exception
    {
        public override string Message { get; } = Message;

        public string? ParamName { get; } = ParamName;
    }
}