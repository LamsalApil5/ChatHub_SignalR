namespace HostelSoftware.Common
{
    public class TokenValidationMiddleware
    {
        private readonly RequestDelegate _next;

        public TokenValidationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var token = context.Session.GetString("SessionToken");
            var userId = context.Session.GetInt32("UserId");

            if (!string.IsNullOrEmpty(token) && userId.HasValue)
            {                
                context.Items["UserId"] = userId;
            }

            await _next(context);
        }
    }
}
