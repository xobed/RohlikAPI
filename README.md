Web scraping API for www.rohlik.cz

Quick Rohlik.cz product search using the API available at:  
https://xobed.github.io/RohlikAPI/

Available on Nuget:
https://www.nuget.org/packages/RohlikAPI/

Example without authentication:
```C#
var client = new RohlikApi("Ceska 1", "Brno");
IEnumerable<Product> allProducts = client.GetAllProducts();
var categories = client.GetCategories();
var productsForCategory = client.GetProducts(categories.First());
var searchResult = client.SearchProducts("Pivo");
```

Example with authentication:
```C#
var client = new AuthenticatedRohlikApi("user@email.cz", "secretpassword");
IEnumerable<Order> orderHistory = client.GetOrderHistory();
RohlikovacResult rohlikovacResult = client.RunRohlikovac();
```
