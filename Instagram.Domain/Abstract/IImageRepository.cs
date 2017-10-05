using Instagram.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Instagram.Domain.Abstract
{
    public interface IImageRepository
    {
        Image GetImageById(int id);

        void InsertOrUpdate(Image image);
        void DeleteImage(int id);

        IEnumerable<Image> GetAllImages();
    }
}
