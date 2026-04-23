using System.ComponentModel.DataAnnotations;

namespace LibraryBookBorrowingSystem.Dtos;

public class CreateMemberRequest
{
        [Required(AllowEmptyStrings = false)]
        public string FullName { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        public DateTime MembershipDate { get; set; }
}
