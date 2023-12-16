
using ClassLibrary.Interfaces;
using ClassLibrary.Services;

namespace AddressBookConsole.Test;

public class FileService_Test
{
    /// <summary>
    /// Two integrationtests for up and downloading data from a jsonfile
    /// </summary>
    [Fact]
    public void SaveContentToFile_ShouldSaveContentToFile_ReturnTrue()
    {
        // Arrange
        //Creates an instance of FileService with filepath /AppData/Roaming as a param. 
        IFileService fileService = new FileService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Adressbok"));
        string testContent = "Test content";

        // Act
        bool result = fileService.SaveContentToFile(testContent); 

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void GetContentFromFile_ShouldGetContentFromFile_ReturnString()
    {
        // Arrange
        //Adding \r\n to testContent since SaveContactToFile uses WriteLine to write to the jsonfile
        IFileService fileService = new FileService(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Adressbok"));
        string testContent = "Test content\r\n";

        // Act
        string result = fileService.GetContentFromFile();

        // Assert
        Assert.Equal(testContent, result);
    }
}
