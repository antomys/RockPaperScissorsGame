namespace Client.Models
{
    public class Statistics
    {
        public string Login { get; set; }
        
        public int Score { get; set; }

        public override string ToString()
        {
            return $"Login: {Login}; Score: {Score}";
        }
    }
}