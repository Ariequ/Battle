using UnityEngine;
using System.Collections;

public class SkillButtonView : MonoBehaviour
{
	public UITexture texture;

	private SkillMeta skillMeta;

	public void Initialize(SkillMeta skillMeta)
    {
        this.skillMeta = skillMeta;

		texture.mainTexture = Resources.Load<Texture>("UITextures/Magics/magic_" + skillMeta.id.ToString());
    }

    public int SkillID
    {
        get
        {
            return skillMeta.id;
        }
    }
}

