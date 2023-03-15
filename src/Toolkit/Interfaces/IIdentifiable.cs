using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Mttechne.Toolkit.Interfaces;

public interface IIdentifiable
{
    int Id { get; set; }
}