using System.ComponentModel.DataAnnotations;

namespace FaceSearch.Api.Data;

public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = "";
    public int SearchesUsed { get; set; } = 0;
    public bool IsPaid { get; set; } = false;
}
