using Focus_Dimmer.Enums;
using Focus_Dimmer.Utils;
using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;

namespace Focus_Dimmer.Tagger
{
    public sealed class DimTagger : ITagger<ClassificationTag>
    {
        private readonly ITextView m_View;
        private CaretPosition m_CaretPosition;
        private ITextBuffer m_SourceBuffer { get; set; }
        private IClassificationTypeRegistryService m_Registry;
        private IViewTagAggregatorFactoryService m_TagAggregatorService;
        private ClassificationTag m_Tag;

        private NormalizedSnapshotSpanCollection m_CurrentSpans;

        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public DimTagger(ITextView view, ITextBuffer sourceBuffer, IClassificationTypeRegistryService registry, IViewTagAggregatorFactoryService tagAggregatorService)
        {
            m_View = view;
            m_CaretPosition = m_View.Caret.Position;
            m_SourceBuffer = sourceBuffer;
            m_Registry = registry;
            m_TagAggregatorService = tagAggregatorService;

            m_SourceBuffer.PostChanged += UpdateOnEvent;
            m_View.Caret.PositionChanged += CaretPositionChanged;

            FocusDimmer.ToggledOnOff += UpdateOnEvent;
            FocusDimmer.ToggledMode += UpdateOnEvent;
        }

        private static ClassificationTag BuildTag(IClassificationTypeRegistryService classificationRegistry, string typeName)
        {
             return new ClassificationTag(classificationRegistry.GetClassificationType(typeName));
        }

        void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            m_CaretPosition = e.NewPosition;
            UpdateTags(GetDimSpans(m_CaretPosition.BufferPosition.Snapshot));
        }

        void UpdateOnEvent(object sender, EventArgs e)
        {
            UpdateTags(GetDimSpans(m_View.TextSnapshot));
        }

        private void UpdateTags(NormalizedSnapshotSpanCollection newSpans)
        {
            m_Tag = FocusDimmer.Mode == Modes.DimGray 
                ? BuildTag(m_Registry, "Alpzy/DimGray") 
                : BuildTag(m_Registry, "Alpzy/Transparent");

            m_CurrentSpans = newSpans;

            TagsChanged?.Invoke(
                this, 
                new SnapshotSpanEventArgs(
                    new SnapshotSpan(m_SourceBuffer.CurrentSnapshot, 0, m_SourceBuffer.CurrentSnapshot.Length)
                    )
                );
        }

        private NormalizedSnapshotSpanCollection GetDimSpans(ITextSnapshot snapshot)
        {
            if (!FocusDimmer.IsOn)
            {
                SnapshotPoint pointZero = new SnapshotPoint(snapshot, 0);
                SnapshotSpan spanZero = new SnapshotSpan(pointZero, pointZero);
                return new NormalizedSnapshotSpanCollection(new List<SnapshotSpan>() { spanZero });
            }

            List<SnapshotSpan> result = new List<SnapshotSpan>();

            SnapshotSpan caretBlockSpan = CodeBlockUtils.GetCaretBlockSpan(m_View, m_CaretPosition, m_TagAggregatorService);

            result.Add(new SnapshotSpan(new SnapshotPoint(snapshot, 0), caretBlockSpan.Start));
            result.Add(new SnapshotSpan(caretBlockSpan.End, new SnapshotPoint(snapshot, snapshot.Length)));

            return new NormalizedSnapshotSpanCollection(result);
        }
            
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans == null || spans.Count == 0 || m_CurrentSpans == null || m_CurrentSpans.Count == 0 || m_CurrentSpans[0].Snapshot != spans[0].Snapshot)
                yield break;
            
            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, m_CurrentSpans))
            {
                yield return new TagSpan<ClassificationTag>(span, m_Tag);
            }
        }
    }
}
