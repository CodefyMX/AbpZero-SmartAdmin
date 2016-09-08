using Cinotam.FileManager.Contracts;
using Cinotam.FileManager.SharedTypes.Enums;
using CloudinaryDotNet;
using System;

namespace Cinotam.FileManager.Cloudinary.Transformations
{
    public static class TransFormationsConfig
    {
        public static Transformation GetTransformationConfiguration(IFileManagerServiceInput input)
        {
            var transformationInt = (int)input.Properties["TransformationType"];
            var parsedTrasformationType = (TransformationsTypes)transformationInt;
            switch (parsedTrasformationType)
            {
                case TransformationsTypes.ProfilePicture120X120:
                    return new Transformation().Width(120).Height(120).Gravity("face").Radius("max").Crop("crop");
                case TransformationsTypes.SimpleUpload:
                    return new Transformation();
                case TransformationsTypes.ImageWithSize:
                    return new Transformation().Width(input.Properties["Width"]).Height(input.Properties["Height"]);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}
