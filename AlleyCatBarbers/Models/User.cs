namespace AlleyCatBarbers.Models
{
    public class User
    {
        public int Id { get; set; }
        public DateTime Date { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public DateOnly DateOfBirth { get; set; }

    }
}
