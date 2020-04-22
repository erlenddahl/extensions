using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NpgsqlExtensions.Orm;

namespace NpgsqlExtensions.Tests
{
    [TestClass]
    public class DumpCsvTests
    {
        [TestMethod]
        public void Dump()
        {
            var db = new PostgresOrm("sinteftfgoofy.sintef.no", 5432, "GeoSUM", "geosum_writer", "lvJdbkmltjveKvu3tHO1");

            using (var session = db.OpenSession())
            {
                session.DumpTableAsCsv("gpsdata_fixed", @"C:\Users\erlendd\Desktop\Søppel\2020-03-10 - GeoSUM, QF-data.csv", ignoreColumns:new []{"geometry", "geometry_utm"});
            }
        }
    }
}
