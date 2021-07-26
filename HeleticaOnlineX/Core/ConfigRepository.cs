using System.IO;

namespace HeleticaOnlineX.Core
{
    public class ConfigRepository
    {
        public string ReadTgBotToken()
        {
            string token = File.ReadAllText("./Data/bot_token.txt");

            return token;
        }

        public string ReadDatabaseConfig()
        {
            string dbConfig = File.ReadAllText("./Data/database.config.txt");

            return dbConfig;
        }
    }
}