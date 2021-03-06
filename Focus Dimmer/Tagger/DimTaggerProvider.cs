﻿using Microsoft.VisualStudio.Text;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Text.Editor;
using Microsoft.VisualStudio.Text.Tagging;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace Focus_Dimmer.Tagger
{
    [Export(typeof(IViewTaggerProvider))]
    [ContentType("text")]
    [TagType(typeof(ClassificationTag))]
    public sealed class DimTaggerProvider : IViewTaggerProvider
    {
        [Import]
        public IClassificationTypeRegistryService registry { get; set; }
        
        [Import]
        public IViewTagAggregatorFactoryService tagAggregatorService { get; set; }

        public ITagger<T> CreateTagger<T>(ITextView textView, ITextBuffer buffer) where T : ITag
        {
            if (buffer != textView.TextBuffer)
                return null;

            return buffer.Properties.GetOrCreateSingletonProperty(() =>
                new DimTagger(textView, buffer, registry, tagAggregatorService) as ITagger<T>);
        }
    }
}
