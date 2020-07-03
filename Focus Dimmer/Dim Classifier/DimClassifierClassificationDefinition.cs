using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Focus_Dimmer
{
    internal static class DimClassifierClassificationDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("DimClassifier")]
        private static ClassificationTypeDefinition typeDefinition;
    }
}
