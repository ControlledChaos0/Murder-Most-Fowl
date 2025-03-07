using System.Collections.Generic;
using UnityEngine;

namespace Yarn.Unity
{
    public class MMFTextLineProvider : TextLineProvider
    {
        public override LocalizedLine GetLocalizedLine(Yarn.Line line)
        {
            var text = YarnProject.GetLocalization(textLanguageCode).GetLocalizedString(line.ID);
            var localLine = new LocalizedLine()
            {
                TextID = line.ID,
                RawText = text,
                Substitutions = line.Substitutions,
                Metadata = YarnProject.lineMetadata.GetMetadata(line.ID),
            };

            DialogueHelper.Instance.UpdateDialogueUI(localLine);

            return localLine;
        }

        public override void PrepareForLines(IEnumerable<string> lineIDs)
        {
            // No-op; text lines are always available
        }

        public override bool LinesAvailable => true;

        public override string LocaleCode => textLanguageCode;
    }
}