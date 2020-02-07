namespace AppNiZiAPI.Models
{
    class Account
    {
        public int AccountId { get; set; }
        public string Name { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public string Email { get; set; }
    }
}
