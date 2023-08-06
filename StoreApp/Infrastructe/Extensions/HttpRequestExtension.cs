namespace StoreApp.Infrastructe.Extensions
{
	public static class HttpRequestExtension
	{
		public static string PathAndQuery(this HttpRequest request)
		{
			return request.QueryString.HasValue //gelen requestin queryString valuesi var mı
				? $"{request.Path}{request.QueryString}" //eğer varsa mevcut queryString' leri birleştir
				: request.Path.ToString(); // eğer yoksa path bilgisini doğrudan döndürsün.
		}
		//geldiğimiz sayfaya geri dönmesi için yazıldı
	}
}
