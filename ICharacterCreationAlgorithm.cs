
// Type: com.digitalarcsystems.Traveller.ICharacterCreationAlgorithm




using com.digitalarcsystems.Traveller.DataModel;
using System.Collections.Generic;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public interface ICharacterCreationAlgorithm
  {
    Character initialize(
      IList<Race> races,
      IList<ISkill> skills,
      IList<World> worlds,
      IList<Career> careers,
      IList<Career> draftCareers,
      IList<Character> partyMembers,
      IList<Contact> peopleToMeet,
      IList<Talent> talentList);

    IDecisionMaker decisionMaker { set; }

    GenerationState selectRace(GenerationState currentState);

    GenerationState generateStats(GenerationState currentState);

    GenerationState chooseHomeworld(GenerationState currentState);

    GenerationState gainBackgroundSkills(GenerationState currentState);

    GenerationState chooseCareer(GenerationState currentState);

    GenerationState getBasicTraining(GenerationState currentState);

    GenerationState chooseCareerSpecialization(GenerationState currentState);

    GenerationState chooseSkillsAndTrainingTableAndObtainSkill(GenerationState currentState);

    GenerationState rollForSurvival(GenerationState currentState);

    GenerationState rollMishap(GenerationState currentState);

    GenerationState rollEvents(GenerationState currentState);

    GenerationState processAdvancementOrCommission(GenerationState currentState);

    GenerationState handleAging(GenerationState currentState);

    GenerationState generateMusteringOutBenefits(GenerationState currentState);

    GenerationState chooseEndOfTermAction(GenerationState currentState);

    GenerationState establishConnectionWithOtherCharacter(
      GenerationState currentState,
      Character newFriend);

    GenerationState chooseConnectionSkills(GenerationState currentState);

    GenerationState selectSkillPackage(GenerationState currentState);

    GenerationState selectSkillFromSharedPackage(GenerationState currentStae);

    GenerationState chooseOneFreeSkill(GenerationState currentState);

    GenerationState handleInjuries(GenerationState currentState);

    GenerationState purchaseEquipment(GenerationState currentState);

    GenerationState getCurrentGenerationState();

    List<Configuration> CurrentConfiguration { get; }

    void setConfiguration(Configuration setMe);

    void setConfiguration(List<Configuration> setUs);

    Character generateCharacter();

    Character generateCharacter(GenerationState restoredState);
  }
}
