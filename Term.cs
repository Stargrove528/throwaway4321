
// Type: com.digitalarcsystems.Traveller.DataModel.Term




using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

#nullable disable
namespace com.digitalarcsystems.Traveller.DataModel
{
  public class Term
  {
    public int num_mustering_out_benefits_awarded_this_term = 0;
    public static int column_width = 80;
    public int additional_mustering_out_benefits;
    public int advancementModifier = 0;
    [JsonProperty]
    public List<Outcome> benefits = new List<Outcome>();
    public string charConnectedTo;
    public Event connectionEvent;
    public bool drafted = false;
    public List<Event> events = new List<Event>();
    public int musteringOutBenefitRollModifierCurrentTerm = 0;
    public int numberOfTermsInCareerIncludingThisOne = 0;
    public int pension_paid_out_for_this_career = 0;
    public bool officer = false;
    public int rank = 0;
    public int survivalModifier = 0;
    public int term = 0;
    public string title = "";
    public int total_ranks_in_career = 0;
    private string ls = Environment.NewLine;

    public string career_category { get; set; }

    public string careerName { get; set; }

    public string Notes { get; set; }

    public string specializationName { get; set; }

    public virtual void addConnection(string characterName) => this.charConnectedTo = characterName;

    public virtual bool hasConnection() => !string.IsNullOrEmpty(this.charConnectedTo);

    public override string ToString()
    {
      StringBuilder stringBuilder1 = new StringBuilder("TERM: " + this.term.ToString() + "\n");
      for (int index = 0; index < Term.column_width; ++index)
        stringBuilder1.Append("=");
      if (this.drafted)
        stringBuilder1.Append(this.ls + "\t\t\t\tDRAFTED" + this.ls);
      stringBuilder1.Append(this.ls + this.careerName + " (" + this.specializationName + ")\tRank: " + this.rank.ToString());
      if (this.title != null)
        stringBuilder1.Append("\tTitle: " + this.title);
      stringBuilder1.Append(this.ls + this.ls + "\tBenefits:" + this.ls + "\t\t");
      StringBuilder stringBuilder2 = new StringBuilder();
      for (int index = 0; index < this.benefits.Count; ++index)
      {
        if (index != 0)
          stringBuilder2.Append(", ");
        stringBuilder2.Append((object) this.benefits[index]);
      }
      stringBuilder1.Append((object) stringBuilder2);
      if (this.events.Count > 0)
      {
        stringBuilder1.Append(this.ls + "\tEvents:" + this.ls);
        foreach (Event @event in this.events)
        {
          stringBuilder1.Append("\t\t" + @event.Description + this.ls);
          if (@event.Description == null || @event.Description.Length == 0)
            EngineLog.Print("Term.cs: " + @event.GetType().Name + " had an empty description");
          if (@event == this.connectionEvent)
            stringBuilder1.Append("\t\t\tConnected with " + this.charConnectedTo);
        }
      }
      if (!stringBuilder1.ToString().EndsWith(this.ls, StringComparison.Ordinal))
        stringBuilder1.Append(this.ls);
      for (int index = 0; index < Term.column_width; ++index)
        stringBuilder1.Append("=");
      return stringBuilder1.ToString();
    }

    [JsonConstructor]
    public Term()
    {
    }
  }
}
