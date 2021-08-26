import sys
import SimpleITK as sitk
from SimpleITKImages import SimpleITKImages, SliceType

print(len(sys.argv))
if len(sys.argv) is 3:
    inputImageName = sys.argv[1]
    output = sys.argv[2]

    reader = sitk.ReadImage(inputImageName)
    # image = reader[256, :, :]

    # flippedImage = sitk.Flip(image, [False, True, False])

    # writer = sitk.ImageFileWriter()
    # filePath = output + "/image.png"
    # image = sitk.Cast(sitk.RescaleIntensity(image), sitk.sitkUInt8)

    # sitk.WriteImage(image, filePath)
    # writer.SetFileName(filePath)
    # writer.Execute(flippedImage)

    images = SimpleITKImages.GetImagesFromSlice(reader)
    images.FlipImages([SliceType.Coronal, SliceType.Sagittal])
    print("Start saving images to " + output)
    images.SaveImages(output)
    print("finished")
    
else:
    print("Not enough arguments!")

      