
using ClassLibrary.Enums;
using ClassLibrary.Interfaces;

namespace ClassLibrary.Models.Responses;

public class ServiceResult : IServiceResult
{
    public ServiceStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
