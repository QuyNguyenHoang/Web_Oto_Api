using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.CategoriesDTOs
{
    public class CategoriesDto
    {
        public string? CategoryName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string? Description { get; set; }
    }
}
