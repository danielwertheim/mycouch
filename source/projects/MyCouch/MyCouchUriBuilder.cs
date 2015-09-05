using System;
using MyCouch.EnsureThat;
using MyCouch.Extensions;
using MyCouch.Net;

namespace MyCouch
{
    public class MyCouchUriBuilder
    {
        protected string Scheme;
        protected string Authority;
        protected string Username;
        protected string Password;

        protected virtual bool ShouldUseBasicCredentials => !string.IsNullOrWhiteSpace(Username);

        public MyCouchUriBuilder(string serverUri)
        {
            Ensure.That(serverUri, "serverUri").IsNotNullOrWhiteSpace();

            var tmp = new Uri(serverUri);
            Scheme = tmp.Scheme;
            Authority = tmp.Authority.Replace("/", string.Empty);
        }

        public virtual MyCouchUriBuilder SetBasicCredentials(string username, string password)
        {
            Ensure.That(username, "username").IsNotNullOrWhiteSpace();
            Ensure.That(password, "password").IsNotNullOrWhiteSpace();

            Username = username;
            Password = password;

            return this;
        }

        public virtual Uri Build()
        {
            var url = $"{Scheme}://{GetCredentialsForBuild()}{Authority}".RemoveTrailing("/");

            return new Uri(url);
        }

        protected virtual string GetCredentialsForBuild()
        {
            return ShouldUseBasicCredentials
                ? $"{UrlParam.Encode(Username)}:{UrlParam.Encode(Password)}@"
                : string.Empty;
        }
    }
}