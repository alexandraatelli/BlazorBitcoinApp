namespace BlazorBitcoinApp.DTOs
{
    public class BitcoinDataDTO
    {
        //Construtor vazio
        public BitcoinDataDTO() { }

        //Construtor que recebe dois parametros
        public BitcoinDataDTO(DateTime day, decimal closeValue) 
        {
            Day = day;
            CloseValue = closeValue;
        }

        public DateTime Day { get; set; }

        public decimal CloseValue { get; set; }
    }
}
