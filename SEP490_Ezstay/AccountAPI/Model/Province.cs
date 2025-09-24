namespace AccountAPI.Model
{
    public class Province
    {
        public string Code { get; set; } = null!;
        public string Name { get; set; } = null!;
        public List<Commune> Communes { get; set; } = new();
    }
}
