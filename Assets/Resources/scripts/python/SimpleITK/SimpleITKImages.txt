import SimpleITK as sitk
from pathlib import Path 

class SliceType:
        Axial = "axial"
        Sagittal = "sagittal"
        Coronal = "coronal" 

class SimpleITKImages:
    def __init__(self):
        self.images = dict(axial=[], sagittal=[], coronal=[])

    @staticmethod
    def GetImagesFromSlice(image):
        sitkImages = SimpleITKImages()

        size = image.GetSize()
        for i in range(size[0] - 1):
            sitkImages.images[SliceType.Coronal].append(image[i, :, :])
        for i in range(size[1] - 1):
            sitkImages.images[SliceType.Sagittal].append(image[:, i, :])
        for i in range(size[2] - 1):
            sitkImages.images[SliceType.Axial].append(image[:, :, i])

        return sitkImages

    def FlipImages(self, slices = [SliceType.Axial, SliceType.Coronal, SliceType.Sagittal]):
        def __Flip(image):
            return sitk.Flip(image, [False, True, False])

        keys = self.images.keys()

        for k in keys:
            if k in slices:
                self.images[k] = map(__Flip, self.images[k])

        

    def SaveImages(self, output):
        keys = self.images.keys()

        for k in keys:
            dirPath = output + "/images/" + k
            Path(dirPath).mkdir(parents=True, exist_ok=True)

            id = 0
            for image in self.images[k]:
                writer = sitk.ImageFileWriter()
                filePath = dirPath + "/" + str(id) + ".png"
                writer.SetFileName(filePath)
                castedImage = sitk.Cast(sitk.RescaleIntensity(image), sitk.sitkUInt8)
                writer.Execute(castedImage)
                id += 1 