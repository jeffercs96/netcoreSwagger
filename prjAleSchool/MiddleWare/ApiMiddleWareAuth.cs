using Microsoft.Extensions.Primitives;
namespace prjAleSchool.MiddleWare
{
    public class ApiMiddleWareAuth
    {
        // Constructor que recibe el siguiente middleware en la cadena
        private readonly RequestDelegate _next;
        
        // Lista de claves API permitidas
        private readonly List<string> allowedApiKeys = new List<string> { "APIKEY1", "APIKEY2" };

        public ApiMiddleWareAuth(RequestDelegate next)
        {
            _next = next;
        }
        
        // Método que se llama cuando llega una solicitud HTTP
        public async Task InvokeAsync(HttpContext context)
        {
            // Verificar si la solicitud contiene la cabecera "ApiKey"
            if (!context.Request.Headers.TryGetValue("ApiKey", out StringValues extractedApiKey))
            {
                // Si no contiene "ApiKey", responder con un error 400 (BadRequest)
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Falta clave api");
                return;
            }
      
            // Verificar si la clave API está en la lista de claves permitidas
            if (!allowedApiKeys.Contains(extractedApiKey))
            {
                // Si la clave no coincide, responder con un error 401 (Unauthorized)
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("La autorización ha fallado");
                return;
            }

            // Si la clave es válida, pasar la solicitud al siguiente middleware en la cadena
            await _next(context);
        }
    }
}
