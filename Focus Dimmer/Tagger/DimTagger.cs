using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Operations;
using Microsoft.VisualStudio.Text.Tagging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Focus_Dimmer.Tagger
{
    public sealed class DimTagger : ITagger<ClassificationTag>
    {
        private ITextBuffer m_SourceBuffer { get; set; }
        private readonly ITextView m_View;
        private CaretPosition m_CaretPosition;
        private NormalizedSnapshotSpanCollection m_CurrentSpans;
        private ClassificationTag m_Tag;
        public event EventHandler<SnapshotSpanEventArgs> TagsChanged;

        public DimTagger(ITextView view, ITextBuffer sourceBuffer, IClassificationTypeRegistryService registry)
        {
            m_View = view;
            m_CaretPosition = view.Caret.Position;
            m_CurrentSpans = GetBlockSpans(m_View.TextSnapshot);
            m_SourceBuffer = sourceBuffer;
            FocusDimmer.Toggled += SetupSelectionChangedListener;
            m_View.GotAggregateFocus += SetupSelectionChangedListener;
            m_View.Caret.PositionChanged += CaretPositionChanged;
            m_View.LayoutChanged += ViewLayoutChanged;
            m_Tag = BuildTag(registry, "Alpzy/DimFormatDefinition");
        }

        private static ClassificationTag BuildTag(IClassificationTypeRegistryService classificationRegistry, string typeName)
        {
            return new ClassificationTag(classificationRegistry.GetClassificationType(typeName));
        }

        void CaretPositionChanged(object sender, CaretPositionChangedEventArgs e)
        {
            m_CaretPosition = e.NewPosition;
            UpdateAtCaretPosition(e.NewPosition);
        }

        void UpdateAtCaretPosition(CaretPosition caretPosition)
        {
            UpdateTags(GetBlockSpans(caretPosition.BufferPosition.Snapshot));
        }

        private void SetupSelectionChangedListener(object sender, EventArgs e)
        {
            if (m_View != null)
            {
                m_View.LayoutChanged += ViewLayoutChanged;
                m_View.GotAggregateFocus -= SetupSelectionChangedListener;
            }
        }

        private void ViewLayoutChanged(object sender, TextViewLayoutChangedEventArgs e)
        {
            UpdateTags(GetBlockSpans(e.NewSnapshot));
        }

        private void UpdateTags(NormalizedSnapshotSpanCollection newSpans)
        {
            m_CurrentSpans = newSpans;
            TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(m_SourceBuffer.CurrentSnapshot, 0, m_SourceBuffer.CurrentSnapshot.Length)));
        }

        private NormalizedSnapshotSpanCollection GetBlockSpans(ITextSnapshot snapshot)
        {
            var blockSpans = new List<SnapshotSpan>();

            if (!FocusDimmer.IsOn) 
            {
                SnapshotPoint pointZero = new SnapshotPoint(snapshot, 0);
                SnapshotSpan spanZero = new SnapshotSpan(pointZero, pointZero);
                return new NormalizedSnapshotSpanCollection(new List<SnapshotSpan>() { spanZero });
            }

            Stack<SnapshotPoint> blockStack = new Stack<SnapshotPoint>();
            foreach(var line in snapshot.Lines)
            {
                string lineText = line.GetText();
                string lineTextTrimmed = lineText.Trim();

                int startIndex = lineTextTrimmed.LastIndexOf('{');
                int endIndex = lineTextTrimmed.LastIndexOf('}');

                if (startIndex > -1)
                {
                    blockStack.Push(new SnapshotPoint(line.Snapshot, line.LineNumber - 1 == 0 ? line.Start + startIndex : snapshot.GetLineFromLineNumber(line.LineNumber - 1).Start));
                }
                if (endIndex > -1)
                {
                    blockSpans.Add(new SnapshotSpan(blockStack.Pop(), new SnapshotPoint(line.Snapshot, line.End)));
                }
            }

            var caretBlockSpan = blockSpans.Where(x => x.Start.Position < m_CaretPosition.BufferPosition.Position && x.End.Position > m_CaretPosition.BufferPosition.Position)
                .OrderBy(x => x.Length)
                .FirstOrDefault();

            List<SnapshotSpan> result = new List<SnapshotSpan>();

            //Avoiding unnecessary exceptions
            try
            {
                result.Add(new SnapshotSpan(new SnapshotPoint(snapshot, 0), caretBlockSpan.Start));
                result.Add(new SnapshotSpan(caretBlockSpan.End, new SnapshotPoint(snapshot, snapshot.Length - 1)));
            }
            catch
            {
            }

            return new NormalizedSnapshotSpanCollection(result);
        }
            
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans == null || spans.Count == 0 || m_CurrentSpans.Count == 0)
                yield break;
            
            //Avoiding unnecessary exceptions
            try
            {
                NormalizedSnapshotSpanCollection.Overlap(spans, m_CurrentSpans);
            }
            catch
            {
                yield break;
            }

            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, m_CurrentSpans))
            {
                yield return new TagSpan<ClassificationTag>(span, m_Tag);
            }
        }
    }
}
