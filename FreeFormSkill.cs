
// Type: com.digitalarcsystems.Traveller.DataModel.FreeFormSkill




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public sealed class FreeFormSkill : Skill
  {
    public const string DefaultName = "Custom Skill";
    public const string DefaultDescription = "This is a custom skill that you can define.";

    public FreeFormSkill()
    {
      this.Name = "Custom Skill";
      this.Description = "This is a custom skill that you can define.";
    }

    public FreeFormSkill(string name, string description, ISkill parent)
    {
      this.Parent = parent;
      this.Name = name;
      this.Description = description;
    }

    public CustomSkill CreateCustomSkill(IDescribable describable)
    {
      return this.CreateCustomSkill(describable.Name, describable.Description);
    }

    public CustomSkill CreateCustomSkill(string name, string description)
    {
      CustomSkill customSkill = new CustomSkill();
      if (!string.IsNullOrEmpty(name))
        customSkill.Name = name;
      if (!string.IsNullOrEmpty(description))
        customSkill.Description = description;
      customSkill.Parent = this.Parent;
      if (this.Categories != null && this.Categories.Count > 0)
        customSkill.Categories.AddRange((IEnumerable<string>) this.Categories);
      return customSkill;
    }
  }
}
