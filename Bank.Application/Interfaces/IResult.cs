namespace Bank.Application.Interfaces
{
    public interface IResult
    {
        bool IsSuccess { get; set; }
        string Error { get; set; }
        string Success { get; set; }
    }
}
