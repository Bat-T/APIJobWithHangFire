namespace APIJobWithHangFire.Helper
{
    public static class CustomCron
    {
        public static string EveryNSeconds(int seconds)
        {
            return $"*/{seconds} * * * * *";
        }
    }
}
