namespace Bank.WebUI.Models
{
    public class Error
    {
        public string ErrorMsg { get; set; }

        public Error(string name, int id)
        {
            ErrorMsg = $"No {name} found with ID {id}";
        }
    }
}
