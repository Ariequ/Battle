using UnityEngine;
using System.Collections;

public class MainCitySceneInitializer : MonoBehaviour
{
    private MockData mockData = new MockData();

    private ApplicationFacade facade;

    void Awake()
    {
        Language.LoadLanguage("Language/CN");

        MetaManager.InitializeSingleton(mockData);
        MockServer.Initialize();
        
        facade = ApplicationFacade.Instance;

        facade.RegisterProxy(new LordProxy());
        
        facade.RegisterMediator(new MainCityMediator());
        facade.RegisterMediator(new MainCityUIMediator());

        facade.RegisterMediator(new SmithyMediator());
		facade.RegisterMediator(new HeroUIMediator());
		facade.RegisterMediator(new HeroDetailsMediator());
		facade.RegisterMediator(new HeroUpStarMediator());

    }

    void OnDestroy()
    {
        facade.RemoveMediator(MainCityMediator.NAME);
        facade.RemoveMediator(MainCityUIMediator.NAME);
		facade.RemoveMediator(HeroUIMediator.NAME);
		facade.RemoveMediator(HeroDetailsMediator.NAME);
		facade.RemoveMediator(HeroUpStarMediator.NAME);
        facade.RemoveProxy(LordProxy.NAME);
    }
	
}

