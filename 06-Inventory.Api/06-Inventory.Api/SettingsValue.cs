namespace _06_Inventory.Api
{
    public class SettingsValue
    {
        public string ConnectionString { get; set; }
        public string DefaultCulture { get; set; }
        public string WalletDirectory { get; set; }
        public ApiAuth ApiAuth { get; set; }
        public string NovusPathBase { get; set; }
    }
    
    public class ApiAuth
    {
        public string SECRET_KEY { get; set; }
        public string ISSUER { get; set; }
        public string AUDIENCE { get; set; }

    }
}
