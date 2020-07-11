using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Tagging;
using System.Linq;

namespace Focus_Dimmer.Utils
{
    public static class TagUtils
    {
        public static bool isStringOrComment(ITextSnapshotLine line, int index, ITagAggregator<IClassificationTag> aggregator)
        {
            return isString(line, index, aggregator) || isComment(line, index, aggregator);
        }

        public static bool isString(ITextSnapshotLine line, int index, ITagAggregator<IClassificationTag> aggregator)
        {
            var tags = aggregator.GetTags(
                new SnapshotSpan(
                    new SnapshotPoint(line.Snapshot, line.Start + index-1 < 0 ? 0 : line.Start + index-1), 
                    new SnapshotPoint(line.Snapshot, line.Start + index)
                )
            );

            return tags.Any(x => x.Tag.ClassificationType.Classification == "string");
        }

        public static bool isComment(ITextSnapshotLine line, int index, ITagAggregator<IClassificationTag> aggregator)
        {
            var tags = aggregator.GetTags(
                new SnapshotSpan(
                    new SnapshotPoint(line.Snapshot, line.Start + index - 1 < 0 ? 0 : line.Start + index - 1),
                    new SnapshotPoint(line.Snapshot, line.Start + index)
                )
            );
            return tags.Any(x => x.Tag.ClassificationType.Classification == "comment");
        }

        public static bool ContainsTag(ITextSnapshotLine line, string classificationName, ITagAggregator<IClassificationTag> aggregator)
        {
            var tags = aggregator.GetTags(
                new SnapshotSpan(
                    new SnapshotPoint(line.Snapshot, line.Start),
                    new SnapshotPoint(line.Snapshot, line.End)
                )
            );
            return tags.Any(x => x.Tag.ClassificationType.Classification == classificationName);
        }
    }
}
