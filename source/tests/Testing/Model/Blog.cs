namespace MyCouch.Testing.Model
{
    public class Blog
    {
        public string Id { get; set; }
        public string Rev { get; set; }
        public string Title { get; set; }
        public Author Author { get; set; }
        public BlogEntry[] Entries { get; set; }

        public int YearsActive { get; set; }
    }

    public class Author
    {
        public int Age { get; set; }
        public string Name { get; set; }
    }

    public class BlogEntry
    {
        public int Sequence { get; set; }
        public string Subject { get; set; }
        public string Content { get; set; }
    }
}
