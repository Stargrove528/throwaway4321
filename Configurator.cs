
// Type: com.digitalarcsystems.Traveller.Configurator




using com.digitalarcsystems.Traveller.DataModel;

#nullable disable
namespace com.digitalarcsystems.Traveller
{
  public abstract class Configurator : IDescribable
  {
    protected Configuration _currentConfiguration;
    protected bool _enabled = false;
    protected ICharacterCreationAlgorithm engine = (ICharacterCreationAlgorithm) null;

    public string Name { get; set; }

    public string Description { get; set; }

    public bool isEnabled() => this._enabled;

    public Configurator(ICharacterCreationAlgorithm engine)
    {
      this.engine = engine;
      this.Name = this.GetType().Name;
    }

    public bool RequiresProcessBeforeNextOperation { get; protected set; }

    public abstract void processBeforeNextOperation(GenerationState currentState);

    public abstract Configuration generateDefaultConfiguration();

    public Configuration CurrentConfiguration
    {
      get
      {
        if (this._currentConfiguration == null)
          this._currentConfiguration = this.generateDefaultConfiguration();
        return this._currentConfiguration;
      }
    }

    public abstract void handleConfiguration(Configuration handleMe);
  }
}
