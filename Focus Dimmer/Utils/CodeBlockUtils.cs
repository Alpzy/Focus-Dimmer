using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;

namespace Focus_Dimmer.Utils
{
    public static class CodeBlockUtils
    {
        public static SnapshotSpan GetCaretBlockSpan(ITextView view, CaretPosition caretPosition, IViewTagAggregatorFactoryService tagAggregatorService)
        {
            ITextSnapshotLine caretLine = view.TextSnapshot.GetLineFromLineNumber(caretPosition.BufferPosition.GetContainingLine().LineNumber);
            SnapshotPoint blockHeaderStartPoint = GetBlockHeaderStartPoint(view, caretLine, tagAggregatorService);
            SnapshotPoint blockEndPoint = view.TextSnapshot.GetLineFromLineNumber(view.TextSnapshot.LineCount - 1).End;

            if (blockHeaderStartPoint != 0)
            {
                blockEndPoint = GetBlockEndPoint(view, caretLine, tagAggregatorService);
            }

            return new SnapshotSpan(blockHeaderStartPoint, blockEndPoint);
        }

        private static SnapshotPoint GetBlockHeaderStartPoint(ITextView view, ITextSnapshotLine caretLine, IViewTagAggregatorFactoryService tagAggregatorService)
        {
            var snapshot = caretLine.Snapshot;
            var aggregator = tagAggregatorService.CreateTagAggregator<IClassificationTag>(view);

            var line = caretLine;

            int closeBraceCount = 1; //Using this like a stack.
            
            while (line.LineNumber - 1 > 0 && closeBraceCount > 0)
            {
                string lineText = line.GetText();

                if (lineText.Contains("}"))
                {
                    int index = lineText.IndexOf("}");
                                        
                    while (index >= 0 && line.Start + index < view.Caret.Position.BufferPosition)
                    {
                        if (!TagUtils.isStringOrComment(line, index, aggregator))
                        {
                            closeBraceCount++;
                        }
                        index = lineText.IndexOf("}", index + 1);
                    }
                }

                if (lineText.Contains("{"))
                {
                    int index = lineText.IndexOf("{");
                    while (index >= 0 && line.Start + index < view.Caret.Position.BufferPosition)
                    {
                        if (!TagUtils.isStringOrComment(line, index, aggregator))
                        {
                            closeBraceCount--;
                        }
                        index = lineText.IndexOf("{", index + 1);
                    }
                }

                if (closeBraceCount > 0)
                {
                    line = snapshot.GetLineFromLineNumber(line.LineNumber - 1);
                }
            }

            int startLineNumber = (line.GetText().Trim().Length != 1 && TagUtils.ContainsTag(line, "keyword", aggregator)) || line.LineNumber == 0 
                ? line.LineNumber 
                : line.LineNumber - 1;

            return line.LineNumber == 1 || caretLine.Snapshot != view.TextSnapshot 
                ? line.Start 
                : caretLine.Snapshot.GetLineFromLineNumber(startLineNumber).Start;
        }

        private static SnapshotPoint GetBlockEndPoint(ITextView view, ITextSnapshotLine caretLine, IViewTagAggregatorFactoryService tagAggregatorService)
        {
            var snapshot = caretLine.Snapshot;
            var aggregator = tagAggregatorService.CreateTagAggregator<IClassificationTag>(view);

            var line = caretLine;
            string lineText;

            int openBraceCount = 1; //Using this like a stack.

            while (line.LineNumber < snapshot.LineCount - 1 && openBraceCount > 0)
            {
                lineText = line.GetText();

                if (lineText.Contains("{"))
                {
                    int index = lineText.IndexOf("{");
                    while (index >= 0 && line.Start + index >= view.Caret.Position.BufferPosition)
                    {
                        if (!TagUtils.isStringOrComment(line, index, aggregator))
                        {
                            openBraceCount++;
                        }
                        index = lineText.IndexOf("{", index + 1);
                    }
                }

                if (lineText.Contains("}"))
                {
                    int index = lineText.IndexOf("}");
                    while (index >= 0 && line.Start + index >= view.Caret.Position.BufferPosition)
                    {
                        if (!TagUtils.isStringOrComment(line, index, aggregator))
                        {
                            openBraceCount--;
                        }
                        index = lineText.IndexOf("}", index + 1);
                    }
                }

                if (openBraceCount > 0)
                {
                    line = snapshot.GetLineFromLineNumber(line.LineNumber + 1);
                }

            }

            lineText = line.GetText();

            return lineText.Contains("}") 
                ? new SnapshotPoint(snapshot, line.Start + lineText.IndexOf("}") + 1) 
                : new SnapshotPoint(snapshot, snapshot.Length - 1);
        }
    }
}
