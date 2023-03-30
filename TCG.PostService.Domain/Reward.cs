namespace TCG.PostService.Domain;

public class Reward
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int RewardItemId { get; set; }

    //Lien vers RewardType
    public int RewardTypeId { get; set; }
    public RewardType RewardType { get; set; }
    

    public ICollection<AvailableReward> AvailableRewards { get; set; }

    public ICollection<AttribuedReward> AttribuedRewards { get; set; }
}