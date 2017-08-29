using System;
using System.Collections.Generic;

namespace FlatsLib {
    internal interface IFlatMailParser {
        Func<Mail, bool> Validator { get; }
        Func<Mail, IEnumerable<Flat>> Algorithm { get; }
    }
}