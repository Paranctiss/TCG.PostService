namespace TCG.PostService.Domain;

public class RewardType
{
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal DropRate { get; set; }

    public ICollection<Reward> Rewards { get; set; }
}