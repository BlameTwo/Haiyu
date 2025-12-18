using Haiyu.Models.Wrapper.Wiki;
using System;
using System.Collections.Generic;
using System.Text;

namespace Haiyu.Controls.Selector
{
    public partial class PunishBannerSelector : DataTemplateSelector
    {
        public DataTemplate? SingleImageTemplate { get; set; } // 单图模板
        public DataTemplate? TripleImageTemplate { get; set; } // 三图模板

        protected override DataTemplate? SelectTemplateCore(object item, DependencyObject container)
        {
            if (item is PunishBannerWrapperItem bannerItem)
            {

                // 根据图片数量返回对应模板
                return bannerItem.ImageUrlCount switch
                {

                    1 => SingleImageTemplate,
                    3 => TripleImageTemplate,

                    _ => SingleImageTemplate
                };
            }
            // 如果不是预期类型，回退到基类默认行为
            return base.SelectTemplateCore(item, container);
        }
    }
}
