using Fluid;
using Microsoft.Extensions.Options;

namespace LocalServicesMarketplace.Identity.API.Infrastructure.Email
{
    public interface ITemplateRenderer
    {
        Task<string> RenderAsync<T>(string templateName, T model);
    }

    public class TemplateRenderer : ITemplateRenderer
    {
        private readonly IFluidParser _parser;
        private readonly TemplateSettings _settings;
        private readonly ILogger<TemplateRenderer> _logger;
        private readonly Dictionary<string, string> _templateCache;

        public TemplateRenderer(
            IOptions<TemplateSettings> settings,
            ILogger<TemplateRenderer> logger)
        {
            _settings = settings.Value;
            _logger = logger;
            _parser = new FluidParser();
            _templateCache = new Dictionary<string, string>();

            // Load templates at startup
            LoadTemplates();
        }

        public async Task<string> RenderAsync<T>(string templateName, T model)
        {
            try
            {
                if (!_templateCache.TryGetValue(templateName, out var template))
                {
                    throw new KeyNotFoundException($"Template '{templateName}' not found");
                }

                if (!_parser.TryParse(template, out var fluidTemplate, out var error))
                {
                    throw new Exception($"Failed to parse template: {error}");
                }

                var context = new TemplateContext(model);
                return await fluidTemplate.RenderAsync(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error rendering template {TemplateName}", templateName);
                throw;
            }
        }

        private void LoadTemplates()
        {
            var templatePath = Path.Combine(AppContext.BaseDirectory, _settings.TemplatesPath);
            
            foreach (var file in Directory.GetFiles(templatePath, "*.liquid"))
            {
                var templateName = Path.GetFileNameWithoutExtension(file);
                var templateContent = File.ReadAllText(file);
                _templateCache[templateName] = templateContent;
            }
        }
    }

    public class TemplateSettings
    {
        public string TemplatesPath { get; set; } = "Templates/Email";
    }
}
