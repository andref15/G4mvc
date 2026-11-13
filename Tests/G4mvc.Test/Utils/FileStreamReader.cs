namespace G4mvc.Test.Utils;

public class FileStreamReader(FileInfo fileInfo) : StreamReader(fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
{
    public FileInfo FileInfo => fileInfo;
}
