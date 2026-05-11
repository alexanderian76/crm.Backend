using Microsoft.Extensions.DependencyInjection;

namespace crm.Backend.Attributes
{
    public class DIAttribute : Attribute
    {
        public string? Key { get; set; }
        public ServiceLifetime ServiceLifetime { get; set; }
    }

}