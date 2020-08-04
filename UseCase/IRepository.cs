using FundaHomework.Entity;
using System.Collections.Generic;

namespace FundaHomework.UseCase
{
    public interface IRepository
    {
        public IList<Property> GetAllProperties();
        public IList<Property> GetAllPropertiesWithGarden();
    }
}