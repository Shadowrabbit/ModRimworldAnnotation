using System;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BE8 RID: 3048
	public class HediffComp_PsychicSuppression : HediffComp
	{
		// Token: 0x17000C92 RID: 3218
		// (get) Token: 0x060047C8 RID: 18376 RVA: 0x0017B570 File Offset: 0x00179770
		public override bool CompShouldRemove
		{
			get
			{
				if (base.Pawn.SpawnedOrAnyParentSpawned)
				{
					GameCondition_PsychicSuppression activeCondition = base.Pawn.MapHeld.gameConditionManager.GetActiveCondition<GameCondition_PsychicSuppression>();
					if (activeCondition != null && base.Pawn.gender == activeCondition.gender)
					{
						return false;
					}
				}
				else if (base.Pawn.IsCaravanMember())
				{
					bool result = true;
					foreach (Site site in Find.World.worldObjects.Sites)
					{
						foreach (SitePart sitePart in site.parts)
						{
							if (sitePart.def.Worker is SitePartWorker_ConditionCauser_PsychicSuppressor)
							{
								CompCauseGameCondition_PsychicSuppression compCauseGameCondition_PsychicSuppression = sitePart.conditionCauser.TryGetComp<CompCauseGameCondition_PsychicSuppression>();
								if (compCauseGameCondition_PsychicSuppression.ConditionDef.conditionClass == typeof(GameCondition_PsychicSuppression) && compCauseGameCondition_PsychicSuppression.InAoE(base.Pawn.GetCaravan().Tile) && compCauseGameCondition_PsychicSuppression.gender == base.Pawn.gender)
								{
									result = false;
								}
							}
						}
					}
					return result;
				}
				return true;
			}
		}
	}
}
