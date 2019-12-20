using System;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NpgsqlExtensions.Orm;

namespace NpgsqlExtensions.Tests
{
    [TestClass]
    public class InsertionTests
    {
        [TestMethod]
        public void InsertRange()
        {
            var db = GetDb();

            using (var session = db.OpenSession())
            {
                session.CreateTable<Site>(tableName: "TEST1", dropIfExists: true);

                var sites = new[]
                {
                    new Site("E39 Øysand", 0000, 0000),
                    new Site("E39 Vågsbotn", 0000, 0000),
                    new Site("E39 Ostereidet", 0000, 0000),
                    new Site("E39 Halbrendslia", 0000, 0000),

                    new Site("E6 Fokstugu", 0000, 0000),
                    new Site("E6 Avsjøen", 0000, 0000),
                    new Site("E6 Hjerkinn", 0000, 0000){Something = 17},
                    new Site("E6 Grønnbakken", 0000, 0000),
                    new Site("Fv29 Gravbekklia", 0000, 0000),


                    new Site("Trondheim", 63.430515, 10.395053),
                    new Site("Oslo", 59.913868, 10.752245),
                    new Site("Bergen", 60.391262, 5.322054),
                    new Site("Tromsø", 69.649208, 18.955324),
                    new Site("Drammen", 59.744076, 10.204456),
                    new Site("Stord, Leirvik", 59.779765, 5.500799),
                    new Site("Trondheim, Steinan", 63.396395, 10.442248)
                };
                session.InsertRange(sites, tableName: "TEST1");
            }
        }

        private PostgresOrm GetDb()
        {
            return new PostgresOrm("sinteftfgoofy.sintef.no", 5432, "WeatherLogger", "weatherloggerwrite", "xL1rvizMX4AFKkQKMvQg");
        }


        [TestMethod]
        public void InsertSingle()
        {
            return;
            var db = GetDb();

            using (var session = db.OpenSession())
            {
                session.CreateTable<Site>(tableName: "TEST2", dropIfExists: true);
                session.Insert(new Site("E39 Øysand", 1, 2), tableName: "TEST2");
            }
        }

        public class Site
        {
            public int Id { get; set; }
            public int? Something { get; set; }
            public string Name { get; set; }
            public double Latitude { get; set; }
            public double Longitude { get; set; }

            public Site()
            {

            }

            public Site(string name, double latitude, double longitude)
            {
                Name = name;
                Latitude = latitude;
                Longitude = longitude;
            }
        }
    }
}
