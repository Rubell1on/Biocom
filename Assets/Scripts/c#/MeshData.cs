public class MeshData
{
    public string name;
    public string inputFilePath;
    public string outputFilePath;

    public Threshold threshold;

    public MeshData(string name, string input, string output, Threshold threshold)
    {
        this.name = name;
        this.inputFilePath = input;
        this.outputFilePath = output;
        this.threshold = threshold;
    }
}
