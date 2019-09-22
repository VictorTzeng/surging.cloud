using System.Threading.Tasks;
using Surging.Core.CPlatform.Runtime.Session;
using Surging.Core.ProxyGenerator;
using Surging.IModuleServices.Common;

namespace Surging.Modules.Common.Domain
{
    public class PermissionService : ProxyServiceBase, IPermissionService
    {
        private readonly ISurgingSession _surgingSession;

        public PermissionService() {
            _surgingSession = NullSurgingSession.Instance;
        }

        public async Task<bool> Check(string serviceId)
        {
            var loginUserId = _surgingSession.UserId;
            return true;
        }
    }
}
