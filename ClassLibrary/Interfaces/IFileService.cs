
namespace ClassLibrary.Interfaces;

public interface IFileService
{
    bool SaveContentToFile(string content);
    string GetContentFromFile();
}
