using System.IO;
using System.Net.Http;
using System.Text;
using MyCouch.Extensions;

namespace MyCouch.Responses.Factories.Materializers
{
    public class TextResponseMaterializer
    {
        public virtual void Materialize(TextResponse response, HttpResponseMessage httpResponse)
        {
            SetContent(response, httpResponse);
        }

        protected virtual async void SetContent(TextResponse response, HttpResponseMessage httpResponse)
        {
            using (var content = await httpResponse.Content.ReadAsStreamAsync().ForAwait())
            {
                var sb = new StringBuilder();

                using (var reader = new StreamReader(content, MyCouchRuntime.DefaultEncoding))
                {
                    while (!reader.EndOfStream)
                        sb.Append(reader.ReadLine());
                }

                response.Content = sb.ToString();

                sb.Clear();
            }
        }
    }
}