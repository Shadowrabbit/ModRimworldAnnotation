using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000975 RID: 2421
	public class ThoughtWorker_GauranlenConnectionDesired : ThoughtWorker_Precept
	{
		// Token: 0x06003D64 RID: 15716 RVA: 0x00151F08 File Offset: 0x00150108
		protected override ThoughtState ShouldHaveThought(Pawn p)
		{
			if (p.connections != null && p.connections.ConnectedThings.Any<Thing>())
			{
				return true;
			}
			if (p.playerSettings == null || p.IsSlave || p.IsPrisoner || p.IsQuestLodger())
			{
				return false;
			}
			if (p.WorkTypeIsDisabled(WorkTypeDefOf.PlantCutting))
			{
				return false;
			}
			int num = Mathf.Max(new int[]
			{
				0,
				p.ideo.joinTick,
				p.playerSettings.joinTick
			});
			return Find.TickManager.TicksGame - num >= 900000;
		}

		// Token: 0x06003D65 RID: 15717 RVA: 0x00151FB4 File Offset: 0x001501B4
		protected override ThoughtState CurrentStateInternal(Pawn p)
		{
			if (!base.CurrentStateInternal(p).Active)
			{
				return ThoughtState.Inactive;
			}
			int stageIndex = 0;
			if (p.connections == null || p.connections.ConnectedThings.NullOrEmpty<Thing>())
			{
				if (p.MapHeld == null)
				{
					stageIndex = 1;
				}
				else
				{
					ExpectationDef expectationDef = ExpectationsUtility.CurrentExpectationFor(p.MapHeld);
					if (expectationDef != null)
					{
						if (expectationDef.order <= ExpectationDefOf.VeryLow.order)
						{
							stageIndex = 1;
						}
						else if (expectationDef.order <= ExpectationDefOf.Low.order)
						{
							stageIndex = 2;
						}
						else if (expectationDef.order <= ExpectationDefOf.Moderate.order)
						{
							stageIndex = 3;
						}
						else if (expectationDef.order <= ExpectationDefOf.High.order)
						{
							stageIndex = 4;
						}
						else
						{
							stageIndex = 5;
						}
					}
				}
			}
			return ThoughtState.ActiveAtStage(stageIndex);
		}

		// Token: 0x040020D4 RID: 8404
		private const int TicksJoinedMin = 900000;
	}
}
