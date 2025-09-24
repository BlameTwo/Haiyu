namespace Haiyu.Models.Args;

public class RecordArgs
{
    public RecordArgs(
        IEnumerable<RecordCardItemWrapper>? roleActivity,
        IEnumerable<RecordCardItemWrapper>? weaponsActivity,
        IEnumerable<RecordCardItemWrapper>? roleResident,
        IEnumerable<RecordCardItemWrapper>? weaponsResident,
        IEnumerable<RecordCardItemWrapper>? beginner,
        IEnumerable<RecordCardItemWrapper>? beginnerChoice,
        IEnumerable<RecordCardItemWrapper>? gratitudeOrientation
    )
    {
        RoleActivity = roleActivity;
        WeaponsActivity = weaponsActivity;
        RoleResident = roleResident;
        WeaponsResident = weaponsResident;
        Beginner = beginner;
        BeginnerChoice = beginnerChoice;
        GratitudeOrientation = gratitudeOrientation;
    }

    public RecordRequest Request { get; set; }

    public IEnumerable<int> Roles { get; set; }

    public IEnumerable<int> Weapons { get; set; }
    public CardPoolType Type { get; internal set; }
    public IEnumerable<RecordCardItemWrapper>? RoleActivity { get; }
    public IEnumerable<RecordCardItemWrapper>? WeaponsActivity { get; }
    public IEnumerable<RecordCardItemWrapper>? RoleResident { get; }
    public IEnumerable<RecordCardItemWrapper>? WeaponsResident { get; }
    public IEnumerable<RecordCardItemWrapper>? Beginner { get; }
    public IEnumerable<RecordCardItemWrapper>? BeginnerChoice { get; }
    public IEnumerable<RecordCardItemWrapper>? GratitudeOrientation { get; }
    public FiveGroupModel? FiveGroup { get; internal set; }
    public List<CommunityRoleData>? AllRole { get; internal set; }

    public List<CommunityWeaponData>? AllWeapon { get; internal set; }
}
