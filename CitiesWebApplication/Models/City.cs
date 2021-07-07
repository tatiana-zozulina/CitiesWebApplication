using System;
using System.ComponentModel.DataAnnotations;

namespace CitiesWebApplication.Models
{
    public class City
    {
        public int Id { get; set; }

        [RegularExpression(@"^[A-ZА-Я][a-zа-яA-ZА-Я'\s-]*$")]
        [MaxLength(length: 3000)]
        [Required]
        public string Name { get; set; }
        
        [Range(0,7000000000)]
        [Required]
        public long Population { get; set; }

        [Display(Name = "Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        [Required]
        public DateTime Date { get; set; }
    }
}
