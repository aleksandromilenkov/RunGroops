using RunGroops.Models;

namespace RunGroops.ViewModels {
    public class HomeViewModel {
        public IEnumerable<Club> Clubs { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string Country { get; set; }
        public bool IsAuthenticated { get; set; }
    }
}
