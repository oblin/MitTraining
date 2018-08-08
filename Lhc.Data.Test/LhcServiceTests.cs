using JagiCore.Admin.Data;
using Lhc.Data;
using System;
using Xunit;

namespace Lhc.Data.Test
{
    public class LhcServiceTests
    {
        [Fact]
        public void Test_Get_Postgresql_Database()
        {
            // 這個測試會直接存取資料庫資料，因此如果發生錯誤可以不用執行
            var clinic = new Clinic { Database = "lhcdb_as", DatabaseUser = "postgres", DatabasePassword = "490910" };
            var service = new LhcService(clinic);

            Assert.NotEmpty(service.GetInHospitalPatients());
        }
    }
}
