using Microsoft.AspNetCore.Razor.TagHelpers;

namespace StoreApp.Infrastructe.TagHelpers
{
	[HtmlTargetElement("table")] 
	//TagHelper' lar html attributeları(etiketleri tanımlayan ifadeler) kullandığı için yukarıdaki tanımı yapmalıyız. Burda table alanında çalışıyoruz.
	public class TableTagHelper : TagHelper
	{
		public override void Process(TagHelperContext context, TagHelperOutput output)
		{
			output.Attributes.SetAttribute("class", "table table-hover");
			//sayfalarda tanımladığımız table' lar artık otomatik olarak bootstrapin özelliğini alacak, <table class=""/> işini artık burada yapıyoruz
		}
	}
}
