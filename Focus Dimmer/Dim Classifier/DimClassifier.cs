using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;

namespace Focus_Dimmer.Dim_Classifier
{
    internal class DimClassifier : IClassifier
    {

        private readonly IClassificationType classificationType;

        internal DimClassifier(IClassificationTypeRegistryService registry)
        {
            this.classificationType = registry.GetClassificationType("DimClassifier");
        }

        #region IClassifier

        public event EventHandler<ClassificationChangedEventArgs> ClassificationChanged;

        public IList<ClassificationSpan> GetClassificationSpans(SnapshotSpan span)
        {
            var result = new List<ClassificationSpan>()
            {
                new ClassificationSpan(new SnapshotSpan(span.Snapshot, new Span(span.Start, span.Length)), this.classificationType)
            };

            return result;
        }

        #endregion
    }
}
