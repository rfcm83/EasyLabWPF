using System.Collections.Generic;
using System.ServiceModel;
using EasyLabWPF.Infrastructure;

namespace EasyLabWPF.Web
{
    [ServiceContract]
    public interface IDataService
    {
        [OperationContract]
        List<Product> GetProducts();

        [OperationContract]
        Product GetProductById(int id);

        [OperationContract]
        Product AddProduct(Product product);

        [OperationContract]
        void SaveProduct(Product product);

        [OperationContract]
        void DeleteProduct(Product product);
    }
}