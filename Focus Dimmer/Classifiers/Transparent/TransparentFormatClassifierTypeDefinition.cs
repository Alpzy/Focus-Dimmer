using System.ComponentModel.Composition;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Focus_Dimmer.Classifiers
{
    internal static class TransparentFormatClassifierTypeDefinition
    {
        [Export(typeof(ClassificationTypeDefinition))]
        [Name("Alpzy/Transparent")]
        private static ClassificationTypeDefinition typeDefinition;
    }
}
