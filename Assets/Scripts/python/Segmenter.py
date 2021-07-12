import os
import os.path
import slicer
import SampleData

class Segmenter:
    def __init__(self, path):
        self.path = path
        self.loadVolume(path)
        self.segments = []
        self.segmentationNodes = dict()

    def loadVolume(self, path):
        self.masterVolumeNode = slicer.util.loadVolume(path)
        return self

    def createSegmentationNode(self, name):
        segmentationNode = slicer.mrmlScene.AddNewNodeByClass("vtkMRMLSegmentationNode")
        segmentationNode.CreateDefaultDisplayNodes() # only needed for display
        segmentationNode.SetReferenceImageGeometryParameterFromVolumeNode(self.masterVolumeNode)
        id = segmentationNode.GetSegmentation().AddEmptySegment(name)
        self.segmentationNodes[name] = segmentationNode

    def setThreshold(self, segmentationNodeName, params = dict(min="1", max="1")):
        segmentEditorWidget = slicer.qMRMLSegmentEditorWidget()
        segmentEditorWidget.setMRMLScene(slicer.mrmlScene)
        segmentEditorNode = slicer.mrmlScene.AddNewNodeByClass("vtkMRMLSegmentEditorNode")
        segmentEditorWidget.setMRMLSegmentEditorNode(segmentEditorNode)
        print(self.segmentationNodes[segmentationNodeName])
        segmentEditorWidget.setSegmentationNode(self.segmentationNodes[segmentationNodeName])
        segmentEditorWidget.setMasterVolumeNode(self.masterVolumeNode)

        # Thresholding
        segmentEditorWidget.setActiveEffectByName("Threshold")
        effect = segmentEditorWidget.activeEffect()
        effect.setParameter("MinimumThreshold", params["min"])
        effect.setParameter("MaximumThreshold", params["max"])
        effect.self().onApply()

    def exportToMesh(self, segmentationNodeName, exportFilePath = "c:/tmp", format = "OBJ"):
        node = self.segmentationNodes[segmentationNodeName]
        node.CreateClosedSurfaceRepresentation()
        if os.path.exists(exportFilePath) is not True:
            os.makedirs(exportFilePath)
        slicer.vtkSlicerSegmentationsModuleLogic.ExportSegmentsClosedSurfaceRepresentationToFiles(exportFilePath, node, None, format)