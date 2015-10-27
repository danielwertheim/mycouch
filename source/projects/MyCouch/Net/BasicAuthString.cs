﻿using System;
using MyCouch.EnsureThat;
using MyCouch.Extensions;

namespace MyCouch.Net
{
    public class BasicAuthString : IEquatable<string>, IEquatable<BasicAuthString>
    {
        public string Value { get; }

        public BasicAuthString(string username, string password)
        {
            Ensure.That(username, "username").IsNotNullOrEmpty();
            Ensure.That(password, "password").IsNotNullOrEmpty();

            Value = GenerateBasicAuthorizationCredentials(username, password);
        }

        private string GenerateBasicAuthorizationCredentials(string username, string password)
        {
            return $"{username}:{password}".AsBase64Encoded();
        }

        public static implicit operator string(BasicAuthString item)
        {
            return item.Value;
        }

        public override bool Equals(object obj)
        {
            if (obj is string)
                return Equals(obj as string);

            if (obj is BasicAuthString)
                return Equals(obj as BasicAuthString);

            return false;
        }

        public bool Equals(BasicAuthString other)
        {
            return Equals(other.Value);
        }

        public bool Equals(string other)
        {
            return string.Equals(Value, other);
        }

        public override int GetHashCode()
        {
            return Value.GetHashCode();
        }

        public override string ToString()
        {
            return Value;
        }
    }
}