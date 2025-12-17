using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.Json;
using Waves.Api.Models.GameWikiiClient;
using WinUIEx.Messaging;

namespace Haiyu.Models.Wrapper.Wiki
{
    public partial class PunishEveryWeekContent : ObservableObject
    {
        [ObservableProperty]
        public partial string NormanRemainingTime { get; set; }
        [ObservableProperty]
        public partial double NormanMaxProgress { get; set; }

        [ObservableProperty]
        public partial double NormanProgress { get; set; }

        [ObservableProperty]
        public partial string NormanImageUrl { get; set; }

        [ObservableProperty]
        public partial string WarZoneRemainingTime { get; set; }

        [ObservableProperty]
        public partial double WarZoneProgress { get; set; }
        [ObservableProperty]
        public partial double WarZoneMaxProgress { get; set; }

        [ObservableProperty]
        public partial string WarZoneImageUrl { get; set; }

        [ObservableProperty]
        public partial string WarZoneBuffName { get; set; }

        [ObservableProperty]
        public partial string WarZoneBuffDescription { get; set; }

        [ObservableProperty]
        public partial ObservableCollection<DisputeWarZoneItem> DisputeWarZoneItems { get; set; } = new();

        [ObservableProperty]
        public partial ObservableCollection<PunishCageItem> PunishCageItems { get; set; } = new();

        [ObservableProperty]
        public partial string PunishRemainingTime { get; set; }

        [ObservableProperty]
        public partial double PunishCageProgress { get; set; } = 50;
        [ObservableProperty]
        public partial double PunishCageMaxProgress { get; set; } = 100;


        private Dictionary<string, PunishCageCacheItem> _punishCageCache = new();

        public void InitWeekContent(List<SideModule> modules, List<MainModule> mainModules = null)
        {
            SideModule? norman = null;
            SideModule? warzone = null;
            SideModule? cage = null;
            if (modules != null)
            {
                foreach (var module in modules)
                {
                    if (module.Title == "诺曼复兴战") norman = module;
                    if (module.Title == "历战映射") warzone = module;
                    if (module.Title == "幻痛囚笼") cage = module;
                }
            }

            if (norman != null) SetNormanData(norman);
            if (warzone != null) SetWarZoneData(warzone);
            if (cage != null) SetPunishCageData(cage);

            if (mainModules != null)
            {
                foreach (var module in mainModules)
                {
                    if (module.Title == "纷争战区")
                    {
                        SetDisputeDataFromMain(module);
                        break;
                    }
                }
            }
        }

        public void SetDisputeDataFromMain(MainModule module)
        {
            if (module == null) return;
            if (module.Content is JsonElement element)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        TypeInfoResolver = WikiContext.Default
                    };

