using Google.Cloud.Vision.V1;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;

namespace Instagram.Site.Helpers
{
    public static class Annotator
    {
        public static AnnotateImageResponse AnotateImage(HttpPostedFileBase file)
        {
            ImageAnnotatorClient imageAnnotatorClient = ImageAnnotatorClient.Create();
            var request = new AnnotateImageRequest()
            {
                Image = Google.Cloud.Vision.V1.Image.FromStream(file.InputStream),
                Features =
                {
                    new Feature() { Type = Feature.Types.Type.FaceDetection},
                    new Feature() { Type = Feature.Types.Type.LabelDetection, MaxResults = 4}
                }
            };

            try
            {
                AnnotateImageResponse response = imageAnnotatorClient.Annotate(request);
                return response;
            }
            catch (Exception ex)
            {
                throw new Exception();
            }


        }

        public static Bitmap DrawFaces(HttpPostedFileBase file, IEnumerable<FaceAnnotation> faceAnnotations)
        {
            Bitmap bitmap = new Bitmap(file.InputStream);

            if (faceAnnotations.Count() < 1) { return bitmap; }

            Pen greenPen = new Pen(Color.LimeGreen, 6);

            var rectangles = GenerateRectangles(faceAnnotations);

            using (var graphics = Graphics.FromImage(bitmap))
            {
                graphics.DrawRectangles(greenPen, rectangles.ToArray());
            }

            return bitmap;
        }

        private static List<Rectangle> GenerateRectangles(IEnumerable<FaceAnnotation> targets)
        {
            List<Rectangle> rectangles = new List<Rectangle>();
            foreach (var target in targets)
            {
                var Coordinates = target.BoundingPoly.Vertices;
                int x = Coordinates[0].X;
                int y = Coordinates[0].Y;
                int width = Coordinates[1].X - x;
                int height = Coordinates[2].Y - y;
                rectangles.Add(new Rectangle(x, y, width, height));
            }
            return rectangles;
        }
    }
}