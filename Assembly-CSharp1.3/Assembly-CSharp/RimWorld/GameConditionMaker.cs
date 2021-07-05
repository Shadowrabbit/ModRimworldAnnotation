using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BD9 RID: 3033
	public static class GameConditionMaker
	{
		// Token: 0x06004754 RID: 18260 RVA: 0x0017999A File Offset: 0x00177B9A
		public static GameCondition MakeConditionPermanent(GameConditionDef def)
		{
			GameCondition gameCondition = GameConditionMaker.MakeCondition(def, -1);
			gameCondition.Permanent = true;
			gameCondition.startTick -= 180000;
			return gameCondition;
		}

		// Token: 0x06004755 RID: 18261 RVA: 0x001799BC File Offset: 0x00177BBC
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
