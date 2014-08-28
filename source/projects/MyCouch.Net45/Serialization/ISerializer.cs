using System;
using System.Collections.Generic;
using System.IO;

namespace MyCouch.Serialization
{
	public interface ISerializer
	{
		string Serialize(object item);
		string Serialize<T>(T item) where T : class;
		T Deserialize<T>(string data);
		T Deserialize<T>(Stream data);
		T DeserializeCopied<T>(Stream data) where T : class;
		void Populate<T>(T item, Stream data) where T : class;
		void Populate<T>(T item, string json) where T : class;

		string ToJson(object value);
		string ToJson(bool value);
		string ToJson(int value);
		string ToJson(long value);
		string ToJson(float value);
		string ToJson(double value);
		string ToJson(decimal value);
		string ToJson(DateTime value);
		string ToJsonArray<T>(IEnumerable<T> value);
	}
}