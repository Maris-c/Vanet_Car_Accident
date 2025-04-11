namespace AccidentMonitor.Application.Common.Results;

public class ServiceResult(int isSucces, string reason)
{
    public int ResultCode { get; set; } = isSucces;
    public string Reason { get; set; } = reason;
}

