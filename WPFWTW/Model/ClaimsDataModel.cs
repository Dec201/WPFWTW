using ChoETL;
using System.ComponentModel.DataAnnotations;
using WPFWTW.Utility;

namespace WPFWTW.Model
{

    [ChoCSVFileHeader]
    //[ChoCSVRecordObject()]
    public class ClaimsDataModel
    {

        // Attempting to use ChoETL Validation ~ Not working 100% correctly

        [Required]
        [ChoCSVRecordField(1, FieldName = "Product", FieldValueTrimOption = ChoFieldValueTrimOption.Trim)]
        public string Product { get; set; }

        [Required]
        [ChoCSVRecordField(2, FieldName = "Origin Year", FieldValueTrimOption = ChoFieldValueTrimOption.Trim)]
        [Range(1900, 3000, ErrorMessage = ErrorCodes.Error_0201)]
        public int OriginYear { get; set; }

        [Required]
        [ChoCSVRecordField(3, FieldName = "Development Year", FieldValueTrimOption = ChoFieldValueTrimOption.Trim)]
        [Range(1900, 3000, ErrorMessage = ErrorCodes.Error_0201)]
        public int DevelopmentYear { get; set; }

        [Required]
        [ChoCSVRecordField(4, FieldName = "Incremental Value", FieldValueTrimOption = ChoFieldValueTrimOption.Trim)]
        public float IncrementalValue { get; set; }



    }
}
