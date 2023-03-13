namespace DotNetLearn.Models.Dto
{
    public class LoginResponseDTO
    {
        public string Access_token { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
