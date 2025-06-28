namespace web_application
{
    public class AppSettings
    {
        public static AppSettings Instance { get; private set; } // ✏️ "Instanec" emas, "Instance"

        public PostgresConfig Postgres { get; set; }

        public static void Init(AppSettings app)
        {
            Instance = app;
        }

        public class PostgresConfig
        {
            public string ConnectionString { get; set; }
        }
    }
}

