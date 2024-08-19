namespace Service.Services.Interfaces
{
    public interface IFileService
    {
        string ReadFile(string path, string readTemplate);
    }
}
