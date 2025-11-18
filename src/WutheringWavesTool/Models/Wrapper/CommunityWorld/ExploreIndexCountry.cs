namespace Haiyu.Models.Wrapper.CommunityWorld
{
    public partial class ExploreIndexCountry : ObservableObject
    {
        public int CountryId { get; set; }

        [ObservableProperty]
        public partial string DisplayName { get; set; }

        [ObservableProperty]
        public partial string Icon { get; set; }

        public ExploreIndexCountry(ExploreList countryId)
        {
            CountryId = countryId.Country.CountryId;
            Icon = countryId.Country.HomePageIcon;
            DisplayName = countryId.Country.CountryName;
        }
    }
}
