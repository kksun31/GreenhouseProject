namespace GreenhouseApi.Models
{
    public class Plant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Species { get; set; } = string.Empty;
        public int WateringIntervalDays { get; set; }
        public bool NeedsDirectSunlight { get; set; }
    }
}