using System.Collections.Generic;
using RohlikAPI.Model;

namespace RohlikAPI
{
    internal class ProductComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            return x.ProductUrl == y.ProductUrl;
        }

        public int GetHashCode(Product product)
        {
            return product.ProductUrl.GetHashCode();
        }
    }
}