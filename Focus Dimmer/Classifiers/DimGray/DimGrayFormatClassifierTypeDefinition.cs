using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Focus_Dimmer.Classifiers
{
    internal static class DimGrayFormatClassifierTypeDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Alpzy/DimGray")]
        private static ClassificationTypeDefinition typeDefinition;
    }
}
