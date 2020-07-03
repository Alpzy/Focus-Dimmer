using System.ComponentModel.Composition;
using System.Windows.Media;
using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;

namespace Focus_Dimmer.Dim_Classifier
{
    [Export(typeof(EditorFormatDefinition))]
    [ClassificationType(ClassificationTypeNames = "DimClassifier")]
    [Name("DimClassifier")]
    [UserVisible(true)] 
    [Order(After = Priority.High)] 
    internal sealed class DimClassifierFormat : ClassificationFormatDefinition
    {
        public DimClassifierFormat()
        {
            this.DisplayName = "Dim Classifier"; // Human readable version of the name
            this.ForegroundColor = Colors.DimGray;
        }
    }
}
