namespace ItecwebApp.Models
{
    public class Commitees
    {
        public int Id { get; set; }
        public int year { get; set; }
        public string Name { get; set; }
    
        public Commitees(int id, string name, int year)
        {
            Id = id;
            Name = name;
           this.year = year;
        }
        public Commitees()
        {
        }
    }
}
