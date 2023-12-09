
using ClassLibrary.Enums;
using ClassLibrary.Interfaces;

namespace ClassLibrary.Models.Responses;
/// <summary>
/// Class that contains an object property and an enum (ServiceStatus) propery. Returned by most ContactService methods
/// </summary>
public class ServiceResult : IServiceResult
{
    public ServiceStatus Status { get; set; }
    public object Result { get; set; } = null!;
}
