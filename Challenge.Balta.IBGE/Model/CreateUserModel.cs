namespace Challenge.Balta.IBGE.Model
{
    public record CreateUserModel
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
