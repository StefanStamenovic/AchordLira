using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ServiceStack.Redis;

namespace AchordLira.Models.Redis
{
    public class RedisDataLayer
    {
        private static IRedisClientsManager manager = null;
        private static object objLock = new object();

        public static IRedisClient GetClient()
        {
            if (manager == null)
            {
                lock (objLock)
                {
                    if (manager == null)
                        manager = CreateClientManager();
                }
            }

            return manager.GetClient();
        }

        private static IRedisClientsManager CreateClientManager()
        {
            return new BasicRedisClientManager();
        }
    }

    
}
