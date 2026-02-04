using ExceptionManagement;

namespace Domain;

public partial class User: IValidation
{
    public User()
    {
    }

    public User(Guid userId, string userEmail, string userName, string password,string salt)
    {
        this.UserId = userId;
        this.UserEmail = userEmail;
        this.UserName = userName;
        this.PasswordHash = password;
        this.PasswordSalt = salt;
    }
    public Guid UserId { get; set; }
    public string UserEmail { get; set; }
    public string UserName { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string PasswordSalt { get; set; } = null!;

    public bool? IsDeleted { get; set; }

    // Validate the user object
    public string Validate(string operationExceptionCode)
    {
        if (String.IsNullOrEmpty(this.UserName))
            operationExceptionCode = "OMS-USERNAME-ERROR";
        else if (this.UserName.Length > 50)
            operationExceptionCode = "OMS-USERNAME-ERROR";
        else if (String.IsNullOrEmpty(this.PasswordHash))
            operationExceptionCode = "OMS-PASSWORD-ERROR";
        else if (this.PasswordHash.Length > 50)
            operationExceptionCode = "OMS-PASSWORD-ERROR";

        return operationExceptionCode;
    }

}
