using Microsoft.VisualStudio.Text.Classification;
using Microsoft.VisualStudio.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Focus_Dimmer.Classifier
{
    [Export(typeof(EditorFormatDefinition))]
    [Name("Alpzy/DimFormatDefinition")]
    [ClassificationType(ClassificationTypeNames = "Alpzy/DimFormatDefinition")]
    [UserVisible(true)]
    [Order(After = Priority.High)]
    class DimFormatDefinition : ClassificationFormatDefinition
    {
        public DimFormatDefinition()
        {
            this.DisplayName = "Dim Classifier"; // Human readable version of the name
            this.ForegroundColor = Colors.DimGray;
        }
    }
}
