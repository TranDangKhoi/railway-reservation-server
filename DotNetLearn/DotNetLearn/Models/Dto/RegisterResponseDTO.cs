namespace DotNetLearn.Models.Dto
{
    public class RegisterResponseDTO
    {
        public string Access_token { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
