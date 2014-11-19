using System;
using System.Collections.Generic;

using Battle;

public class MockData
{
	//效果Meta
	public Dictionary<int, EffectMeta> effectMetaDic = new Dictionary<int, EffectMeta>();
    //子弹Meta
    public Dictionary<int, BulletMeta> bulletMetaDic = new Dictionary<int, BulletMeta>();
    //士兵Meta
    public Dictionary<int, SoldierMeta> soldierMetaDic = new Dictionary<int, SoldierMeta>();
    //英雄Meta
    public Dictionary<int, HeroMeta> heroMetaDic = new Dictionary<int, HeroMeta>();
    //技能Meta
    public Dictionary<int, SkillMeta> skillMetaDic = new Dictionary<int, SkillMeta>();
    //Buff Meta
    public Dictionary<int, BuffMeta> buffMetaDic = new Dictionary<int, BuffMeta>();
    //主城建筑Meta
    public Dictionary<int, CityBuildingMeta> cityBuildingMetaDic = new Dictionary<int, CityBuildingMeta>();
    //关卡Meta
	public Dictionary<int, LevelMeta> levelMetaDic = new Dictionary<int, LevelMeta> ();
    //探索地图建筑Meta
    public Dictionary<int, MapElementMeta> mapBuildingMetaDic = new Dictionary<int, MapElementMeta>();
    //探索地图建筑Meta
    public Dictionary<int, MapElementMeta> mapMonsterMetaDic = new Dictionary<int, MapElementMeta>();
    public Dictionary<int, MapElementMeta> mapTreasureMetaDic = new Dictionary<int, MapElementMeta>();

    //领主经验配置
    public Dictionary<int, long> lordLevelExpDic = new Dictionary<int, long>();
	
