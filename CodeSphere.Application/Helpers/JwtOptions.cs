namespace CodeSphere.Application.Helpers
{
    public class JwtOptions
    {
        public static string SectionName = "JWT";
        public string Key { get; set; }
        public string Issure { get; set; }
        public string Audience { get; set; }
        public int DurationInDays { get; set; }
    }
}
