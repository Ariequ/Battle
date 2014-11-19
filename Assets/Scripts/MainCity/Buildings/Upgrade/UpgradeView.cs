using UnityEngine;
using System.Collections;

public class UpgradeView : BasePopupView
{
    public UISprite closeButton;
    public UISprite upgradeButton;

    public UILabel titleLabel;
    public UILabel upgradeDurationLabel;
    public UILabel currentDescLabel;
    public UILabel nextDestLabel;

    public UILabel coinCostLabel;
    public UILabel woodCostLabel;
    public UILabel oreCostLabel;

    public UITexture icon;

    private bool isUpgrading;
    private float remainTime;

    void Start ()
    {
        isUpgrading = false;
        remainTime = -1f;
    }

    void Update ()
    {
        remainTime -= Time.deltaTime;
    }

    public void UpdateUI(int buildingMetaID, bool isUpgrading, long remainTime)
    {
        CityBuildingMeta buildingMeta = MetaManager.Instance.GetCityBuildingMeta(buildingMetaID);

        if (buildingMeta == null)
        {
            return;
        }

        string titleKey = "upgrade_title_" + buildingMetaID;
        titleLabel.text = Language.GetContentOfKey(titleKey);

        string currentDescKey = "upgrade_desc_" + buildingMetaID;
        currentDescLabel.text = Language.GetContentOfKey(currentDescKey);

        string nextDescKey = "upgrade_desc_" + buildingMeta.upgradeBuildingID;
        nextDestLabel.text = Language.GetContentOfKey(nextDescKey);

        Texture texture = ResourceFacade.Instance.LoadTexture2D(buildingMeta.iconPath);
        icon.mainTexture = texture;

        this.remainTime = (float)remainTime;
        this.isUpgrading = isUpgrading;

        updateTimeLabel();

        if (isUpgrading)
        {
            StartCoroutine(updateRemainTime());
        }
    }

    private IEnumerator updateRemainTime()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.5f);

            updateTimeLabel();
        }
    }

    private void updateTimeLabel()
    {
        if (isUpgrading && remainTime >= 0)
            upgradeDurationLabel.text = CommonUtil.ParseTimeToString((long)remainTime);
    }
}

