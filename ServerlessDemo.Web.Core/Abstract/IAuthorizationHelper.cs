using System.Threading.Tasks;

namespace ServerlessDemo.Web.Core.Abstract
{
    public interface IAuthorizationHelper
    {
        Task<bool> IsAdminByUpn(string upn);
        Task<bool> IsAdminBySub(string sub);
    }
}