                    if (element.ValueKind == JsonValueKind.Array)
                    {
                        try
                        {
                            var items = element.Deserialize<List<DisputeJsonItem>>(options);
                            if (items != null && items.Count > 0)
                            {
                                DisputeWarZoneItems.Clear();
                                foreach (var item in items)
                                {
                                    var disputeItem = new DisputeWarZoneItem();
                                    disputeItem.Title = item.Title;
                                    disputeItem.ImageUrl = item.ContentUrl;

                                    if (item.CountDown != null)
                                    {
                                        List<string> range = null;
                                        if (item.CountDown.DateRange != null && item.CountDown.DateRange.Count >= 2)
                                        {
                                            range = item.CountDown.DateRange;
                                        }
                                        else if (item.CountDown.Repeat != null && item.CountDown.Repeat.DataRanges != null)
                                        {
                                            var dr = item.CountDown.Repeat.DataRanges.FirstOrDefault();
                                            if (dr != null) range = dr.DataRange;
                                        }

                                        if (range != null && range.Count >= 2)
                                        {
                                            if (DateTime.TryParse(range[0], out var start) && DateTime.TryParse(range[1], out var end))
                                            {
                                                disputeItem.TimeRange = $"{start:MM.dd HH:mm}-{end:MM.dd HH:mm}";
                                                var now = DateTime.Now;
                                                TimeSpan _endCountdownTimeSpan = end - now;
                                                TimeSpan _totalDurationTimeSpan = end - start;
                                                TimeSpan _overCountdownTimeSpace = now - start;
                                                disputeItem.MaxProgress = _totalDurationTimeSpan.TotalSeconds;
                                                double elapsed = _endCountdownTimeSpan.TotalSeconds;


                                                if (elapsed <= 0)
                                                {
                                                    disputeItem.CurrentProgress = disputeItem.MaxProgress;
                                                    disputeItem.RemainingTime = "已结束";
                                                    disputeItem.ColorVal = "Red";
                                                    return;
                                                }

                                                disputeItem.CurrentProgress = _overCountdownTimeSpace.TotalSeconds;
                                                disputeItem.RemainingTime = $"剩余{_endCountdownTimeSpan.Days}天" +
                                                            $"{_endCountdownTimeSpan.Hours}小时" +
                                                            $"{_endCountdownTimeSpan.Minutes}分";
                                                disputeItem.ColorVal = "#66CCFF";


                                            }
                                        }
                                    }
                                    DisputeWarZoneItems.Add(disputeItem);
                                }
                                return;
                            }
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Error parsing DisputeJsonItem: {ex.Message}");
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error parsing Dispute data: {ex.Message}");
                }
            }
        }

        public void SetWarZoneData(SideModule module)
        {
            if (module == null) return;

            if (module.Content is JsonElement element)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        TypeInfoResolver = WikiContext.Default
                    };
                    WeekContentJsonContent? content = null;

                    if (element.ValueKind == JsonValueKind.Array)
                    {
                        var list = element.Deserialize<List<WeekContentJsonContent>>(options);
                        content = list?.FirstOrDefault();
                    }
                    else
                    {
                        content = element.Deserialize<WeekContentJsonContent>(options);
                    }

