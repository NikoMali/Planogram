using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Planograma.EmplUser.Domain.Helpers
{
    public interface IEnumModel<TModel, TModelIdType, TModelNameType>
    {
        TModelIdType Id { get; set; }
        TModelNameType Name { get; set; }
        bool IsActive { get; set; }
    }



    public static class EnumHelpers
    {
        public static IEnumerable<TModel> GetModelFromEnum<TModel, TEnum>() where TModel : IEnumModel<TModel, byte, TEnum>, new()
        {
            var enums = new List<TModel>();
            foreach (var item in Enum.GetValues(typeof(TEnum)).Cast<TEnum>())
            {
                enums.Add(new TModel()
                {
                    Id = Convert.ToByte(item),
                    Name = item,
                    IsActive = true
                });
            }

            return enums;
        }
    }
    
}
