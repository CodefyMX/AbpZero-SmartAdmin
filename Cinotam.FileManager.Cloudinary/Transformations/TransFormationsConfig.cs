using Cinotam.FileManager.Cloudinary.Cloudinary.Inputs;
using Cinotam.FileManager.SharedTypes.Enums;
using CloudinaryDotNet;
using System;

namespace Cinotam.FileManager.Cloudinary.Transformations
{
    public static class TransFormationsConfig
    {
        public static Transformation GetTransformationConfiguration(SaveImageInput input)
        {
            switch (input.TransformationsType)
            {
                case TransformationsTypes.ProfilePicture120X120:
                    return new Transformation().Width(120).Height(120).Gravity("face").Radius("max").Crop("crop");
                case TransformationsTypes.SimpleUpload:
                    return new Transformation();
                case TransformationsTypes.ImageWithSize:
                    return new Transformation().Width(input.Width).Height(input.Height);
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }


    }
}
