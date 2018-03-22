namespace RohlikAPI
{
    public class City
    {
        private City(string street, string city)
        {
            Street = street;
            CityName = city;
        }

        public string Street { get; set; }
        public string CityName { get; set; }
        public static City Brno => new City("Česká 161/1", "Brno");
        public static City Praha => new City("Václavské náměstí 846/1", "Praha");
    }
}