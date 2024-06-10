namespace capygram.Auth.DependencyInjection.Options
{
    public class UserDBSetting
    {
        public string ConnectionString { get; set; }
        public string DatabaseName { get; set; }
        public string UserCollectionName { get; set; }
        public string UserOTPCollectionName { get; set; }
    }
}
