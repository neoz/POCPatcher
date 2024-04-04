using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SampleConsoleNeedCrack
{
    internal static class License
    {
        public static bool IsValidActivation()
        {
            // http get from www.google.com, if response is "OK" then return true
            HttpClient client = new HttpClient();
            HttpResponseMessage response = client.GetAsync("https://www.google.com").Result;
            var body = response.Content.ReadAsStringAsync().Result;
            if (body=="OK")
            {
                return true;
            }
            return false;
        }

    }
}
