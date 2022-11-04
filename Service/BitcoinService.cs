using BlazorBitcoinApp.DTOs;
using Newtonsoft.Json.Linq;

namespace BlazorBitcoinApp.Services
{
    public class BitcoinService : IBitcoinService
    {
        //Injecao de dependência - para fazer o mapeamento dos dados, para utilizar a funcao getAsync
        private readonly HttpClient _httpClient;

        //parametros devido a injecao de dependencia
        public BitcoinService(HttpClient httpClient) 
        {
            _httpClient = httpClient;

        }

        //funcao para retornar os dados da API qeu utilizamos. Utiliza pela data inicial
        public async Task<List<BitcoinDataDTO>> FindBy(DateTime startDate) 
        {
            var resonse = await _httpClient.GetAsync("https://data.messari.io/api/v1/markets/binance-btc-usdt/metrics/price/time-series?start=" + startDate.ToString("yyyy-MM-dd") + "&interval=1d");
            //verifica se deu tudo certo
            resonse.EnsureSuccessStatusCode();
            var jsonResult = await resonse.Content.ReadAsStringAsync();
            JObject jObject = JObject.Parse(jsonResult);
            var values = jObject.SelectToken("data.values").ToString();
            if (string.IsNullOrWhiteSpace(values))
                return new List<BitcoinDataDTO>();

            //vai converter com uma lista de decimal
            var data = Newtonsoft.Json.JsonConvert.DeserializeObject<List<decimal[]>>(values);

            //temos que fazer retorno dessa lista de decimal a lista de retorno BitcoinDataDTO - dia e valor de fechamento
            return data.Select(d => new BitcoinDataDTO(new DateTime(1970, 1, 1).AddMilliseconds((long)d[0]), d[3])).ToList();

        }
    }
}
