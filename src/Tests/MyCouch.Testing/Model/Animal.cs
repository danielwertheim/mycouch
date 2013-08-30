namespace MyCouch.Testing.Model
{
    public class Animal
    {
        //Could be _id, AnimalId, DocumentId, EntityId, Id (you can change this convention)
        public string AnimalId { get; set; }
        //Could be _rev, AnimalRev, DocumentRev, EntityRev, Rev (you can change this convention)
        public string AnimalRev { get; set; }

        public string Class { get; set; }
        public double MinWeight { get; set; }
        public double MaxWeight { get; set; }
        public double MinLength { get; set; }
        public double MaxLength { get; set; }
        public string LatinName { get; set; }
        public string WikiPage { get; set; }
        public string Diet { get; set; }
    }
}