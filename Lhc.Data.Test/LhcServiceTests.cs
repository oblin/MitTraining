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
            // �o�Ӵ��շ|�����s����Ʈw��ơA�]���p�G�o�Ϳ��~�i�H���ΰ���
            var clinic = new Clinic { Database = "lhcdb_as", DatabaseUser = "postgres", DatabasePassword = "490910" };
            var service = new LhcService(clinic);

            Assert.NotEmpty(service.GetInHospitalPatients());
        }
    }
}
