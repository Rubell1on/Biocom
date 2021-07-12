import requests
import json
import SampleData
from Segmenter import Segmenter
import argparse

parser = argparse.ArgumentParser()
parser.add_argument("-p", "--port", required=True)
args = parser.parse_args()

response = requests.get(f"http://localhost:{args.port}/event/Slicer3d/connected")
if response.status_code == 200:
    print("Connected!")
    obj = json.loads(response.text)

    nodeName = obj["name"]
    sg = Segmenter(obj["inputFilePath"])
    sg.createSegmentationNode(nodeName)
    sg.setThreshold(nodeName, obj["threshold"])
    sg.exportToMesh(nodeName, exportFilePath=obj["outputFilePath"])
    createdMeshResponse = requests.get(f"http://localhost:{args.port}/event/Slicer3d/MeshCreated")

    if createdMeshResponse.status_code == 200:
        print("Created")
    else:
        print("Not created")
else:
    print("not connected")

quit()