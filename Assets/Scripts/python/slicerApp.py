# import pydicom

path = "C:/tmp/images/ScalarVolume_9"

# ds = pydicom.dcmread(path+"/IMG0001.dcm")

# print()

import pydicom # for gettind information out of the IMA files
import numpy as np
from PIL import Image
"""
This function takes IOP of an image and returns its plane (Sagittal, Coronal, Transverse)
"""

def file_plane(IOP):
    IOP_round = [round(x) for x in IOP]
    plane = np.cross(IOP_round[0:3], IOP_round[3:6])
    plane = [abs(x) for x in plane]
    if plane[0] == 1:
        return "Sagittal"
    elif plane[1] == 1:
        return "Coronal"
    elif plane[2] == 1:
        return "Transverse"

a=pydicom.read_file(path + "/IMG0001.dcm")
IOP = a.ImageOrientationPatient
plane = file_plane(IOP)
shape = a.pixel_array.shape

image = Image.fromarray(a.pixel_array)
image.save("./image.jpeg")
print()