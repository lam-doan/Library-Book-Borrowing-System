using System.ComponentModel.DataAnnotations;

namespace LibraryBookBorrowingSystem.Dtos;

public class CreateBookRequest
{
        [Required(AllowEmptyStrings = false)]
        public string Title { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string Author { get; set; } = string.Empty;

        [Required(AllowEmptyStrings = false)]
        public string ISBN { get; set; } = string.Empty;

        [Range(1, int.MaxValue, ErrorMessage = "TotalCopies must be greater than 0.")]
        public int TotalCopies { get; set; }
}
