namespace Company.Repository.Auth.Model
{
    public class LoginResponseModel
    {
        public string AccessToken { get; set; }     
        public int ExpiresIn { get; set; }
    }
}
