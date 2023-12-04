using ClassLibrary.Enums;

namespace ClassLibrary.Interfaces
{
    public interface IServiceResult
    {
        object Result { get; set; }
        ServiceStatus Status { get; set; }
    }
}