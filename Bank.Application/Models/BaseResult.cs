using Bank.Application.Interfaces;

namespace Bank.Application.Models
{
    public class BaseResult : IResult
    {
        public bool IsSuccess { get; set; }
        public string Success { get; set; }
        public string Error { get; set; }
    }
}
