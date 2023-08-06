using System.Text.Json;

namespace StoreApp.Infrastructe.Extensions
{
	public static class SessionExtension
	{
		//static bir classın tüm üyeleri static olur yani new' leme yapmadan ilgili sınıf adı üzerinden sınıf üyelerine erişebiliriz.
		public static void SetJson(this ISession session, string key, object value)
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}
		//serialize edip herhangi bir sınıf yapısını(objeyi) value' yi json stringi olarak hafızada tutabiliriz

		public static void SetJson<T>(this ISession session, string key, T value) //generic function
		{
			session.SetString(key, JsonSerializer.Serialize(value));
		}
		//iki metod da aynı işe yarar
		public static T? GetJson<T>(this ISession session, string key) //generic function. ifadeyi okuruz burada
		{
			var data = session.GetString(key); //anahtar değerle verdiğimiz ifadeyi getir.
			return data is null 
				? default(T) // saklı değilse default T yi alcaz. eğer sınıf bilgisiyse null olur(genelde).ama eğer farklı bir type varsa varsayılan onu dikkate alır
				: JsonSerializer.Deserialize<T>(data); // eğer anahtar değerle verdiğimiz session hafızada saklıysa o bilgiyi alacağız
		}
	}
}
