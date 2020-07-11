using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System.ComponentModel.Composition;
using System.Windows.Media;

namespace Focus_Dimmer.Classifiers
{
    [Export(typeof(EditorFormatDefinition))]
    [Name("Alpzy/DimGray")]
    [ClassificationType(ClassificationTypeNames = "Alpzy/DimGray")]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    class DimGrayFormatDefinition : ClassificationFormatDefinition
    {
        public DimGrayFormatDefinition()
        {
            this.DisplayName = "DimGray Classifier"; // Human readable version of the name
            this.ForegroundColor = Colors.DimGray;
        }
    }
}
