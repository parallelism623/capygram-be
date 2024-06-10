namespace capygram.Auth.Domain.Model
{
    public class Role
    {
        public static string USER = nameof(USER);
        public static string ADMIN = nameof(ADMIN);
        public string Name { get; set; }
    }
}
