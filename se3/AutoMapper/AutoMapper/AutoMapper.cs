using System;
using System.Collections.Generic;
using System.Reflection;

namespace AutoMapper
{
    public class AutoMapper
    {
        public static PropertyBuilder<TSrc, TDest> Build<TSrc, TDest>()
        {
            return new PropertyBuilder<TSrc, TDest>();
        }
    }
}