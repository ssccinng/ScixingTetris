using System;

namespace Hikari.AI.Utils.Throws {
    public class SpeculationBrokeException : Exception {
        public SpeculationBrokeException() : base("Speculation machine broke") { }
    }
}