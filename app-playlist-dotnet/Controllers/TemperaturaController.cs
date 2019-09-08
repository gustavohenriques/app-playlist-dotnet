using app_playlist_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace app_playlist_dotnet.Controllers
{
    [RoutePrefix("api/Temperatura")]
    public class TemperaturaController : ApiController
    {
        private Uri URL_API_TEMPERATURA;

        public TemperaturaController()
        {
            URL_API_TEMPERATURA = new Uri("http://api.openweathermap.org/data/2.5/");
        }

        [Route("GetTemperaturaAtualByCidade")]
        [AllowAnonymous]
        [HttpGet]
        public async Task<TemperaturaModel> GetTemperaturaAtualByCidade([FromUri] string cidade)
        {
            TemperaturaModel temperaturaModel = new TemperaturaModel();

            try
            {
                string urlFinal = URL_API_TEMPERATURA.AbsoluteUri + "weather?q="
                    + cidade.Trim()
                    + "&units=metric&APPID=ffb5a59fa46d5dadaad146262b19e400";

                using (var http = new HttpClient())
                {
                    var response = await http.GetAsync(urlFinal);

                    if (response.IsSuccessStatusCode)
                    {
                        string stringJson = await response.Content.ReadAsStringAsync();

                        temperaturaModel = JsonConvert.DeserializeObject<TemperaturaModel>(stringJson);
                        temperaturaModel.Temperatura = (int)Math.Round(Convert.ToDouble(temperaturaModel.main["temp"]));
                    }
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return temperaturaModel;
        }
    }
}
