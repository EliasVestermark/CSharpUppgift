
using ClassLibrary.Interfaces;
using System.Diagnostics;

namespace ClassLibrary.Services;

/// <summary>
/// Class for handling up and downloading to a jsonfile
/// </summary>
/// <param name="filePath">Filepath provided by DI</param>
public class FileService(string filePath) : IFileService
{
    private readonly string _filePath = filePath;

    /// <summary>
    /// Writes data to jsonfile, returns true or false depending if it succeded or not
    /// </summary>
    /// <param name="content">data to be written to the jsonfile</param>
    /// <returns></returns>
    public bool SaveContentToFile(string content)
    {
        try
        {
            using (var sw = new StreamWriter(_filePath)) 
            {
                sw.WriteLine(content);
            }
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine(ex.Message);
            return false;
        }
    }

    /// <summary>
    /// Gets data from jsonfile.
    /// </summary>
    /// <returns>return the data from the jsonfile as a string</returns>
    public string GetContentFromFile()
    {
        try
        {
            if(File.Exists(_filePath))
            {
                using (var sr =  new StreamReader(_filePath))
                {
                    return sr.ReadToEnd();
                }
            }
        }
        catch (Exception ex) { Debug.WriteLine(ex.Message); }
        return null!;
    }
}
