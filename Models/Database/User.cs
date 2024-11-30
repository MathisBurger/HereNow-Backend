namespace PresenceBackend.Models.Database;

public class User
{
    public int Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string Username { get; set; }
    [System.Text.Json.Serialization.JsonIgnore]
    public string Password { get; set; }
    
}