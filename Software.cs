
// Type: com.digitalarcsystems.Traveller.DataModel.Equipment.Software




using Newtonsoft.Json;
using System;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel.Equipment
{
  public class Software : com.digitalarcsystems.Traveller.DataModel.Equipment.Equipment
  {
    [JsonProperty]
    private int _rating;
    [JsonProperty]
    private int _currentRating;

    [JsonIgnore]
    public int Rating
    {
      get => this._rating;
      set
      {
        this.CurrentRating += value - this._rating;
        this._rating = value;
      }
    }

    [JsonIgnore]
    public int MaxRating
    {
      get => this.Rating;
      set => this.Rating = value;
    }

    [JsonIgnore]
    public int CurrentRating
    {
      get => this._currentRating;
      set
      {
        if (value > this.MaxRating)
          this._currentRating = this.MaxRating;
        else
          this._currentRating = value > 0 ? value : 0;
      }
    }

    public Software()
    {
      this.MaxRating = 0;
      this.CurrentRating = 0;
    }

    public Software(int maxRating)
    {
      this.MaxRating = maxRating;
      this.CurrentRating = maxRating;
    }

    [JsonConstructor]
    public Software(int maxRating, int currentRating)
    {
      this.MaxRating = maxRating;
      this.CurrentRating = currentRating;
    }

    public Software(Software copyMe)
      : base((IEquipment) copyMe)
    {
      this.MaxRating = copyMe.MaxRating;
      this.CurrentRating = copyMe.CurrentRating;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is Software software))
        return false;
      Guid guid = this.Id;
      int num;
      if (guid.Equals(software.Id))
      {
        guid = this.InstanceID;
        num = guid.Equals(software.InstanceID) ? 1 : 0;
      }
      else
        num = 0;
      return num != 0;
    }

    public override int GetHashCode()
    {
      return (23 * 31 + this.Id.GetHashCode()) * 31 + this.InstanceID.GetHashCode();
    }
  }
}
