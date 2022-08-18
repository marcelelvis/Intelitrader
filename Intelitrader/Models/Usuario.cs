using System.Text.Json.Serialization;

namespace Intelitrader.Models
{
    public class Usuario
    {
        
        public string Id { get; private set; } = Guid.NewGuid().ToString();

        public string firstName { get; set; }

        public string? surname { get; set; }

        public int age { get; set; }

        public DateTime creationDate { get; private set; }

       

       
    }
}
