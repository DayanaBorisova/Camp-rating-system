namespace Camp_Rating_System.Models.User
{
    public class EmailConfirmationViewModel
    {
        public required string UserId { get; set; }
        public required string Token { get; set; }
    }
}