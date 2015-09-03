using System;
using System.Text;
using MyCouch.EnsureThat;

namespace MyCouch.Net
{
    public class BasicAuthString
    {
        public string Value { get; private set; }

        public BasicAuthString(string username, string password)
        {
            Ensure.That(username, "username").IsNotNullOrEmpty();
            Ensure.That(password, "password").IsNotNullOrEmpty();

            Value = GenerateBasicAuthorizationCredentials(username, password);
        }

        private string GenerateBasicAuthorizationCredentials(string username, string password)
        {
            var credentialsBytes = Encoding.UTF8.GetBytes(string.Format("{0}:{1}", username, password));

            return Convert.ToBase64String(credentialsBytes);
        }

        public static implicit operator string(BasicAuthString item)
        {
            return item.Value;
        }

        public override string ToString()
        {
            return Value;
        }
    }
}