using System;

namespace Hikari.AI.Utils.Throws {
    public class UnreachableException : Exception {
        public UnreachableException() : base("The statement assumed to be unreachable, but the code path reached") { }
    }
}