using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharedLib
{
    /// <summary>
    /// Specify assembly target tenant
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class TenantAttributeAttribute : Attribute
    {
        private readonly string _tenantId;

        public TenantAttributeAttribute(string tenantId) { _tenantId = tenantId; }

        /// <summary>
        /// Tenant Identifier.
        /// </summary>
        public string TenantId { get { return _tenantId; } }
    }
}