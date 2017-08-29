using System;
using System.Collections.Generic;
using System.Linq;

namespace FlatsLib {
    internal static class MailParser {

        private static readonly List<IFlatMailParser> _parsers;

        static MailParser() {
            _parsers = new List<IFlatMailParser> {
                new CianNewOffersParser()
            };
        }

        internal static IEnumerable<Flat> ParseFlats(Mail mail) {
            if (!IsConsistentMail(mail))
                throw new Exception("Broken mail data");
            if (!FindParser(mail, out Func<Mail, IEnumerable<Flat>> parsingAlgorithm)) {
                return Enumerable.Empty<Flat>();
            }
            return parsingAlgorithm(mail);
        }

        private static bool FindParser(Mail mail, out Func<Mail, IEnumerable<Flat>> parsingAlgorithm) {
            foreach (var parser in _parsers) {
                if (parser.Validator(mail) == true) {
                    parsingAlgorithm = parser.Algorithm;
                    return true;
                }
            }
            parsingAlgorithm = null;
            return false;
        }

        private static bool IsConsistentMail(Mail mail) {
            return (mail.From != null 
                && mail.Subject != null 
                && mail.Body != null 
                && mail.DateTime != null);
        }
    }
}