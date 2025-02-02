
// Type: com.digitalarcsystems.Traveller.DataModel.Power




#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Power : IDescribable
  {
    public string parentTalentName { get; set; }

    public Difficulty difficulty { get; set; }

    public int cost { get; set; }

    public string Description { get; set; }

    public string Name { get; set; }

    public TimeIncrement timeIncrement { get; set; }

    public TalentRangeBands reach { get; set; }

    public Power()
    {
    }

    public Power(Power copyMe)
    {
      this.parentTalentName = copyMe.parentTalentName;
      this.difficulty = copyMe.difficulty;
      this.cost = copyMe.cost;
      this.Name = copyMe.Name;
      this.Description = copyMe.Description;
      this.timeIncrement = copyMe.timeIncrement;
      this.reach = copyMe.reach;
    }

    public Power(
      string name,
      string description,
      Difficulty taskDifficulty,
      int cost_to_use,
      TimeIncrement timeToUse,
      TalentRangeBands range)
    {
      this.Name = name;
      this.Description = description;
      this.difficulty = taskDifficulty;
      this.cost = cost_to_use;
      this.timeIncrement = timeToUse;
      this.reach = range;
    }
  }
}
