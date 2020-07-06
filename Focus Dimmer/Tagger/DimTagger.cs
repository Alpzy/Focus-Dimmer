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
            m_SourceBuffer = sourceBuffer;
            FocusDimmer.Toggled += UpdateOnToggle;
            m_View.Caret.PositionChanged += CaretPositionChanged;
            m_Tag = BuildTag(registry, "Alpzy/DimFormatDefinition");
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

        void UpdateOnToggle(object sender, EventArgs e)
        {
            UpdateTags(GetDimSpans(m_View.TextSnapshot));
        }

        private void UpdateTags(NormalizedSnapshotSpanCollection newSpans)
        {

            m_CurrentSpans = newSpans;
            TagsChanged?.Invoke(this, new SnapshotSpanEventArgs(new SnapshotSpan(m_SourceBuffer.CurrentSnapshot, 0, m_SourceBuffer.CurrentSnapshot.Length)));
        }

        private SnapshotPoint GetBlockHeaderStartPoint(ITextSnapshotLine caretLine)
        {
            int closeBraceCount = 1;

            var line = caretLine;
            string lineText;
            var snapshot = caretLine.Snapshot;

            while (line.LineNumber - 1 > 0 && closeBraceCount > 0)
            {
                lineText = line.GetText();

                if (lineText.Contains("}"))
                {
                    int index = lineText.IndexOf("}");
                    while (index >= 0 && line.Start + index < m_View.Caret.Position.BufferPosition)
                    {
                        closeBraceCount++;
                        index = lineText.IndexOf("}", index + 1);
                    }
                }

                if (lineText.Contains("{"))
                {
                    int index = lineText.IndexOf("{");
                    while (index >= 0 && line.Start + index < m_View.Caret.Position.BufferPosition)
                    {
                        closeBraceCount--;
                        index = lineText.IndexOf("{", index + 1);
                    }
                }

                if (closeBraceCount > 0)
                {
                    line = snapshot.GetLineFromLineNumber(line.LineNumber - 1);
                }

            }

            int startLineNumber = line.GetText().Trim().Length != 1 || line.LineNumber == 0 ? line.LineNumber : line.LineNumber - 1;
            
            return line.LineNumber == 1 || caretLine.Snapshot != m_View.TextSnapshot ? line.Start : caretLine.Snapshot.GetLineFromLineNumber(startLineNumber).Start;
        }

        private SnapshotPoint GetBlockEndPoint(ITextSnapshotLine caretLine)
        {
            int openBraceCount = 1;

            var line = caretLine;
            string lineText;
            var snapshot = caretLine.Snapshot;
            
            while (line.LineNumber < snapshot.LineCount - 1 && openBraceCount > 0)
            {
                lineText = line.GetText();

                if (lineText.Contains("{"))
                {
                    int index = lineText.IndexOf("{");
                    while(index >= 0 && line.Start + index >= m_View.Caret.Position.BufferPosition)
                    {
                        openBraceCount++;
                        index = lineText.IndexOf("{", index + 1);
                    }
                }

                if (lineText.Contains("}"))
                {
                    int index = lineText.IndexOf("}");
                    while(index >= 0 && line.Start + index >= m_View.Caret.Position.BufferPosition)
                    {
                        openBraceCount--;
                        index = lineText.IndexOf("}", index + 1);
                    }
                }
                
                if(openBraceCount > 0)
                {
                    line = snapshot.GetLineFromLineNumber(line.LineNumber + 1);
                }

            }

            lineText = line.GetText();

            return lineText.Contains("}") ? new SnapshotPoint(snapshot, line.Start + lineText.IndexOf("}") + 1) : new SnapshotPoint(snapshot, snapshot.Length - 1);
        }

        private SnapshotSpan GetCaretBlockSpan()
        {
            ITextSnapshotLine caretLine = m_View.TextSnapshot.GetLineFromLineNumber(m_CaretPosition.BufferPosition.GetContainingLine().LineNumber);
            SnapshotPoint blockHeaderStartPoint = GetBlockHeaderStartPoint(caretLine);
            SnapshotPoint blockEndPoint = m_View.TextSnapshot.GetLineFromLineNumber(m_View.TextSnapshot.LineCount - 1).End;

            if (blockHeaderStartPoint != 0)
            {
                blockEndPoint = GetBlockEndPoint(caretLine);
            }
            
            return new SnapshotSpan(blockHeaderStartPoint, blockEndPoint);
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

            SnapshotSpan caretBlockSpan = GetCaretBlockSpan();

            result.Add(new SnapshotSpan(new SnapshotPoint(snapshot, 0), caretBlockSpan.Start));
            result.Add(new SnapshotSpan(caretBlockSpan.End, new SnapshotPoint(snapshot, snapshot.Length)));

            return new NormalizedSnapshotSpanCollection(result);
        }
            
        public IEnumerable<ITagSpan<ClassificationTag>> GetTags(NormalizedSnapshotSpanCollection spans)
        {
            if (spans == null || spans.Count == 0 || m_CurrentSpans == null ||m_CurrentSpans.Count == 0 || m_CurrentSpans[0].Snapshot != spans[0].Snapshot)
                yield break;
            
            foreach (SnapshotSpan span in NormalizedSnapshotSpanCollection.Overlap(spans, m_CurrentSpans))
            {
                yield return new TagSpan<ClassificationTag>(span, m_Tag);
            }
        }
    }
}
