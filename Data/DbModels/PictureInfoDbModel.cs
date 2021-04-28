using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RESTStoreAPI.Data.DbModels
{
    public class PictureInfoDbModel
    {
        public int Id { get; set; }
        public string Path { get; set; }
        public string FileName { get; set; }
    }
}
