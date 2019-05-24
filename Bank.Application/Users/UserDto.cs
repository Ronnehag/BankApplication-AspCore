using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Bank.Application.Users
{
    public class UserViewModel
    {
        public List<UserDto> Users { get; set; }
    }

    public class UserDto
    {
        private string _currentClaim;

        [Required]
        public string Id { get; set; }
        public string Email { get; set; }
        public IList<ClaimDto> Claims { get; set; } = new List<ClaimDto>();
        public string SelectedClaim { get; set; }

        public string CurrentClaim
        {
            get => Claims.Count > 0 ? Claims[0].Type : string.Empty;
            set => _currentClaim = value;
        }
    }

    public class ClaimDto
    {
        public string Type { get; set; }
    }
}
