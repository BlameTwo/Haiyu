using Haiyu.Models.Wrapper.Wiki;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Waves.Api.Models.GameWikiiClient;

namespace Haiyu.Helpers;

public static class WikiExtensions
{
    public static ObservableCollection<HotContentSideWrapper>? Format( this IEnumerable<HotContentSide> result)
    {
        ObservableCollection<HotContentSideWrapper> wrappers = new();
        if (result == null)
            return wrappers;
        foreach (var item in result)
        {
            var value = new HotContentSideWrapper()
            {
                Title = item.Title,
                ImageUrl = item.ContentUrl,
                StartTime = item.CountDown.DateRange[0],
                EndTime = item.CountDown.DateRange[1],
            };
            var spanResult = (DateTime.Parse(item.CountDown.DateRange[1]) - DateTime.Now);
            value.Cali();
            wrappers.Add(value);
        }
        return wrappers;
    }
}
