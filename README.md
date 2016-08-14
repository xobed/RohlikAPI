Web scraping API for www.rohlik.cz

Available on Nuget:
https://www.nuget.org/packages/RohlikAPI/

###Supported actions  
####Without user authentication
GetCenoveTrhaky  
GetLastMinute  
GetProducts(category)  
SearchProducts(searchString)  
####Requiring authentication
GetOrderHistory  
[RunRohlikovac](https://www.rohlik.cz/stranka/rohlikovac)

###Example
```cs
var api = new RohlikApi(City.Brno);
// Category as visible in URL
// E.g. https://www.rohlik.cz/c300101000-pekarna-a-cukrarna
var productList = api.GetProducts("c300101000-pekarna-a-cukrarna").ToList();
```
```cs
// Auth api performs login on instantiation
var rohlikApi = new AuthenticatedRohlikApi(user@mail.com, SecretPassword);
var allRohlikOrders = rohlikApi.GetOrderHistory();
```
