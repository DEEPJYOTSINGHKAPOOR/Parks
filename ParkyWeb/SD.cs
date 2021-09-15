using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ParkyWeb
{
    public static class SD
    {
        public static string APIBaseUrl = "https://localhost:5001/";

        /// <summary>
        /// dont forget to write / at end!
        /// </summary>
        public static string NationalParkApiPath = APIBaseUrl + "api/nationalparks/";
    }
}
