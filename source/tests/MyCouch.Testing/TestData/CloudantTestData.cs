﻿using MyCouch.Cloudant;
using MyCouch.Testing.Model;

namespace MyCouch.Testing.TestData
{
    public static class CloudantTestData
    {
        public static class Animals
        {
            public static Animal[] CreateAll()
            {
                return new[]
                {
                    CreatePanda(), CreateZebra(),
                    CreateSnipe(), CreateLlama(),
                    CreateLemur(), CreateKookaBurra(),
                    CreateGiraffe(), CreateElephant(),
                    CreateBadger(), CreateAardvark()
                };
            }

            public static Animal CreatePanda()
            {
                return new Animal
                {
                    AnimalId = "panda",
                    Class = "mammal",
                    Diet = "carnivore",
                    MinLength = 1.2,
                    MaxLength = 1.8,
                    MinWeight = 75,
                    MaxWeight = 115,
                    WikiPage = "http://en.wikipedia.org/wiki/Panda"
                };
            }

            public static Animal CreateZebra()
            {
                return new Animal
                {
                    AnimalId = "zebra",
                    Class = "mammal",
                    Diet = "herbivore",
                    MinLength = 2,
                    MaxLength = 2.5,
                    MinWeight = 175,
                    MaxWeight = 387,
                    WikiPage = "http://en.wikipedia.org/wiki/Plains_zebra"
                };
            }

            public static Animal CreateSnipe()
            {
                return new Animal
                {
                    AnimalId = "snipe",
                    Class = "bird",
                    LatinName = "Gallinago gallinago",
                    Diet = "omnivore",
                    MinLength = 0.25,
                    MaxLength = 0.27,
                    MinWeight = 0.08,
                    MaxWeight = 0.14,
                    WikiPage = "http://en.wikipedia.org/wiki/Common_Snipe"
                };
            }

            public static Animal CreateLlama()
            {
                return new Animal
                {
                    AnimalId = "llama",
                    Class = "mammal",
                    LatinName = "Lama glama",
                    Diet = "herbivore",
                    MinLength = 1.7,
                    MaxLength = 1.8,
                    MinWeight = 130,
                    MaxWeight = 200,
                    WikiPage = "http://en.wikipedia.org/wiki/Llama"
                };
            }

            public static Animal CreateLemur()
            {
                return new Animal
                {
                    AnimalId = "lemur",
                    Class = "mammal",
                    Diet = "omnivore",
                    MinLength = 0.95,
                    MaxLength = 1.1,
                    MinWeight = 2.2,
                    MaxWeight = 2.2,
                    WikiPage = "http://en.wikipedia.org/wiki/Ring-tailed_lemur"
                };
            }

            public static Animal CreateKookaBurra()
            {
                return new Animal
                {
                    AnimalId = "kookaburra",
                    Class = "bird",
                    LatinName = "Dacelo novaeguineae",
                    Diet = "carnivore",
                    MinLength = 0.28,
                    MaxLength = 0.42,
                    WikiPage = "http://en.wikipedia.org/wiki/Kookaburra"
                };
            }

            public static Animal CreateGiraffe()
            {
                return new Animal
                {
                    AnimalId = "giraffe",
                    Class = "mammal",
                    Diet = "herbivore",
                    MinLength = 5,
                    MaxLength = 6,
                    MinWeight = 830,
                    MaxWeight = 1600,
                    WikiPage = "http://en.wikipedia.org/wiki/Giraffe"
                };
            }

            public static Animal CreateElephant()
            {
                return new Animal
                {
                    AnimalId = "elephant",
                    Class = "mammal",
                    Diet = "herbivore",
                    MinLength = 3.2,
                    MaxLength = 4,
                    MinWeight = 4700,
                    MaxWeight = 6050,
                    WikiPage = "http://en.wikipedia.org/wiki/African_elephant"
                };
            }

            public static Animal CreateBadger()
            {
                return new Animal
                {
                    AnimalId = "badger",
                    Class = "mammal",
                    LatinName = "Meles meles",
                    Diet = "omnivore",
                    MinLength = 0.6,
                    MaxLength = 0.9,
                    MinWeight = 7,
                    MaxWeight = 30,
                    WikiPage = "http://en.wikipedia.org/wiki/Badger"
                };
            }

            public static Animal CreateAardvark()
            {
                return new Animal
                {
                    AnimalId = "aardvark",
                    Class = "mammal",
                    LatinName = "Orycteropus afer",
                    Diet = "omnivore",
                    MinLength = 1,
                    MaxLength = 2.2,
                    MinWeight = 40,
                    MaxWeight = 65,
                    WikiPage = "http://en.wikipedia.org/wiki/Aardvark"
                };
            }
        }

        public static class Views
        {
            public static readonly ViewIdentity[] AllViewIds;

            static Views()
            {
                AllViewIds = new[]
                {
                    Views101LatinNameJsSumId,
                    Views101LatinNameId,
                    Views101DietSumId,
                    Views101DietCountId,
                    Views101ComplexCountId,
                    Views101DietId,
                    Views101ComplexLatinNameCountId,
                    Views101DietJsCountId,
                    Views101LatinNameCountId,
                    Views101LatinNameSumId
                };
            }

