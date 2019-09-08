using app_playlist_dotnet.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using WebApi.OutputCache.V2;

namespace app_playlist_dotnet.Controllers
{
    [RoutePrefix("api/Playlist")]
    public class PlaylistController : ApiController
    {
        private Uri URL_API_PLAYLIST;

        public PlaylistController()
        {
            URL_API_PLAYLIST = new Uri("https://api.spotify.com/v1/");
        }

        [Route("GetPlayListByCidade")]
        [CacheOutput(ServerTimeSpan = 300)] // Cache de 5 minutos (melhorar performance)
        [AllowAnonymous]
        [HttpGet]
        public async Task<PlaylistCustomModel> GetPlayListByCidade([FromUri] string cidade)
        {
            //Inicializa Models Globais
            PlaylistCustomModel playlistCustomModel = new PlaylistCustomModel();
            TokenAcessoModel tokenAcessoModel = new TokenAcessoModel();

            try
            {
                // Monta URL API
                string urlFinal = URL_API_PLAYLIST.AbsoluteUri;

                // Get Temperatura atual da cidade
                TemperaturaController temperaturaController = new TemperaturaController();
                TemperaturaModel temperaturaModel = await temperaturaController.GetTemperaturaAtualByCidade(cidade);

                // Set Temperatura atual e Cidade
                playlistCustomModel.NomeCidade = cidade;
                playlistCustomModel.Temperatura = temperaturaModel.Temperatura;

                // Playlist de musicas Pop
                if (temperaturaModel.Temperatura >= 25)
                {
                    urlFinal += "playlists/37i9dQZF1DWVLcZxJO5zyf";
                    playlistCustomModel.GeneroMusicalSugerido = "Pop";
                }
                // Playlist de musicas Rock
                else if (temperaturaModel.Temperatura >= 10 && temperaturaModel.Temperatura < 25)
                {
                    urlFinal += "playlists/37i9dQZF1DXcmaoFmN75bi";
                    playlistCustomModel.GeneroMusicalSugerido = "Rock";
                }
                // Playlist de musicas Classicas
                else if (temperaturaModel.Temperatura < 10)
                {
                    urlFinal += "playlists/37i9dQZF1DWWEJlAGA9gs0";
                    playlistCustomModel.GeneroMusicalSugerido = "Musica Clássica";
                }

                #region Get Token Authorization API 
                using (var http = new HttpClient())
                {
                    //Tranforma Client_ID e Client_Secret em Base 64
                    byte[] bytes = Encoding.ASCII.GetBytes("ce4d41cc1cb14568be5247b1716f57f2:cab9bb46f92d424f89a97d793273906a");
                    string codeToken = Convert.ToBase64String(bytes);

                    // Define Header
                    http.DefaultRequestHeaders.Add("Authorization", "Basic " + codeToken);

                    // Define parameters Body
                    var content = new FormUrlEncodedContent(new[] {
                        new KeyValuePair<string, string>("grant_type", "client_credentials"),
                    });

                    var tokenResponse = await http.PostAsync("https://accounts.spotify.com/api/token", content);

                    if (tokenResponse.IsSuccessStatusCode)
                    {
                        string tokenAcessoString = await tokenResponse.Content.ReadAsStringAsync();
                        tokenAcessoModel = JsonConvert.DeserializeObject<TokenAcessoModel>(tokenAcessoString);
                    }
                }
                #endregion

                #region Retonar Playlist pelo gênero
                using (var http = new HttpClient())
                {
                    // define headers
                    http.DefaultRequestHeaders.Add("Authorization", "Bearer " + tokenAcessoModel.access_token);
                    http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    // get response API
                    var response = await http.GetAsync(urlFinal);

                    if (response.IsSuccessStatusCode)
                    {
                        string stringJson = await response.Content.ReadAsStringAsync();

                        List<string> playList = PlayListTemperaturaModel.FromJson(stringJson)
                            .Tracks.Items.Select(a => a.Track.Name).ToList();

                        playlistCustomModel.PlayList = playList;
                    }
                }
                #endregion

                playlistCustomModel.Data = DateTime.Now;

                return playlistCustomModel;
            }
            catch (Exception ex)
            {
                return new PlaylistCustomModel();
            }
        }
    }
}