    public MockData()
    {
		EffectMeta effectMeta;
		BulletMeta bulletMeta;
		HeroMeta heroMeta;
		SoldierMeta soldierMeta;
		SkillMeta skillMeta;
		SkillLinkMeta skillLinkMeta;
		CityBuildingMeta cityBuildingMeta;
		LevelMeta levelMeta;
        MapElementMeta mapElementMeta;

		#region Skill

		effectMeta = new EffectMeta();
		effectMeta.id = 1;
		effectMeta.prefabPath = "Effects/Skills/Lightning";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 2;
		effectMeta.prefabPath = "Effects/Skills/Resurrection";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 3;
		effectMeta.prefabPath = "Effects/Skills/Weakness";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 4;
		effectMeta.prefabPath = "Effects/Bullets/Fireball_AfterEffect";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 5;
		effectMeta.prefabPath = "Effects/Bullets/Iceball_AfterEffect";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 6;
		effectMeta.prefabPath = "Effects/Bullets/Arrow";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 7;
		effectMeta.prefabPath = "Effects/Bullets/Fireball";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 8;
		effectMeta.prefabPath = "Effects/Bullets/Iceball";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 9;
		effectMeta.prefabPath = "Effects/Fireman/FireMan";
		effectMetaDic[effectMeta.id] = effectMeta;
		
		effectMeta = new EffectMeta();
		effectMeta.id = 10;
		effectMeta.prefabPath = "Effects/Fireman/IceMan";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 11;
		effectMeta.prefabPath = "Effects/Skills/BingZui";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 12;
		effectMeta.prefabPath = "Effects/Skills/DianGuangQiu";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 13;
		effectMeta.prefabPath = "Effects/Skills/YunShiZhaoHuan";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 14;
		effectMeta.prefabPath = "Effects/Skills/LJF_DanTi";
		effectMetaDic[effectMeta.id] = effectMeta;

		effectMeta = new EffectMeta();
		effectMeta.id = 15;
		effectMeta.prefabPath = "Effects/Skills/LJF_DuoChong";
		effectMetaDic[effectMeta.id] = effectMeta;

		#endregion

		#region Skill
		
		skillMeta = new SkillMeta();
		skillMeta.id = 1;
		skillMeta.metaName = "LJF_DanTi";
		skillMeta.effectID = 14;
		skillMeta.type = SkillType.Damage;
		skillMeta.isAOE = true;
		skillMeta.toSelf = false;
		skillMeta.coreRadius = 10f;
		skillMeta.edgeRadius = 10f;
		skillMeta.angerExpense = 10;
		skillMeta.propertyType = PropertyType.HP;
		skillMeta.minValue = 450;
		skillMeta.maxValue = 500;
		skillMeta.addPercent = 0f;
		skillMeta.maxDistance = 0;
		skillMeta.maxAffectCount = 1;
		skillMetaDic[skillMeta.id] = skillMeta;
		
		skillMeta = new SkillMeta();
		skillMeta.id = 2;
		skillMeta.metaName = "Resurrection";
		skillMeta.effectID = 2;
        skillMeta.type = SkillType.Damage;
		skillMeta.isAOE = true;
		skillMeta.toSelf = true;
		skillMeta.coreRadius = 5f;
		skillMeta.edgeRadius = 5f;
		skillMeta.angerExpense = 100;
		skillMeta.minValue = 0;
		skillMeta.maxValue = 0;
		skillMetaDic[skillMeta.id] = skillMeta;
		
		skillMeta = new SkillMeta();
		skillMeta.id = 3;
		skillMeta.metaName = "LJF_DuoChong";
		skillMeta.effectID = 15;
		skillMeta.type = SkillType.Damage;
		skillMeta.isAOE = true;
		skillMeta.toSelf = false;
		skillMeta.coreRadius = 10f;
		skillMeta.edgeRadius = 10f;
		skillMeta.angerExpense = 10;
		skillMeta.minValue = 45;
		skillMeta.maxValue = 48;
		skillLinkMeta = new SkillLinkMeta();
		skillLinkMeta.id = 1;
		skillLinkMeta.duration = 10000;
		skillLinkMeta.interval = 1000;
		skillMetaDic[skillMeta.id] = skillMeta;

		skillMeta = new SkillMeta();
		skillMeta.id = 4;
		skillMeta.metaName = "YunShiZhaoHuan";
		skillMeta.effectID = 13;
		skillMeta.type = SkillType.Damage;
		skillMeta.isAOE = true;
		skillMeta.toSelf = false;
		skillMeta.coreRadius = 10f;
		skillMeta.edgeRadius = 10f;
		skillMeta.angerExpense = 10;
		skillMeta.propertyType = PropertyType.HP;
		skillMeta.minValue = 3500;
		skillMeta.maxValue = 3600;
		skillMeta.addPercent = 0f;
		skillMeta.maxDistance = 0;
		skillMeta.maxAffectCount = 1;
		skillMetaDic[skillMeta.id] = skillMeta;
		
		skillMeta = new SkillMeta();
		skillMeta.id = 5;
		skillMeta.metaName = "DianGuangQiu";
		skillMeta.effectID = 12;
		skillMeta.type = SkillType.Damage;
		skillMeta.isAOE = true;
		skillMeta.toSelf = false;
		skillMeta.coreRadius = 5f;
		skillMeta.edgeRadius = 5f;
		skillMeta.angerExpense = 10;
		skillMeta.propertyType = PropertyType.HP;
		skillMeta.minValue = 900;
		skillMeta.maxValue = 1000;
		skillMeta.addPercent = 0f;
		skillMeta.maxDistance = 0;
		skillMeta.maxAffectCount = 1;
		skillMetaDic[skillMeta.id] = skillMeta;
		skillMeta = new SkillMeta();

		skillMeta.id = 6;
		skillMeta.metaName = "BingZui";
		skillMeta.effectID = 11;
		skillMeta.type = SkillType.Damage;
		skillMeta.isAOE = true;
		skillMeta.toSelf = false;
		skillMeta.coreRadius = 10f;
		skillMeta.edgeRadius = 10f;
		skillMeta.angerExpense = 10;
		skillMeta.propertyType = PropertyType.HP;
		skillMeta.minValue = 450;
		skillMeta.maxValue = 500;
		skillMeta.addPercent = 0f;
		skillMeta.maxDistance = 0;
		skillMeta.maxAffectCount = 1;
		skillMetaDic[skillMeta.id] = skillMeta;
		
		#endregion

        #region Bullet

        bulletMeta = new BulletMeta();
        bulletMeta.id = 1;
        bulletMeta.metaName = "Arrow";
        bulletMeta.effectID = 6;
		bulletMeta.minSpeed = 15f;
		bulletMeta.maxSpeed = 20f;
		bulletMeta.drag = 6f;
        bulletMeta.isAreaAttack = false;
//      bulletMeta.attackRadius = 0;
        bulletMetaDic[bulletMeta.id] = bulletMeta;

        bulletMeta = new BulletMeta();
        bulletMeta.id = 2;
        bulletMeta.metaName = "Fireball";
		bulletMeta.effectID = 7;
		bulletMeta.areaEffectID = 4;
		bulletMeta.additionEffectID = 9;
		bulletMeta.minSpeed = 18f;
		bulletMeta.maxSpeed = 20f;
        bulletMeta.drag = 1f;
        bulletMeta.isAreaAttack = true;
        bulletMeta.attackRadius = 5f;
        bulletMetaDic[bulletMeta.id] = bulletMeta;

		bulletMeta = new BulletMeta();
		bulletMeta.id = 3;
		bulletMeta.metaName = "Iceball";
		bulletMeta.effectID = 8;
		bulletMeta.areaEffectID = 5;
		bulletMeta.additionEffectID = 10;
		bulletMeta.minSpeed = 18f;
		bulletMeta.maxSpeed = 20f;
		bulletMeta.drag = 1f;
		bulletMeta.isAreaAttack = true;
		bulletMeta.attackRadius = 5f;
		bulletMetaDic[bulletMeta.id] = bulletMeta;

        #endregion

        #region Hero

        heroMeta = new HeroMeta();
        heroMeta.id = 1;
        heroMeta.metaName = "Adam";
        heroMeta.initialAttack = 40;
        heroMeta.initialDefense = 10;
        heroMeta.initialHP = 100;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 1;
		heroMeta.quality = HeroQuality.White;
        heroMetaDic[heroMeta.id] = heroMeta;

        heroMeta = new HeroMeta();
        heroMeta.id = 2;
        heroMeta.metaName = "Arique";
        heroMeta.initialAttack = 20;
        heroMeta.initialDefense = 20;
        heroMeta.initialHP = 200;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 2;
		heroMeta.quality = HeroQuality.Green;
        heroMetaDic[heroMeta.id] = heroMeta;

        heroMeta = new HeroMeta();
        heroMeta.id = 3;
        heroMeta.metaName = "Steven";
        heroMeta.initialAttack = 30;
        heroMeta.initialDefense = 30;
        heroMeta.initialHP = 100;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 3;
		heroMeta.quality = HeroQuality.Blue;
        heroMetaDic[heroMeta.id] = heroMeta;

		heroMeta = new HeroMeta();
		heroMeta.id = 4;
		heroMeta.metaName = "Eric";
		heroMeta.initialAttack = 30;
		heroMeta.initialDefense = 25;
		heroMeta.initialHP = 100;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 4;
		heroMeta.quality = HeroQuality.Purple1;
		heroMetaDic[heroMeta.id] = heroMeta;

		heroMeta = new HeroMeta();
		heroMeta.id = 5;
		heroMeta.metaName = "Daniel";
		heroMeta.initialAttack = 22;
		heroMeta.initialDefense = 20;
		heroMeta.initialHP = 100;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 5;
		heroMeta.quality = HeroQuality.Orange1;
		heroMetaDic[heroMeta.id] = heroMeta;

		heroMeta = new HeroMeta();
		heroMeta.id = 6;
		heroMeta.metaName = "Clark";
		heroMeta.initialAttack = 19;
		heroMeta.initialDefense = 21;
		heroMeta.initialHP = 100;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 6;
		heroMeta.quality = HeroQuality.Purple1;
		heroMetaDic[heroMeta.id] = heroMeta;

		heroMeta = new HeroMeta();
		heroMeta.id = 7;
		heroMeta.metaName = "Thomas";
		heroMeta.initialAttack = 22;
		heroMeta.initialDefense = 17;
		heroMeta.initialHP = 100;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 1;
		heroMeta.quality = HeroQuality.Purple1;
		heroMetaDic[heroMeta.id] = heroMeta;

		heroMeta = new HeroMeta();
		heroMeta.id = 8;
		heroMeta.metaName = "Tom";
		heroMeta.initialAttack = 22;
		heroMeta.initialDefense = 17;
		heroMeta.initialHP = 100;
		heroMeta.skillDic = new Dictionary<int, int>();
		heroMeta.skillDic[1] = 2;
		heroMeta.quality = HeroQuality.Purple1;
		heroMetaDic[heroMeta.id] = heroMeta;

        #endregion

        #region Soldier

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 1;
        soldierMeta.metaName = "RedDragon";
		soldierMeta.layer = Layers.AIR_FORCE;
        soldierMeta.prefabPath = "Soldiers/Flying/M_Long_Red";
        soldierMeta.maxUnitCount = 1;
		soldierMeta.initialAttack = 506;
		soldierMeta.initialDefense = 127;
		soldierMeta.initialHP = 11385;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 2f;
		soldierMeta.attackMaxRange = 20;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 4;
        soldierMeta.sizeLevel = SizeLevel.Large;
        soldierMeta.bulletMeta = bulletMetaDic[2];
        soldierMetaDic[soldierMeta.id] = soldierMeta;

		soldierMeta = new SoldierMeta();
		soldierMeta.id = 2;
		soldierMeta.metaName = "BlueDragon";
		soldierMeta.layer = Layers.AIR_FORCE;
		soldierMeta.prefabPath = "Soldiers/Flying/M_Long_Blue";
		soldierMeta.maxUnitCount = 1;
		soldierMeta.initialAttack = 431;
		soldierMeta.initialDefense = 127;
		soldierMeta.initialHP = 11385;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 2f;
		soldierMeta.attackMaxRange = 20;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 4;
		soldierMeta.sizeLevel = SizeLevel.Large;
		soldierMeta.bulletMeta = bulletMetaDic[3];
		soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 3;
        soldierMeta.metaName = "BigBoss";
		soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Boss/M_Boss_01";
		soldierMeta.maxUnitCount = 1;
		soldierMeta.initialAttack = 759;
		soldierMeta.initialDefense = 190;
		soldierMeta.initialHP = 17077;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 3f;
		soldierMeta.attackMaxRange = 5;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 3;
        soldierMeta.sizeLevel = SizeLevel.Large;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 4;
        soldierMeta.metaName = "Paladin";
		soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/SoldierBlue";
        soldierMeta.maxUnitCount = 9;
		soldierMeta.initialAttack = 319;
		soldierMeta.initialDefense = 94;
		soldierMeta.initialHP = 8443;
		soldierMeta.dodge = 100;
		soldierMeta.attackMaxRange = 4;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
		soldierMeta.boundsRadius = 0.6f;
        soldierMeta.sizeLevel = SizeLevel.Small;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 5;
        soldierMeta.metaName = "Paladin2";
		soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/SoldierYellow";
        soldierMeta.maxUnitCount = 9;
		soldierMeta.initialAttack = 360;
		soldierMeta.initialDefense = 106;
		soldierMeta.initialHP = 9529;
		soldierMeta.dodge = 100;
		soldierMeta.attackMaxRange = 2;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
		soldierMeta.boundsRadius = 0.6f;
        soldierMeta.sizeLevel = SizeLevel.Small;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 6;
        soldierMeta.metaName = "Archer";
		soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Ranged/Archer";
        soldierMeta.maxUnitCount = 9;
		soldierMeta.initialAttack = 278;
		soldierMeta.initialDefense = 70;
		soldierMeta.initialHP = 6236;
		soldierMeta.dodge = 100;
		soldierMeta.attackMaxRange = 30;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
		soldierMeta.boundsRadius = 0.6f;
        soldierMeta.sizeLevel = SizeLevel.Small;
        soldierMeta.bulletMeta = bulletMetaDic[1];
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 7;
        soldierMeta.metaName = "Ogre";
		soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/M_ShiRenMo_1";
        soldierMeta.maxUnitCount = 4;
		soldierMeta.initialAttack = 354;
		soldierMeta.initialDefense = 104;
		soldierMeta.initialHP = 10300;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 1.1f;
		soldierMeta.attackMaxRange = 4;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 3;
        soldierMeta.sizeLevel = SizeLevel.Medium;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 8;
        soldierMeta.metaName = "Ogre2";
		soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/M_ShiRenMo_2";
        soldierMeta.maxUnitCount = 4;
		soldierMeta.initialAttack = 384;
		soldierMeta.initialDefense = 104;
		soldierMeta.initialHP = 9300;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 1.1f;
		soldierMeta.attackMaxRange = 3;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 3;
        soldierMeta.sizeLevel = SizeLevel.Medium;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 9;
        soldierMeta.metaName = "Trooper";
		soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/M_ZhongJiaBing_1";
        soldierMeta.maxUnitCount = 9;
		soldierMeta.initialAttack = 360;
		soldierMeta.initialDefense = 106;
		soldierMeta.initialHP = 9500;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 0.7f;
		soldierMeta.attackMaxRange = 4;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
        soldierMeta.sizeLevel = SizeLevel.Small;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 10;
        soldierMeta.metaName = "DuYanJuShou";
        soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/M_DuYanJuShou_1";
        soldierMeta.maxUnitCount = 1;
		soldierMeta.initialAttack = 360;
		soldierMeta.initialDefense = 129;
		soldierMeta.initialHP = 11546;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 4.0f;
		soldierMeta.attackMaxRange = 5;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 3;
        soldierMeta.sizeLevel = SizeLevel.Large;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 11;
        soldierMeta.metaName = "YuanLing";
        soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/F_YuanLing_1";
        soldierMeta.maxUnitCount = 9;
		soldierMeta.initialAttack = 384;
		soldierMeta.initialDefense = 96;
		soldierMeta.initialHP = 8600;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 0.6f;
		soldierMeta.attackMaxRange = 5;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
        soldierMeta.sizeLevel = SizeLevel.Small;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;

        soldierMeta = new SoldierMeta();
        soldierMeta.id = 12;
        soldierMeta.metaName = "KuMuZhanShi";
        soldierMeta.layer = Layers.ARMY;
        soldierMeta.prefabPath = "Soldiers/Infantry/M_KuMuZhanShi_1";
        soldierMeta.maxUnitCount = 4;
		soldierMeta.initialAttack = 351;
		soldierMeta.initialDefense = 104;
		soldierMeta.initialHP = 9280;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 1.3f;
		soldierMeta.attackMaxRange = 4;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
        soldierMeta.sizeLevel = SizeLevel.Medium;
        soldierMeta.bulletMeta = null;
        soldierMetaDic[soldierMeta.id] = soldierMeta;


		soldierMeta = new SoldierMeta();
		soldierMeta.id = 13;
		soldierMeta.metaName = "FengHuang";
		soldierMeta.layer = Layers.AIR_FORCE;
		soldierMeta.prefabPath = "Soldiers/Flying/D_FengHuang_1";
		soldierMeta.maxUnitCount = 1;
		soldierMeta.initialAttack = 431;
		soldierMeta.initialDefense = 127;
		soldierMeta.initialHP = 11385;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 2f;
		soldierMeta.attackMaxRange = 20;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 4;
		soldierMeta.sizeLevel = SizeLevel.Large;
		soldierMeta.bulletMeta = bulletMetaDic[2];
		soldierMetaDic[soldierMeta.id] = soldierMeta;

		soldierMeta = new SoldierMeta();
		soldierMeta.id = 14;
		soldierMeta.metaName = "DengShenZhiZhu";
		soldierMeta.layer = Layers.ARMY;
		soldierMeta.prefabPath = "Soldiers/Infantry/Z_DengShenZhiZhu_1";
		soldierMeta.maxUnitCount = 4;
		soldierMeta.initialAttack = 351;
		soldierMeta.initialDefense = 104;
		soldierMeta.initialHP = 9280;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 1.3f;
		soldierMeta.attackMaxRange = 4;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
		soldierMeta.sizeLevel = SizeLevel.Medium;
		soldierMeta.bulletMeta = null;
		soldierMetaDic[soldierMeta.id] = soldierMeta;

		soldierMeta = new SoldierMeta();
		soldierMeta.id = 15;
		soldierMeta.metaName = "BingNv";
		soldierMeta.layer = Layers.ARMY;
		soldierMeta.prefabPath = "Soldiers/Infantry/Z_BingNv_1";
		soldierMeta.maxUnitCount = 9;
		soldierMeta.initialAttack = 319;
		soldierMeta.initialDefense = 94;
		soldierMeta.initialHP = 8443;
		soldierMeta.dodge = 100;
		soldierMeta.attackMaxRange = 4;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 2;
		soldierMeta.boundsRadius = 0.6f;
		soldierMeta.sizeLevel = SizeLevel.Medium;
		soldierMeta.bulletMeta = null;
		soldierMetaDic[soldierMeta.id] = soldierMeta;

		soldierMeta = new SoldierMeta();
		soldierMeta.id = 16;
		soldierMeta.metaName = "HuangJinMengMa";
		soldierMeta.layer = Layers.ARMY;
		soldierMeta.prefabPath = "Soldiers/Boss/D_HuangJinMengMa_1";
		soldierMeta.maxUnitCount = 1;
		soldierMeta.initialAttack = 759;
		soldierMeta.initialDefense = 190;
		soldierMeta.initialHP = 17077;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 3f;
		soldierMeta.attackMaxRange = 5;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 3;
		soldierMeta.sizeLevel = SizeLevel.Large;
		soldierMeta.bulletMeta = null;
		soldierMetaDic[soldierMeta.id] = soldierMeta;

		soldierMeta = new SoldierMeta();
		soldierMeta.id = 17;
		soldierMeta.metaName = "TianMaShengNv";
		soldierMeta.layer = Layers.ARMY;
		soldierMeta.prefabPath = "Soldiers/Infantry/Z_TianMaShengNv_1";
		soldierMeta.maxUnitCount = 4;
		soldierMeta.initialAttack = 354;
		soldierMeta.initialDefense = 104;
		soldierMeta.initialHP = 10300;
		soldierMeta.dodge = 100;
		soldierMeta.boundsRadius = 1.1f;
		soldierMeta.attackMaxRange = 4;
		soldierMeta.attackMinRange = 0;
		soldierMeta.attackFrequency = 3;
		soldierMeta.sizeLevel = SizeLevel.Medium;
		soldierMeta.bulletMeta = null;
		soldierMetaDic[soldierMeta.id] = soldierMeta;

        
        #endregion  

        #region Building

        cityBuildingMeta = new CityBuildingMeta();
        cityBuildingMeta.id = 1;
        cityBuildingMeta.metaName = "Parliament_0";
        cityBuildingMeta.type = CityBuildingType.Parliament;
        cityBuildingMeta.productType = CurrencyType.Gold;
        cityBuildingMeta.productAmount = 100;
        cityBuildingMeta.upgradePrice = new Price(1000, 10, 10);
        cityBuildingMeta.upgradeDuration = 23453;
        cityBuildingMeta.upgradeBuildingID = 2;
        cityBuildingMeta.iconPath = "UITextures/BuildingIcon/Parliament";
        cityBuildingMetaDic[cityBuildingMeta.id] = cityBuildingMeta;

        cityBuildingMeta = new CityBuildingMeta();
        cityBuildingMeta.id = 2;
        cityBuildingMeta.metaName = "Parliament_1";
        cityBuildingMeta.type = CityBuildingType.Parliament;
        cityBuildingMeta.productType = CurrencyType.Gold;
        cityBuildingMeta.productAmount = 200;
        cityBuildingMeta.upgradePrice = new Price(2000, 15, 15);
        cityBuildingMeta.upgradeDuration = 546747;
        cityBuildingMeta.upgradeBuildingID = 3;
        cityBuildingMeta.iconPath = "UITextures/BuildingIcon/Parliament";
        cityBuildingMetaDic[cityBuildingMeta.id] = cityBuildingMeta;

        cityBuildingMeta = new CityBuildingMeta();
        cityBuildingMeta.id = 10;
        cityBuildingMeta.metaName = "Defense";
        cityBuildingMeta.type = CityBuildingType.Defense;
        cityBuildingMeta.productType = CurrencyType.Gold;
        cityBuildingMeta.upgradePrice = new Price(2000, 15, 15);
        cityBuildingMeta.upgradeDuration = 546747;
        cityBuildingMeta.upgradeBuildingID = 11;
        cityBuildingMeta.iconPath = "UITextures/BuildingIcon/castle";
        cityBuildingMetaDic[cityBuildingMeta.id] = cityBuildingMeta;

        cityBuildingMeta = new CityBuildingMeta();
        cityBuildingMeta.id = 20;
        cityBuildingMeta.metaName = "Goldmine";
        cityBuildingMeta.type = CityBuildingType.Goldmine;
        cityBuildingMeta.productType = CurrencyType.Gold;
        cityBuildingMeta.upgradePrice = new Price(2000, 15, 15);
        cityBuildingMeta.upgradeDuration = 546747;
        cityBuildingMeta.upgradeBuildingID = 21;
        cityBuildingMeta.iconPath = "UITextures/BuildingIcon/goldmine";
        cityBuildingMetaDic[cityBuildingMeta.id] = cityBuildingMeta;

        cityBuildingMeta = new CityBuildingMeta();
        cityBuildingMeta.id = 30;
        cityBuildingMeta.metaName = "Smithy";
        cityBuildingMeta.type = CityBuildingType.Smithy;
        cityBuildingMeta.productType = CurrencyType.Gold;
        cityBuildingMeta.upgradePrice = new Price(2000, 15, 15);
        cityBuildingMeta.upgradeDuration = 546747;
        cityBuildingMeta.upgradeBuildingID = 31;
        cityBuildingMeta.iconPath = "UITextures/BuildingIcon/smithy";
        cityBuildingMetaDic[cityBuildingMeta.id] = cityBuildingMeta;

        #endregion

		#region Level

		levelMeta = new LevelMeta();
		levelMeta.id = 1;
		levelMeta.name = "Saint Mountain";
		levelMeta.description = "床前明月光，疑是地上霜。举头望明月，低头思故乡。";
		levelMeta.selfAnchor = new Vector2(96, 23);
		levelMeta.opponentAnchor = new Vector2(42f, 83f);

		levelMeta.selfPositions = new Vector2[]
		{
			new Vector2(-4.5f, 9f),
			new Vector2(4.5f, 9f),
			new Vector2(-9f, 0),
			new Vector2(0, 0),
			new Vector2(9f, 0),
			
		};
		levelMeta.opponentPositions = new Vector2[]
		{
			new Vector2(-4.5f, 9f),
			new Vector2(4.5f, 9f),
			new Vector2(-9f, 0),
			new Vector2(0, 0),
			new Vector2(9f, 0),
			
		};

		// Archer YuanLing FengHuang YuanLing DengShenZhiZhu
		levelMeta.enemySoldierIDs = new int[] { 6, 11, 13, 11, 14 };
		levelMeta.enemySoldierLevels = new int[] { 1, 1, 1, 1, 1 };
		levelMeta.enemyHeroIDs = new int[] { 1, 1, 4 };
		levelMeta.enemyHeroLevels = new int[] { 1, 1, 1 };
		levelMeta.enemyHeroStarLevels = new int[] { 1, 1, 1 };
		levelMeta.battlefieldID = "001";
		levelMeta.battlefieldSize = 100;

		levelMetaDic[levelMeta.id] = levelMeta;

        levelMeta = new LevelMeta();
        levelMeta.id = 2;
        levelMeta.name = "Barbarian Cave";
        levelMeta.description = "少小离家老大回，安能辨我是雄雌？";
        levelMeta.selfAnchor = new Vector2(96, 23);
        levelMeta.opponentAnchor = new Vector2(42f, 83f);
        
        levelMeta.selfPositions = new Vector2[]
        {
            new Vector2(-4.5f, 9f),
            new Vector2(4.5f, 9f),
            new Vector2(-9f, 0),
            new Vector2(0, 0),
            new Vector2(9f, 0),
            
        };
        levelMeta.opponentPositions = new Vector2[]
        {
            new Vector2(-4.5f, 9f),
            new Vector2(4.5f, 9f),
            new Vector2(-9f, 0),
            new Vector2(0, 0),
            new Vector2(9f, 0),
            
        };

		//honglong pangzi pangzi daobing bingnv
		levelMeta.enemySoldierIDs = new int[] { 1, 7, 16, 4, 4 };

		//Huangjinmengma
//		levelMeta.enemySoldierIDs = new int[] { 16, 7, 8, 4, 15 };
        levelMeta.enemySoldierLevels = new int[] { 1, 1, 1, 1, 1 };
		levelMeta.enemyHeroIDs = new int[] { 1, 2, 4 };
		levelMeta.enemyHeroLevels = new int[] { 1, 1, 1 };
        levelMeta.enemyHeroStarLevels = new int[] { 1, 1, 1 };
		levelMeta.battlefieldID = "001";
		levelMeta.battlefieldSize = 100;
        
        levelMetaDic[levelMeta.id] = levelMeta;

		#endregion

        #region MapElement

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 1;
        mapElementMeta.prefabPath = "Buildings/Building_1";
        mapElementMeta.type = MapElementType.Building;
        mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 2;
        mapElementMeta.prefabPath = "Buildings/Building_2";
        mapElementMeta.type = MapElementType.Building;
        mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 3;
        mapElementMeta.prefabPath = "Buildings/Building_3";
        mapElementMeta.type = MapElementType.Building;
        mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 4;
        mapElementMeta.prefabPath = "Buildings/Building_4";
        mapElementMeta.type = MapElementType.Building;
        mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 5;
        mapElementMeta.prefabPath = "Buildings/Building_5";
        mapElementMeta.type = MapElementType.Building;
        mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

		mapElementMeta = new MapElementMeta();
		mapElementMeta.id = 6;
		mapElementMeta.prefabPath = "Buildings/Building_6";
		mapElementMeta.type = MapElementType.Building;
		mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

		mapElementMeta = new MapElementMeta();
		mapElementMeta.id = 7;
		mapElementMeta.prefabPath = "Buildings/Building_7";
		mapElementMeta.type = MapElementType.Building;
		mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

		mapElementMeta = new MapElementMeta();
		mapElementMeta.id = 8;
		mapElementMeta.prefabPath = "Buildings/Building_8";
		mapElementMeta.type = MapElementType.Building;
		mapBuildingMetaDic[mapElementMeta.id] = mapElementMeta;

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 1;
        mapElementMeta.prefabPath = "Monsters/Monster_1";
        mapElementMeta.type = MapElementType.Monster;
        mapMonsterMetaDic[mapElementMeta.id] = mapElementMeta;

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 2;
        mapElementMeta.prefabPath = "Monsters/Monster_2";
        mapElementMeta.type = MapElementType.Monster;
        mapMonsterMetaDic[mapElementMeta.id] = mapElementMeta;

        mapElementMeta = new MapElementMeta();
        mapElementMeta.id = 1;
        mapElementMeta.prefabPath = "Treasures/Chest_1";
        mapElementMeta.type = MapElementType.Treasure;
        mapTreasureMetaDic[mapElementMeta.id] = mapElementMeta;

        #endregion

        #region LordLevelExp

        lordLevelExpDic[1] = 10L;
        lordLevelExpDic[2] = 15L;
        lordLevelExpDic[3] = 20L;
        lordLevelExpDic[4] = 30L;
        lordLevelExpDic[5] = 30L;
        lordLevelExpDic[6] = 40L;
        lordLevelExpDic[7] = 50L;
        lordLevelExpDic[8] = 70L;
        lordLevelExpDic[9] = 90L;
        lordLevelExpDic[10] = 100L;

        #endregion
    }
}

