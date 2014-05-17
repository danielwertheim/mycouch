using MyCouch;

namespace MrLab
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var client = new MyCouchClient("http://sa:test@localhost:5984/a"))
            {

            }
        }
    }
}
