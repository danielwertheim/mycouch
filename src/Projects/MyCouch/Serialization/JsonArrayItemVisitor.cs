using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using EnsureThat;
using MyCouch.Serialization.Writers;
using Newtonsoft.Json;

namespace MyCouch.Serialization
{
    /// <summary>
    /// Used to read a JSON tree representing array items.
    /// </summary>
    public class JsonArrayItemVisitor
    {
        public delegate void OnVisitMember<in T>(T item, JsonReader jr, JsonWriter jw, StringBuilder sb) where T : class;

        protected readonly SerializationConfiguration Configuration;

        public JsonArrayItemVisitor(SerializationConfiguration configuration)
        {
            Ensure.That(configuration, "configuration").IsNotNull();

            Configuration = configuration;
        }

        /// <summary>
        /// Traverses JSON-array items of sent <paramref name="jsonReader"/>, and invokes visitor
        /// callbacks depending on what nodes that are being found in the tree.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonReader"></param>
        /// <param name="onVisitStartNode"></param>
        /// <param name="onVisitEndNode"></param>
        /// <param name="memberHandlers"></param>
        /// <returns></returns>
        public virtual IEnumerable<T> Visit<T>(JsonReader jsonReader, Func<T> onVisitStartNode, Func<T, T> onVisitEndNode, IDictionary<string, OnVisitMember<T>> memberHandlers) where T : class
        {
            Ensure.That(jsonReader, "jsonReader").IsNotNull();
            Ensure.That(jsonReader.TokenType == JsonToken.StartArray, "jsonReader").WithExtraMessageOf(
                () => ExceptionStrings.QueryResponseRowsDeserializerNeedsJsonArray);
            Ensure.That(memberHandlers, "memberHandlers").HasItems();

            var startDepth = jsonReader.Depth;
            var itemDepth = startDepth + 1;
            var memDepth = itemDepth + 1;
            var sb = new StringBuilder();
            var numOfHandlersProcessed = 0;
            T currentItem = null;

            using (var sw = new StringWriter(sb))
            {
                using (var jw = Configuration.ApplyConfigToWriter(new MyCouchJsonWriter(sw)))
                {
                    while (jsonReader.Read() && !(jsonReader.TokenType == JsonToken.EndArray && jsonReader.Depth == startDepth))
                    {
                        if (jsonReader.Depth == itemDepth && jsonReader.TokenType == JsonToken.StartObject)
                        {
                            currentItem = onVisitStartNode();
                            continue;
                        }

                        if (jsonReader.Depth == itemDepth && jsonReader.TokenType == JsonToken.EndObject)
                        {
                            yield return onVisitEndNode(currentItem);
                            numOfHandlersProcessed = 0;
                            currentItem = null;
                            continue;
                        }

                        if(jsonReader.Depth != memDepth)
                            continue;
                        
                        if (jsonReader.TokenType != JsonToken.PropertyName)
                            continue;

                        if (numOfHandlersProcessed == memberHandlers.Count)
                            continue;

                        var memName = jsonReader.Value.ToString().ToLower();
                        if (memberHandlers.ContainsKey(memName))
                        {
                            memberHandlers[memName](currentItem, jsonReader, jw, sb);
                            sb.Clear();
                        }

                        numOfHandlersProcessed++;
                    }
                }
            }
        }
    }
}