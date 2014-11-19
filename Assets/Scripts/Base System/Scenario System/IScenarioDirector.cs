public interface IScenarioDirector
{
    void Initialize();
    void PlayNext();
    void PlayScenario(IScenario scenario);
}
