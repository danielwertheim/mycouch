﻿using System;
using System.Collections.Generic;

namespace MyCouch
{
#if !PCL && !vNext
    [Serializable]
#endif
    public enum ChangesStyle
    {
        AllDocs,
        MainOnly
    }

    public static class ChangesStyleEnumExtensions
    {
        private static readonly Dictionary<ChangesStyle, string> Mappings;

        static ChangesStyleEnumExtensions()
        {
            Mappings = new Dictionary<ChangesStyle, string> {
                { ChangesStyle.AllDocs, "all_docs" },
                { ChangesStyle.MainOnly, "main_only" }
            };
        }

        public static string AsString(this ChangesStyle style)
        {
            return Mappings[style];
        }
    }
}