using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Domain.Entities
{
    public class UserInfo
    {
        [Key]
        [Required]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserInfoId { get; set; }

        public string? Id { get; set; }

        [StringLength(100)]
        public string? FullName { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Address { get; set; }

        public DateTime? BirthDate { get; set; }

        [StringLength(255)]
        public string? Picture { get; set; }

        public bool? Sex { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [StringLength(500)]
        public string? Hobbies { get; set; }

        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [StringLength(255)]
        public string? Facebook { get; set; }

        [StringLength(255)]
        public string? Website { get; set; }

        [StringLength(100)]
        public string? Country { get; set; }

        [StringLength(100)]
        public string? Province { get; set; }

        [StringLength(100)]
        public string? City { get; set; }

        [StringLength(100)]
        public string? District { get; set; }

        [StringLength(100)]
        public string? Ward { get; set; }

        [ForeignKey("Id")]
        public virtual User? User { get; set; }
    }
}
