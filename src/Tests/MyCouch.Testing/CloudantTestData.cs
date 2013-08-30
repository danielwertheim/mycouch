using MyCouch.Testing.Model;

namespace MyCouch.Testing
{
    public static class CloudantTestData
    {
        public static class Animals
        {
            public static Animal Panda = new Animal
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

            public static Animal Zebra = new Animal
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

            public static Animal Snipe = new Animal
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

            public static Animal Llama = new Animal
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

            public static Animal Lemur = new Animal
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

            public static Animal KookaBurra = new Animal
            {
                AnimalId = "kookaburra",
                Class = "bird",
                LatinName = "Dacelo novaeguineae",
                Diet = "carnivore",
                MinLength = 0.28,
                MaxLength = 0.42,
                WikiPage = "http://en.wikipedia.org/wiki/Kookaburra"
            };

            public static Animal Giraffe = new Animal
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

            public static Animal Elephant = new Animal
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

            public static Animal Badger = new Animal
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

            public static Animal Aardvark = new Animal
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
}