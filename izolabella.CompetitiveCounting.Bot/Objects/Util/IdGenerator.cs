namespace izolabella.CompetitiveCounting.Bot.Objects.Util
{
    public class IdGenerator
    {
        public static ulong LastGenerated { get; private set; }
        private static DateTime MakeFrom => new(2010, 1, 1);
        public static ulong CreateNewId()
        {
            TimeSpan IdFrom = DateTime.UtcNow - MakeFrom;
            ulong IdTime = (ulong)IdFrom.TotalMilliseconds;
            if (LastGenerated != IdTime)
            {
                LastGenerated = IdTime;
                return IdTime;
            }
            else
            {
                return CreateNewId();
            }
        }
    }
}
