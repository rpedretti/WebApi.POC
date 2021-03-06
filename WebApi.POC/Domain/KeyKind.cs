﻿using System.Collections.Generic;
using WebApi.Shared.Domain;

namespace WebApi.POC.Domain
{
    /// <summary>
    /// Representes a key kind (Private or Public)
    /// </summary>
    public sealed class KeyKind : Enumeration
    {
        /// <summary>
        /// Publick RSA key
        /// </summary>
        public static KeyKind PUBLIC = new KeyKind(1, "PUBLIC");

        /// <summary>
        /// Private RSA Key
        /// </summary>
        public static KeyKind PRIVATE = new KeyKind(2, "PRIVATE");

        private KeyKind() { }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="id">Key kind Id</param>
        /// <param name="name">Key kind Name</param>
        public KeyKind(int id, string name) : base(id, name) { }

        /// <summary>
        /// Return a list of all key kinds
        /// </summary>
        /// <returns>Return a list of all key kinds</returns>
        public static IEnumerable<KeyKind> List()
        {
            return new[] { PUBLIC, PRIVATE };
        }
    }
}
