using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Models.Common
{
    public class PageResponce<T>
    {
        [Required]
        public List<T> Items { get; set; }
        [Required]
        public PageInfoResponce PageInfo { get; set; }
    }

    public class PageInfoResponce
    {
        [Required]
        public int TotalPages { get; set; }
        [Required]
        public int TotalItems { get; set; }
        [Required]
        public int CurrentPage { get; set; }
        [Required]
        public int PageSize { get; set; }
    }
}
