using FakeItEasy;

namespace MyCouch.Testing
{
    public static class DoFake
    {
         public static T This<T>()
         {
             return A.Fake<T>();
         }
    }
}