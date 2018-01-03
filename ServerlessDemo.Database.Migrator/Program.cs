using System;
using System.Linq;
using NLog;
using ServerlessDemo.Database.Migrator.Infrastructure.DataAccess;
using ServerlessDemo.Database.Migrator.Model;

namespace ServerlessDemo.Database.Migrator
{
    class Program
    {
        private static Logger _logger = LogManager.GetCurrentClassLogger();

        static void Main(string[] args)
        {
            try
            {
                _logger.Debug("Configuring database");
                Boostrapper.ConfigureDatabase();

                _logger.Debug("Applying migrations on the target database");
                var dbContextFactory = new DbContextFactory();
                using (var context = dbContextFactory.Get())
                {
                    //Executing any query to force model update in database;
                    context.Set<Image>().ToList();
                }
            }
            catch (Exception exc)
            {
                _logger.Error(exc, "Unexpected error occured during processing");
            }
        }
    }
}
