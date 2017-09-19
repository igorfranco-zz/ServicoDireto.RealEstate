using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpongeSystems.Core.Attributes
{
    using System;
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = false, Inherited = true)]
    public sealed class AllowAnonymousAttribute : Attribute { }
}
