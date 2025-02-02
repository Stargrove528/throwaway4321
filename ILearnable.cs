
// Type: com.digitalarcsystems.Traveller.DataModel.ILearnable




using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public interface ILearnable : IDescribable
  {
    int Level { set; get; }

    void increment();

    ILearnable Parent { get; }

    void setParent(ILearnable parent);

    List<Attribute> Attributes { get; set; }

    Attribute GetPrimaryAttribute();

    bool Cascade { get; }

    bool Specialization { get; }

    IList<Skill> Specializations { get; }

    void Add(ILearnable addMe);
  }
}
