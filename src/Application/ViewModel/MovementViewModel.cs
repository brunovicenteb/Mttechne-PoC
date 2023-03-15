using System;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Mttechne.Application.ViewModel;

public class MovementViewModel
{
    public int Id { get; set; }

    [DisplayName("Value")]
    [Required(ErrorMessage = "The Value is Required")]
    [Range(0.01, int.MaxValue, ErrorMessage = "Value Must Bigger Than Zero")]
    public decimal Value { get; set; }

    [Required(ErrorMessage = "The Description is Required")]
    [MinLength(5, ErrorMessage = "The Description must be at least 5 characters")]
    public string Description { get; set; }

    [Required(ErrorMessage = "The Movement Type is Required")]
    public int MovementTypeId { get; set; }

    public string MovementType { get; set; }

    public DateTime CreatedAt { get; set; }
}
