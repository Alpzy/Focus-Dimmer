using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;

namespace Focus_Dimmer.Classifiers
{
    [Export(typeof(EditorFormatDefinition))]
    [Name("Alpzy/Transparent")]
    [ClassificationType(ClassificationTypeNames = "Alpzy/Transparent")]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    class TransparentFormatDefinition : ClassificationFormatDefinition
    {
        public TransparentFormatDefinition()
        {
            this.DisplayName = "Transparent Classifier"; // Human readable version of the name
            this.ForegroundOpacity = 0.25;
        }
    }
}
