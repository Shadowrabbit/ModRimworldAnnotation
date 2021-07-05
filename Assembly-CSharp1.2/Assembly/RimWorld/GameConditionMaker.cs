using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02001170 RID: 4464
	public static class GameConditionMaker
	{
		// Token: 0x06006252 RID: 25170 RVA: 0x00043A51 File Offset: 0x00041C51
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1);
			gameCondition.Permanent = true;
			gameCondition.startTick -= 180000;
			return gameCondition;
		}

		// Token: 0x06006253 RID: 25171 RVA: 0x001EB9E4 File Offset: 0x001E9BE4
		public static GameCondition MakeCondition(GameConditionDef def, int duration = -1)
		{
			GameCondition gameCondition = (GameCondition)Activator.CreateInstance(def.conditionClass);
			gameCondition.startTick = Find.TickManager.TicksGame;
			gameCondition.def = def;
			gameCondition.Duration = duration;
			gameCondition.uniqueID = Find.UniqueIDsManager.GetNextGameConditionID();
			gameCondition.PostMake();
			return gameCondition;
		}
	}
}
