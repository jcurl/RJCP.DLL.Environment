﻿namespace RJCP.Core.Environment.Resources
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;
    using System.Reflection;
    using System.Resources;
    using NUnit.Framework;
    using RJCP.CodeQuality;

    [TestFixture]
    public class ResourcesPrintTest
    {
        private const string MessageResources = "RJCP.Core.Environment.Resources.Messages";

        private static readonly CultureInfo[] Cultures = new CultureInfo[] {
            new("en"),
            new("en-US"),
            new("en-GB"),
            new("en-AU"),
            new("de"),
            new("de-DE"),
            new("de-AT"),
            new("ro")
        };

        private static CultureInfo GetNeutralResourceLanguage()
        {
            Assembly assembly = typeof(Xdg).Assembly;
            if ((Attribute.GetCustomAttribute(assembly, typeof(NeutralResourcesLanguageAttribute))
                is NeutralResourcesLanguageAttribute lang)) {
                return new CultureInfo(lang.CultureName);
            }
            return null;
        }

        [Test]
        public void CheckNeutralResourcesLanguage()
        {
            CultureInfo neutral = GetNeutralResourceLanguage();
            Assert.That(neutral, Is.Not.Null, "Check AssemblyInfo has a NeutralResourcesLanguage");
        }

        [TestCaseSource(nameof(Cultures))]
        public void PrintResources(CultureInfo culture)
        {
            Resources.Print(MessageResources, typeof(Xdg), culture);
        }

        [TestCaseSource(nameof(Cultures))]
        public void MissingResources(CultureInfo culture)
        {
            bool allTranslation = true;
            allTranslation &= CheckMissingResources(MessageResources, typeof(Xdg).Assembly, culture);

            Assert.That(allTranslation, Is.True, $"Some translations missing for {culture}");
        }

        private static bool CheckMissingResources(string baseName, Assembly assembly, CultureInfo culture)
        {
            ResourceManager rsrc = new(baseName, assembly);
            ResourceSet set = rsrc.GetResourceSet(culture, true, true);
            ResourceSet invariant = rsrc.GetResourceSet(GetNeutralResourceLanguage(), true, false);

            HashSet<string> found = new();
            foreach (DictionaryEntry entry in set) {
                string key = entry.Key.ToString();
                if (entry.Value is string resource && !string.IsNullOrWhiteSpace(resource))
                    found.Add(key);
            }
            if (!culture.IsNeutralCulture) {
                ResourceSet parent = rsrc.GetResourceSet(culture.Parent, true, true);
                foreach (DictionaryEntry entry in parent) {
                    string key = entry.Key.ToString();
                    if (entry.Value is string resource && !string.IsNullOrWhiteSpace(resource))
                        found.Add(key);
                }
            }

            Console.WriteLine("Resource: {0}; Culture: {1}; Parent: {2}", baseName, culture.ToString(), culture.Parent.ToString());
            List<string> missing = new();
            foreach (DictionaryEntry entry in invariant) {
                string key = entry.Key.ToString();
                if (!found.Contains(key)) {
                    missing.Add(key);
                }
            }
            var sortedKeys = from key in missing orderby key select key;
            foreach (var key in sortedKeys) {
                Console.WriteLine("Missing Key: {0}", key);
            }

            return missing.Count == 0;
        }
    }
}
