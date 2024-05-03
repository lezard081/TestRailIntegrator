using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CID.TestRail.Enums
{
    public enum ClientType
    {
        [Description("TestRailGet")]
        TestRailGet,

        [Description("TestRailPost")]
        TestRailPost
    }
}
