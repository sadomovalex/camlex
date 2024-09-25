using CamlexNET;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using ExpressionToCodeLib;

namespace CamlexOnlineX.Pages
{
    public class IndexModel : PageModel
    {
        public const int MAX_VALUE_LEN = 2000;

        private readonly ILogger<IndexModel> _logger;

        [Required]
        [StringLength(MAX_VALUE_LEN)]
        [BindProperty]
        public string Value { get; set; }

        public string Result { get; set; }
        public bool IsSuccess { get; set; }

        public IndexModel(ILogger<IndexModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
            this.Value =
                "<View>\n" +
                "  <Query>\n" +
                "    <Where>\n" +
                "      <Or>\n" +
                "        <Eq>\n" +
                "          <FieldRef Name=\"Title\" />\n" +
                "          <Value Type=\"Text\">Hello</Value>\n" +
                "        </Eq>\n" +
                "        <Eq>\n" +
                "          <FieldRef Name=\"Title\" />\n" +
                "          <Value Type=\"Text\">world</Value>\n" +
                "        </Eq>\n" +
                "      </Or>\n" +
                "    </Where>\n" +
                "  </Query>\n" +
                "</View>";
        }

        public void OnPost()
        {
            try
            {
                if (!this.ModelState.IsValid)
                {
                    return;
                }

                var expr = Camlex.QueryFromString(this.Value).ToExpression();
                this.Result = ExpressionToCode.ToCode(expr);
                this.IsSuccess = true;
            }
            catch (Exception x)
            {
                this.Result = x.Message;
                this.IsSuccess = false;
            }
        }
    }
}