namespace AlleyCatBarbers.ViewModels
{
    public class UserViewModel
    {
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public DateOnly DateOfBirth { get; set; }
        public IList<string> Roles { get; set; }



    }
}
