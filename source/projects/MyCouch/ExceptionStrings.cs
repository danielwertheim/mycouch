﻿namespace MyCouch
{
    public static class ExceptionStrings
    {
        public static string ServerClientSeemsToConnectToDb
        {
            get { return "The URI: {0}, seems to specify a DB. A server client should not have a DB specified."; }
        }

        public static string JsonConverterDoesNotSupportSerialization
        {
            get { return "This JSON-converter does only support deserialization, NOT serialization."; }
        }

        public static string GetChangesForNonContinuousFeedOnly
        {
            get { return "This method only supports Normal and Longpolling feeds. Please use other overload for e.g. Continuous feed."; }
        }

        public static string GetContinuousChangesInvalidFeed
        {
            get { return "This method only supports Continuous feeds. Please use other overload for e.g. Normal or Longpolling feed."; }
        }
    }
}
