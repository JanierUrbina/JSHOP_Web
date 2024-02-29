namespace JSHOP_Web.Helper.ViewModels
{
    public enum Respuestas { Bueno, Malo, Error}
    public class UserAuthenticated
    {
        public string Role { get; set; }
        public bool IsAuthenticated { get; set; }
        public string Name { get; set; }
    }
   
}
