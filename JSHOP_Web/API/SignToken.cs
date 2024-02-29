using System.Diagnostics.Contracts;
using System.Net.Http;

namespace JSHOP_Web.API
{
    public class SignToken
    {
        private HttpClient _cliente ;
        public record validateLogin
        {
            public string username { get; set; }
            public string password { get; set; }
        };

        public HttpClient cliente
        {
            get
            {
                if (_cliente == null)
                {
                    _cliente = new HttpClient();

                }
                return _cliente;

            }
            set { _cliente = value; }
        }
        public SignToken(string url)
        {

            cliente.DefaultRequestHeaders.Accept.Clear();
            cliente.BaseAddress = new Uri(url);


            cliente.DefaultRequestHeaders.Accept.Add(new
            System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));
           

        }

        public async Task IniciarSesion(string usuario, string contrasena)
        {// Crear un objeto validateLogin con los datos
            var validateLoginData = new validateLogin
            {
                username = usuario,
                password = contrasena
            };

            // Realizar la solicitud POST con el objeto directamente
            var t = Task.Run(()=> _cliente.PostAsJsonAsync("token", validateLoginData));

            string resultContent = t.Result.Content.ReadAsStringAsync().Result;
            Token.Instance.elemento = Newtonsoft.Json.JsonConvert.DeserializeObject<Token>(resultContent);
        }
        public Token getToken { get { return Token.Instance.elemento; } }
        public class Token
        {
            private static volatile Token instance;
            private static object syncRoot = new Object();

            private Token() { }

            public static Token Instance
            {
                get
                {
                    if (instance == null)
                    {
                        lock (syncRoot)
                        {
                            if (instance == null)
                                instance = new Token();
                        }
                    }

                    return instance;
                }
            }

            public Token elemento
            {
                get { return instance; }
                set { instance = value; }
            }

            public string AccessToken { get; set; }           
            public string msj { get; set; }           
        }
    }
}
