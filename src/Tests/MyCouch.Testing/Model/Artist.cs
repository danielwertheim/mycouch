namespace MyCouch.Testing.Model
{
    public class Artist
    {
        //Could be _id, ArtistId, DocumentId, EntityId, Id (you can change this convention)
        public string ArtistId { get; set; }

        //Could be _rev, ArtistRev, DocumentRev, EntityRev, Rev (you can change this convention)
        public string ArtistRev { get; set; }

        public string Name { get; set; }

        public Album[] Albums { get; set; }
    }
}