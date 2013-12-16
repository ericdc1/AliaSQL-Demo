using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Demo.Website.ViewModels 
{
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public class TestTable : Models.Database.TestTable 
    {
        [Required]
        [MaxLength(50)]
        [DisplayName("Full Name")]
        public override string FullName { get; set; }

        [Required]
        [Range(minimum: 1, maximum: 9)]
        [DisplayName("Value One")]
        public override int value1 { get; set; }

        [Required]
        [Range(minimum: 1, maximum: 9)]
        [DisplayName("Value Two")]
        public override int value2 { get; set; }

        public int CalculatedTotal
        {
            get
            {
                return value1 + value2;
            }
        }
    }
}