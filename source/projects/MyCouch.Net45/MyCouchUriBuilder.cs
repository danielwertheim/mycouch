using System;
using EnsureThat;
using MyCouch.Extensions;

namespace MyCouch
{
    public class MyCouchUriBuilder
    {
        protected string Scheme;
        protected string Authority;
        protected string DbName;
        protected string Username;
        protected string Password;

        protected virtual bool ShouldUseBasicCredentials
        {
            get { return !string.IsNullOrWhiteSpace(Username); }
        }

        public MyCouchUriBuilder(string serverUri)
        {
            Ensure.That(serverUri, "serverUri").IsNotNullOrWhiteSpace();

            var tmp = new Uri(serverUri);
            Scheme = tmp.Scheme;
            Authority = tmp.Authority.Replace("/", string.Empty);
            DbName = tmp.LocalPath.Replace("/", string.Empty);
        }

        public virtual MyCouchUriBuilder SetDbName(string dbname)
        {
            Ensure.That(dbname, "dbname").IsNotNullOrWhiteSpace();

            DbName = dbname.Replace("/", string.Empty);

            return this;
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
            var url = string.Format("{0}://{1}{2}/{3}",
                Scheme,
                GetCredentialsForBuild(),
                Authority,
                DbName).RemoveTrailing("/");

            return new Uri(url);
        }

        protected virtual string GetCredentialsForBuild()
        {
            return ShouldUseBasicCredentials
                ? string.Format("{0}:{1}@",
                    Uri.EscapeDataString(Username),
                    Uri.EscapeDataString(Password)) 
                : string.Empty;
        }
    }
}