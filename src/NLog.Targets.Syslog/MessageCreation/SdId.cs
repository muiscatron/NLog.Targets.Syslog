using NLog.Layouts;
using NLog.Targets.Syslog.Policies;
using System.Collections.Generic;
using System.Linq;

namespace NLog.Targets.Syslog.MessageCreation
{
    public class SdId : SimpleLayout
    {
        private static readonly InternalLogDuplicatesPolicy DuplicatesPolicy;
        private SdIdPolicySet sdIdPolicySet;

        static SdId()
        {
            DuplicatesPolicy = new InternalLogDuplicatesPolicy();
        }

        internal void Initialize(Enforcement enforcement)
        {
            sdIdPolicySet = new SdIdPolicySet(enforcement);
        }

        internal static IEnumerable<IEnumerable<byte>> Bytes(IEnumerable<SdId> sdIds, LogEventInfo logEvent, EncodingSet encodings)
        {
            return InternalLogDuplicatesPolicy.Apply(sdIds, x => x.Render(logEvent))
                .Select(x => x.Bytes(logEvent, encodings));
        }

        private IEnumerable<byte> Bytes(LogEventInfo logEvent, EncodingSet encodings)
        {
            var sdId = sdIdPolicySet.Apply(Render(logEvent));
            return encodings.Ascii.GetBytes(sdId);
        }
    }
}