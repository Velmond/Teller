namespace Teller.Models
{
    using System.ComponentModel.DataAnnotations.Schema;

    public class LinkedProfiles
    {
        public int Id { get; set; }

        public string Facebook { get; set; }

        public string GooglePlus { get; set; }

        public string Twitter { get; set; }

        public string LinkedIn { get; set; }

        public string YouTube { get; set; }
    }
}
