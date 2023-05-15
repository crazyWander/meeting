using meeting;
using Newtonsoft.Json;

class SaveManager
{
    private string _defaultSaveFile = "saveMeeting.json";
    public async Task WriteDataToFileAsync(List<Meeting> resultData, string? saveFile)
    {
        try
        {
            saveFile = string.IsNullOrEmpty(saveFile) ? _defaultSaveFile : saveFile;
            saveFile = Path.GetFullPath(saveFile);

            string json = JsonConvert.SerializeObject(resultData, Formatting.Indented);
            File.WriteAllText(saveFile, json);
        }
        catch (Exception e)
        {
            Console.WriteLine("Произошла ошибка при выполнении программы");
            throw;
        }
    }
}