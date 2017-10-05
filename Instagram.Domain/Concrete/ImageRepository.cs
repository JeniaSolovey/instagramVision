using Instagram.Domain.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Instagram.Domain.Entities;

namespace Instagram.Domain.Concrete
{
    public class ImageRepository : IImageRepository
    {
        private DBContext context = new DBContext();

        public void DeleteImage(int id)
        {
            var targetImage = context.images.Find(id);
            if (targetImage != null)
            {
                context.images.Remove(targetImage);
                context.SaveChanges();
            }
        }

        public IEnumerable<Image> GetAllImages()
        {
            return context.images;
        }

        public Image GetImageById(int id)
        {
            return context.images.Find(id);
        }

        public void InsertOrUpdate(Image image)
        {
            if (image.ImageId == 0)
            {
                context.images.Add(image);
            }
            else
            {
                Image targetImage = context.images.Find(image.ImageId);

                if (targetImage != null)
                {
                    targetImage.Path = image.Path;
                    targetImage.Annotation = image.Annotation;
                }
            }
            context.SaveChanges();
        }
    }
}
