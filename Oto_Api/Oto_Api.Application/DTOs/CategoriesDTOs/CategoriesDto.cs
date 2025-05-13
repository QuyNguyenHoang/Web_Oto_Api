using Oto_Api.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Oto_Api.Application.DTOs.CategoriesDTOs
{
    public class CategoriesDto
    {
       
        public string? CategoryName {  get; set; }
        public string? Description { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

       
        
    }
}

