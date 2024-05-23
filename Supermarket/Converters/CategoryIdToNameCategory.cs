using Supermarket.Models.BusinessLogicLayer;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Supermarket.Converters
{
    internal class CategoryIdToNameCategory : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values[0] is int categoryId && values[1] is CategoriesBLL categoriesBLL)
            {
                return categoriesBLL.GetCategoryNameFromId(categoryId);
            }
            return "Unknown Producer";
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
