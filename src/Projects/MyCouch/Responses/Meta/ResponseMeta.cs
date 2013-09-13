namespace MyCouch.Responses.Meta
{
    public static class ResponseMeta
    {
        public static ResponseScheme Scheme { get; private set; }

        static ResponseMeta()
        {
            Scheme = new ResponseScheme();
        }
    }
}