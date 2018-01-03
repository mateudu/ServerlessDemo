using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ServerlessDemo.Web.Core.Abstract
{
    public interface IAadHelper
    {
        Task<bool> IsAdminBySub(string sub);
    }
}
