using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AvnConnect.Projects
{
    public class CategoryItem
    {
        public CategoryItem()
        {
            this.SubCategories = new ObservableCollection<CategoryItem>();
        }

        public Data.Category Category {get; set;}

        public ObservableCollection<CategoryItem> SubCategories { get; set; }

        public CategoryItem FindParentCategoryHasItemWithKey(string Key)
        {
            CategoryItem result = null;
            if (this.SubCategories.Where(sub => sub.Category.Key == Key).Count() > 0)
            {
                result = this;
            }
            else
            {
                foreach (var item in this.SubCategories)
                {
                    var r = item.FindParentCategoryHasItemWithKey(Key);
                    if (r != null)
                    {
                        result = r;
                        break;
                    }
                }
            }
            return result;
        }
    }
}
