using System;
using System.Collections.Generic;
using RohlikAPI.Model;

namespace RohlikAPI
{
    internal class ProductComparer : IEqualityComparer<Product>
    {
        public bool Equals(Product x, Product y)
        {
            return x.Name == y.Name && Math.Abs(x.Price - y.Price) < 0.01;
        }

        public int GetHashCode(Product product)
        {
            return product.Name.GetHashCode();
        }
    }
}