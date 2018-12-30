namespace BullseyeTests
{
    using System;
    using System.Linq;
    using Bullseye;
    using BullseyeTests.Infra;
    using PublicApiGenerator;
    using Xunit;

    public class Api
    {
        [Fact]
        public void IsUnchanged() =>
            AssertFile.Contains(
#if NETCOREAPP2_2
                "../../../api-netcoreapp2_2.txt",
#endif
                ApiGenerator
                    .GeneratePublicApi(
                        typeof(Targets).Assembly,
                        typeof(Targets).Assembly.GetExportedTypes().Where(type => !type.Namespace.Contains("Internal")).ToArray())
                    .Replace(Environment.NewLine, "\r\n"));
    }
}
