using CID.TestRail.Enums;

namespace CID.RabbitMQ.Extensions
{
    public static class StringExtensions
    {
        public static ResultStatus ParseToTestRailStatus(this string value)
        {

            switch (value.ToUpper())
            {
                case "SUCCESS":
                    return ResultStatus.Passed;
                case "FAILURE":
                    return ResultStatus.Failed;
                default:
                    return ResultStatus.Retest;
            }
        }
    }
}