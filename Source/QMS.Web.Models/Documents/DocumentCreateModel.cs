﻿namespace QMS.Web.Models.Documents
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;
    using System.Web.Mvc;

    public class DocumentCreateModel
    {
        [Required]
        [MaxLength(50)]
        [AllowHtml]
        public string Title { get; set; }

        [MaxLength(500)]
        public string Description { get; set; }

        [Required]
        [MaxLength(20)]
        [AllowHtml]
        [RegularExpression("^F \\d+\\-\\d+$", ErrorMessage = "The document code must be something like: \"F 5-6\"")]
        public string Code { get; set; }

        [Required]
        [DisplayName("Procedure")]
        public int ProcedureId { get; set; }
    }
}