                    if (content != null && content.Tabs != null && content.Tabs.Count > 0)
                    {
                        var tab = content.Tabs.FirstOrDefault(t => (bool)t.Active) ?? content.Tabs.First();
                        SetWarZoneDataInternal(tab);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error parsing WarZone data: {ex.Message}");
                }
            }
        }

        private (double currVal, double maxVal, string msg) SetDataInternal(WeekContentTab item, double currVal, double maxVal, string msg, int days = 14)
        {

            if (item.CountDown != null && item.CountDown.Repeat != null && item.CountDown.Repeat.DataRanges != null)
            {
                var dataRange = item.CountDown.Repeat.DataRanges.FirstOrDefault();
                if (dataRange != null && dataRange.DataRange != null && dataRange.DataRange.Count >= 2)
                {

                    if (DateTime.TryParse(dataRange.DataRange[0], out var start) &&
                        DateTime.TryParse(dataRange.DataRange[1], out var end))
                    {
                        var res = CalculateLatestDayCycle(start.ToString(), days);
                        start = res[0];
                        end = res[1];
                        var now = DateTime.Now;
                        TimeSpan _endCountdownTimeSpan = end - now;
                        TimeSpan _totalDurationTimeSpan = end - start;
                        TimeSpan _overCountdownTimeSpace = now - start;
                        maxVal = _totalDurationTimeSpan.TotalSeconds;
                        double elapsed = _endCountdownTimeSpan.TotalSeconds;

                        if (elapsed <= 0)
                        {
                            currVal = maxVal;
                            msg = "已结束";
                            return (currVal, maxVal, msg);
                        }



                        currVal = _overCountdownTimeSpace.TotalSeconds;
                        msg = $"剩余{_endCountdownTimeSpan.Days}天" +
                                    $"{_endCountdownTimeSpan.Hours}小时" +
                                    $"{_endCountdownTimeSpan.Minutes}分";
                        return (currVal, maxVal, msg);
                    }
                    return (0, 0, string.Empty);
                }
            }
            return (0, 0, string.Empty);
        }

        private void SetWarZoneDataInternal(WeekContentTab item)
        {
            if (item == null) return;

            if (item.Imgs != null && item.Imgs.Count > 0)
            {
                WarZoneImageUrl = item.Imgs[0].Img;
            }

            if (item.InnerTabs != null && item.InnerTabs.Count > 0)
            {
                var buff = item.InnerTabs.FirstOrDefault();
                if (buff != null)
                {
                    WarZoneBuffName = buff.Name;
                    WarZoneBuffDescription = buff.Description;
                }
            }



            var result = SetDataInternal(item, WarZoneProgress, WarZoneMaxProgress, WarZoneRemainingTime);
            WarZoneMaxProgress = result.maxVal;
            WarZoneProgress = result.currVal;
            WarZoneRemainingTime = result.msg;
        }
        public void SetNormanData(SideModule module)
        {
            if (module == null) return;

            if (module.Content is JsonElement element)
            {
                try
                {
                    WeekContentJsonContent? content = null;

                    if (element.ValueKind == JsonValueKind.Array)
                    {
                        var list = JsonSerializer.Deserialize(element, WikiContext.Default.ListWeekContentJsonContent);
                        content = list?.FirstOrDefault();
                    }
                    else
                    {
                        content = JsonSerializer.Deserialize(element, WikiContext.Default.WeekContentJsonContent);
                    }

                    if (content != null && content.Tabs != null && content.Tabs.Count > 0)
                    {
                        var tab = content.Tabs.FirstOrDefault(t => (bool)t.Active) ?? content.Tabs.First();
                        SetNormanDataInternal(tab);
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error parsing Norman data: {ex.Message}");
                }
            }
        }

        private void SetNormanDataInternal(WeekContentTab item)
        {

            if (item == null) return;

            if (item.Imgs != null && item.Imgs.Count > 0)
            {
                NormanImageUrl = item.Imgs[0].Img;
            }

            var result = SetDataInternal(item, NormanProgress, NormanMaxProgress, NormanRemainingTime);
            NormanMaxProgress = (double)result.maxVal;
            NormanProgress = (double)result.currVal;
            NormanRemainingTime = result.msg;
        }

        private void SetPunishCageDataInternal(WeekContentTab item)
        {

            if (item == null) return;


            var result = SetDataInternal(item, PunishCageProgress, PunishCageMaxProgress, PunishRemainingTime, 7);
            PunishCageMaxProgress = result.maxVal;
            PunishCageProgress = result.currVal;
            PunishRemainingTime = result.msg;
        }

        public void SetDisputeData(SideModule module)
        {
            if (module == null) return;
            if (module.Content is JsonElement element)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        TypeInfoResolver = WikiContext.Default
                    };

                    if (element.ValueKind == JsonValueKind.Array)
                    {

                        try
                        {
                            var tabs = element.Deserialize<List<WeekContentTab>>(options);
                            if (tabs != null && tabs.Count > 0 && (!string.IsNullOrEmpty(tabs[0].Name) || tabs[0].Imgs?.Count > 0))
                            {
                                FillDisputeItems(tabs);
                                return;
                            }
                        }
                        catch
                        {

                        }

                        var list = element.Deserialize<List<WeekContentJsonContent>>(options);
                        var content = list?.FirstOrDefault();
                        if (content != null && content.Tabs != null)
                        {
                            FillDisputeItems(content.Tabs);
                        }
                    }
                    else
                    {
                        var content = element.Deserialize<WeekContentJsonContent>(options);
                        if (content != null && content.Tabs != null)
                        {
                            FillDisputeItems(content.Tabs);
                        }
                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error parsing Dispute data: {ex.Message}");
                }
            }
        }

        private void FillDisputeItems(List<WeekContentTab> tabs)
        {
            DisputeWarZoneItems.Clear();
            foreach (var tab in tabs)
            {
                var item = new DisputeWarZoneItem();
                item.Title = tab.Name;
                if (tab.Imgs != null && tab.Imgs.Count > 0)
                {
                    item.ImageUrl = tab.Imgs[0].Img;
                }

                if (tab.CountDown != null)
                {
                    List<string> range = null;
                    if (tab.CountDown.DateRange != null && tab.CountDown.DateRange.Count >= 2)
                    {
                        range = tab.CountDown.DateRange;
                    }
                    else if (tab.CountDown.Repeat != null && tab.CountDown.Repeat.DataRanges != null)
                    {
                        var dr = tab.CountDown.Repeat.DataRanges.FirstOrDefault();
                        if (dr != null) range = dr.DataRange;
                    }

                    if (range != null && range.Count >= 2)
                    {
                        if (DateTime.TryParse(range[0], out var start) && DateTime.TryParse(range[1], out var end))
                        {
                            item.TimeRange = $"{start:MM.dd HH:mm}-{end:MM.dd HH:mm}";

                        }
                    }
                }
                DisputeWarZoneItems.Add(item);
            }
        }

        public void SetPunishCageData(SideModule module)
        {
            if (module == null) return;
            if (module.Content is JsonElement element)
            {
                try
                {
                    var options = new JsonSerializerOptions
                    {
                        PropertyNameCaseInsensitive = true,
                        TypeInfoResolver = WikiContext.Default
                    };

                    WeekContentJsonContent? content = null;
                    if (element.ValueKind == JsonValueKind.Array)
                    {
                        var list = element.Deserialize<List<WeekContentJsonContent>>(options);
                        content = list?.FirstOrDefault();
                    }
                    else
                    {
                        content = element.Deserialize<WeekContentJsonContent>(options);
                    }

                    if (content != null && content.Tabs != null)
                    {
                        var atab = content.Tabs.First();
                        SetPunishCageDataInternal(atab);

                        var items = new List<PunishCageItem>();
                        _punishCageCache.Clear();
                        foreach (var tab in content.Tabs)
                        {

                            if (tab.Imgs != null)
                            {
                                foreach (var img in tab.Imgs)
                                {
                                    items.Add(new PunishCageItem
                                    {
                                        ImageUrl = img.Img
                                    });
                                }
                            }
                            if (!string.IsNullOrEmpty(tab.Name))
                            {
                                var key = tab.Name.Trim();
                                var cacheItem = new PunishCageCacheItem { Items = items };

                                _punishCageCache[key] = cacheItem;
                            }
                        }

                    }
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Error parsing Punish Cage data: {ex.Message}");
                }
            }
        }

        public void UpdatePunishCageContent()
        {
            var key = "高级区";
            if (_punishCageCache.ContainsKey(key))
            {
                var cacheItem = _punishCageCache[key];
                PunishCageItems.Clear();
                foreach (var item in cacheItem.Items)
                {
                    PunishCageItems.Add(item);
                }
                PunishCageProgress = cacheItem.Progress;
            }
        }

        public DateTime[] CalculateLatestDayCycle(string startDateString, int days = 7)
        {
            DateTime startDate = DateTime.Parse(startDateString);


            DateTime now = DateTime.Now;
            TimeSpan totalTimeSpan = now - startDate;
            double totalDays = totalTimeSpan.TotalDays;


            int completeCycles = (int)(totalDays / days);



            DateTime latestFutureDate = startDate.AddDays((completeCycles + 1) * days).AddHours(6);
            DateTime latestPastDate = latestFutureDate.AddDays(-days);

            // 返回数组
            return new DateTime[] { latestPastDate, latestFutureDate };
        }
    }



    public class PunishCageCacheItem
    {
        public List<PunishCageItem> Items { get; set; } = new();
        public string? Status { get; set; }
        public double Progress { get; set; }
    }

    public class PunishCageItem
    {
        public string? ImageUrl { get; set; }
    }

    public class DisputeWarZoneItem
    {
        public string? Title { get; set; }
        public string? ImageUrl { get; set; }
        public string? TimeRange { get; set; }
        public double CurrentProgress { get; set; }
        public double MaxProgress { get; set; }
        public string? RemainingTime { get; set; }
        public string? ColorVal { get; set; }
    }
}
