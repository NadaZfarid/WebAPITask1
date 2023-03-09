namespace WebAPIDay1.DTO
{
    public class TopicDTO
    {
        public int Top_ID { get; set; }
        public string Top_Name { get; set; }
        public List<string> Crs_Name { get; set; }=new List<string>();

    }
}
