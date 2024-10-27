namespace CodeSphere.Domain.Models.Entities
{
    public class TutorialImage
    {
        public int ID { get; set; }
        public int TID { get; set; }
        public string Image { get; set; }

        public Tutorial Tutorial { get; set; }
    }
}
