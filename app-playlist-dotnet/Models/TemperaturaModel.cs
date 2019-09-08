using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace app_playlist_dotnet.Models
{
    public class TemperaturaModel
    {
        public int cod { get; set; }
        public string name { get; set; }
        public Dictionary<string, object> main { get; set; }
        public int Temperatura { get; set; }
    }
}