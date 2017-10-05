using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Domain.Entities
{
    public class Image
    {
        public int ImageId { get; set; }
        public string Path { get; set; }
        public string Annotation { get; set; }
    }
}
