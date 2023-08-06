using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.Routing;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;
using StoreApp.Models;

namespace StoreApp.Infrastructe.TagHelpers
{
	[HtmlTargetElement("div",Attributes= "page-model")]
	public class PageLinkTagHelper : TagHelper
	{
		private readonly IUrlHelperFactory _urlHelperFactory;
		[ViewContext]
		[HtmlAttributeNotBound]//ViewContext ile page eşleşmesin diye
		public ViewContext? ViewContext { get; set; } //görünüm ile ilgili bilgileri düzenleme için kullanacağız (input ....) gibi.
        public Pagination PageModel { get; set; }
        public String? PageAction { get; set; }
		public bool PageClassesEnabled { get; set; } = false; 
        public String PageClass { get; set; } = String.Empty;
        public String PageClassNormal { get; set; } = String.Empty;
        public String PageClassSelected { get; set; } = String.Empty;
		public PageLinkTagHelper(IUrlHelperFactory urlHelperFactory)
		{
			_urlHelperFactory = urlHelperFactory;
		}
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			if (ViewContext is not null && PageModel is not null)
			{
				//link üreteceğiz.
				IUrlHelper urlHelper = _urlHelperFactory.GetUrlHelper(ViewContext);
				TagBuilder result = new TagBuilder("div");
				for (int i = 1; i <= PageModel.TotalPages ; i++)
				{
					TagBuilder tag = new TagBuilder("a");
					tag.Attributes["href"] = urlHelper.Action(PageAction, new {PageNumber = i});
					//tag.Attributes.Add <=> tag.Attributes["href"]
					if(PageClassesEnabled)
					{
						//sayfa seçiminin yönetilmesi
						tag.AddCssClass(PageClass);
						tag.AddCssClass(i == PageModel.CurrentPage ? PageClassSelected : PageClassNormal);
						//sayfa numarası ile currentpage' i karşılaştırıyoruz.
					}
					tag.InnerHtml.Append(i.ToString());
					result.InnerHtml.AppendHtml(tag);
				}
				output.Content.AppendHtml(result.InnerHtml);
			}
		}
	}
}
