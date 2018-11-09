using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Sikafon.Models;

namespace Sikafon.Services
{
    public class Register
    {
        public async Task<Response> Add(Agent model)
        {
            Rest restService = new Rest();
            Response response = await restService.PostAsync("agents", model);
            return response;
        }
    }
}