            public const string Views101 =
                "{" +
                    "\"_id\": \"_design/views101\"," +
                    "\"language\": \"javascript\"," +
                    "\"views\": {" +
                        "\"latin_name_jssum\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"," +
                            "\"reduce\": \"function (key, values, rereduce){\\n  return sum(values);\\n}\"" +
                        "}," +
                        "\"latin_name\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"" +
                        "}," +
                        "\"diet_sum\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet){\\n    emit(doc.diet, 1);\\n  }\\n}\"," +
                            "\"reduce\": \"_sum\"" +
                        "}," +
                        "\"diet_count\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet && doc.latinName){\\n    emit(doc.diet, doc.latinName);\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"complex_count\": {" +
                            "\"map\": \"function(doc){\\n  if(doc.class && doc.diet){\\n    emit([doc.class, doc.diet], 1);\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"diet\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet){\\n    emit(doc.diet, 1);\\n  }\\n}\"" +
                        "}," +
                        "\"complex_latin_name_count\": {" +
                            "\"map\": \"function(doc){\\n  if(doc.latinName){\\n    emit([doc.class, doc.diet, doc.latinName], doc.latinName.length)\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"diet_jscount\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.diet){\\n    emit(doc.diet, 1);\\n  }\\n}\"," +
                            "\"reduce\": \"function (key, values, rereduce){\\n  return values.length;\\n}\"" +
                        "}," +
                        "\"latin_name_count\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"," +
                            "\"reduce\": \"_count\"" +
                        "}," +
                        "\"latin_name_sum\": {" +
                            "\"map\": \"function(doc) {\\n  if(doc.latinName){\\n    emit(doc.latinName, doc.latinName.length);\\n  }\\n}\"," +
                            "\"reduce\": \"_sum\"" +
                        "}" +
                    "}," +
                    "\"indexes\": {" +
                        "\"animals\": {" +
                            "\"index\": \"function(doc){\\n" +
                            "  index('default', doc._id);\\n" +
                            "  if (doc.minLength){\\n    index('minLength', doc.minLength, {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.maxLength){\\n    index('maxLength', doc.maxLength, {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.diet){\\n    index('diet', doc.diet, {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc.latinName){\\n    index('latinName', doc.latinName, {\\\"store\\\": \\\"yes\\\"});\\n  }\\n" +
                            "  if (doc['class']){\\n    index('class', doc['class'], {\\\"facet\\\": true, \\\"store\\\": \\\"yes\\\"});\\n  }\\n}\"" +
                        "}" +
                    "}" +
                "}";
            public static readonly ViewIdentity Views101LatinNameJsSumId = new ViewIdentity("views101", "latin_name_jssum");
            public static readonly ViewIdentity Views101LatinNameId = new ViewIdentity("views101", "latin_name");
            public static readonly ViewIdentity Views101DietSumId = new ViewIdentity("views101", "diet_sum");
            public static readonly ViewIdentity Views101DietCountId = new ViewIdentity("views101", "diet_count");
            public static readonly ViewIdentity Views101ComplexCountId = new ViewIdentity("views101", "complex_count");
            public static readonly ViewIdentity Views101DietId = new ViewIdentity("views101", "diet");
            public static readonly ViewIdentity Views101ComplexLatinNameCountId = new ViewIdentity("views101", "complex_latin_name_count");
            public static readonly ViewIdentity Views101DietJsCountId = new ViewIdentity("views101", "diet_jscount");
            public static readonly ViewIdentity Views101LatinNameCountId = new ViewIdentity("views101", "latin_name_count");
            public static readonly ViewIdentity Views101LatinNameSumId = new ViewIdentity("views101", "latin_name_sum");
            public static readonly SearchIndexIdentity Views101AnimalsSearchIndexId = new SearchIndexIdentity("views101", "animals");
        }

        public static class Blogs
        {
            public static Blog[] CreateAll()
            {
                return new [] 
                {
                    CreateJsonBlog(),
                    CreateCouchBlog(),
                    CreateHtml5Blog()
                };
            }

            private static Blog CreateHtml5Blog()
            {
                return new Blog
                {
                    Title = "Html5 blog",
                    Author = new Author { Age = 21, Name = "Html5 Author" },
                    Entries = new[]
                    {
                        new BlogEntry { Sequence = 1, Subject = "Semantic tags", Content = "Content on semantic tags" },
                        new BlogEntry { Sequence = 2, Subject = "Media elements", Content = "Content on media elements" }
                    },
                    YearsActive = 2
                };
            }

            private static Blog CreateCouchBlog()
            {
                return new Blog
                {
                    Title = "Couch blog",
                    Author = new Author { Age = 32, Name = "Couch Author" },
                    Entries = new[]
                    {
                        new BlogEntry { Sequence = 1, Subject = "Primary indexes", Content = "Content on primary indexes" }
                    },
                    YearsActive = 4
                };
            }

            private static Blog CreateJsonBlog()
            {
                return new Blog
                {
                    Title = "Json blog",
                    Author = new Author { Age = 43, Name = "Json Author" },
                    Entries = new[]
                    {
                        new BlogEntry { Sequence = 1, Subject = "Json syntax", Content = "Content on Json syntax" },
                        new BlogEntry { Sequence = 2, Subject = "Json arrays", Content = "Content on Json arrays" }
                    },
                    YearsActive = 5
                };
            }
        }
    }
}