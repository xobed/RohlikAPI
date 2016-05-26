namespace RohlikAPI
{
    public class City
    {
        private City(string address) { Address = address; }
        public string Address { get; set; }
        public static City Brno => new City("Česká 161/1, Brno 60200");
        public static City Praha => new City("Václavské náměstí 846/1, Praha 11000");
    }
}