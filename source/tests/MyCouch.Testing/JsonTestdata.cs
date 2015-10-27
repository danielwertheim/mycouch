namespace MyCouch.Testing
{
    public static class JsonTestdata
    {
        public static readonly string ViewQueryAlbumRows = "[" +
            "    {" +
            "        \"id\": \"1\"," +
            "        \"key\": \"Fake artist 1\"," +
            "        \"value\": [" +
            "            {" +
            "                \"name\": \"Greatest fakes #1\"" +
            "            }" +
            "        ]" +
            "    }," +
            "    {" +
            "        \"id\": \"2\"," +
            "        \"key\": \"Fake artist 2\"," +
            "        \"value\": [" +
            "            {" +
            "                \"name\": \"Greatest fakes #2.1\"" +
            "            }," +
            "            {" +
            "                \"name\": \"Greatest fakes #2.2\"" +
            "            }" +
            "        ]" +
            "    }," +
            "    {" +
            "        \"id\": \"3\"," +
            "        \"key\": \"Fake artist 3\"," +
            "        \"value\": [" +
            "            {" +
            "                \"name\": \"Greatest fakes #3.1\"" +
            "            }," +
            "            {" +
            "                \"name\": \"Greatest fakes #3.2\"" +
            "            }," +
            "            {" +
            "                \"name\": \"Greatest fakes #3.3\"" +
            "            }" +
            "        ]" +
            "    }," +
            "    {" +
            "        \"id\": \"4\"," +
            "        \"key\": \"Fake artist 4\"," +
            "        \"value\": [" +
            "            {" +
            "                \"name\": \"Greatest fakes #4.1\"" +
            "            }," +
            "            {" +
            "                \"name\": \"Greatest fakes #4.2\"" +
            "            }," +
            "            {" +
            "                \"name\": \"Greatest fakes #4.3\"" +
            "            }," +
            "            {" +
            "                \"name\": \"Greatest fakes #4.4\"" +
            "            }" +
            "        ]" +
            "    }" +
            "]";

        public static readonly string ViewQueryAllDocRows = "[" +
            "    {" +
            "        \"id\": \"1\"," +
            "        \"key\": \"1\"," +
            "        \"value\": {" +
            "            \"rev\": \"43-4886b6a3da60a647adea18b1c6c81cd5\"" +
            "        }" +
            "    }," +
            "    {" +
            "        \"id\": \"2\"," +
            "        \"key\": \"2\"," +
            "        \"value\": {" +
            "            \"rev\": \"42-e7620ba0ea71c48f6a11bacee4999d79\"" +
            "        }" +
            "    }" +
            "]";

        public static readonly string ViewQueryComplexKeysRows = "[" +
            "    {\"id\":\"complex:1\",\"key\":[\"test1\",1,3.14,\"2013-09-22T22:36:00\"],\"value\":null}," +
            "    {\"id\":\"complex:2\",\"key\":[\"test2\",2,3.15,\"2013-09-22T22:37:00\"],\"value\":null}" +
            "]";

        public static readonly string ViewQueryComplexKeyWithArray = "[" +
            "    {" +
            "        \"id\": \"e5c0dce2ef044d2815f4c83aa00000e4\"," +
            "        \"key\": [" +
            "            [" +
            "                \"a\"," +
            "                \"b\"" +
            "            ]," +
            "            123234" +
            "        ]," +
            "        \"value\": {" +
            "            \"_id\": \"e5c0dce2ef044d2815f4c83aa00000e4\"," +
            "            \"_rev\": \"1-5c4be5d43287f2a8b6121d2720e44d57\"," +
            "            \"strings\": [" +
            "                \"a\"," +
            "                \"b\"" +
            "            ]," +
            "            \"ts\": 123234" +
            "        }" +
            "    }" +
            "]";

        public static readonly string ViewQuerySingleValueKeysRows = "[" +
            "    {\"id\":\"integer:1\",\"key\":1,\"value\":null}," +
            "    {\"id\":\"float:1\",\"key\":3.14,\"value\":null}," +
            "    {\"id\":\"datetime:1\",\"key\":\"2013-09-22T22:36:00\",\"value\":null}," +
            "    {\"id\":\"string:1\",\"key\":\"test1\",\"value\":null}" +
            "]";
    }
}
