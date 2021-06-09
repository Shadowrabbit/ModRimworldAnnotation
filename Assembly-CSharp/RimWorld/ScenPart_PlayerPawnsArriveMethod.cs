using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x0200160B RID: 5643
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		// Token: 0x06007AB3 RID: 31411 RVA: 0x0005278D File Offset: 0x0005098D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlayerPawnsArriveMethod>(ref this.method, "method", PlayerPawnsArriveMethod.Standing, false);
		}

		// Token: 0x06007AB4 RID: 31412 RVA: 0x0024F224 File Offset: 0x0024D424
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			if (Widgets.ButtonText(listing.GetScenPartRect(this, ScenPart.RowHeight), this.method.ToStringHuman(), true, true, true))
			{
				List<FloatMenuOption> list = new List<FloatMenuOption>();
				foreach (object obj in Enum.GetValues(typeof(PlayerPawnsArriveMethod)))
				{
					PlayerPawnsArriveMethod localM2 = (PlayerPawnsArriveMethod)obj;
					PlayerPawnsArriveMethod localM = localM2;
					list.Add(new FloatMenuOption(localM.ToStringHuman(), delegate()
					{
						this.method = localM;
					}, MenuOptionPriority.Default, null, null, 0f, null, null));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x06007AB5 RID: 31413 RVA: 0x000527A7 File Offset: 0x000509A7
		public override string Summary(Scenario scen)
		{
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				return "ScenPart_ArriveInDropPods".Translate();
			}
			return null;
		}

		// Token: 0x06007AB6 RID: 31414 RVA: 0x000527C3 File Offset: 0x000509C3
		public override void Randomize()
		{
			this.method = ((Rand.Value < 0.5f) ? PlayerPawnsArriveMethod.DropPods : PlayerPawnsArriveMethod.Standing);
		}

		// Token: 0x06007AB7 RID: 31415 RVA: 0x0024F2FC File Offset: 0x0024D4FC
		public override void GenerateIntoMap(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			List<List<Thing>> list = new List<List<Thing>>();
			foreach (Pawn item in Find.GameInitData.startingAndOptionalPawns)
			{
				list.Add(new List<Thing>
				{
					item
				});
			}
			List<Thing> list2 = new List<Thing>();
			foreach (ScenPart scenPart in Find.Scenario.AllParts)
			{
				list2.AddRange(scenPart.PlayerStartingThings());
			}
			int num = 0;
			foreach (Thing thing in list2)
			{
				if (thing.def.CanHaveFaction)
				{
					thing.SetFactionDirect(Faction.OfPlayer);
				}
				list[num].Add(thing);
				num++;
				if (num >= list.Count)
				{
					num = 0;
				}
			}
			DropPodUtility.DropThingGroupsNear_NewTmp(MapGenerator.PlayerStartSpot, map, list, 110, Find.GameInitData.QuickStarted || this.method != PlayerPawnsArriveMethod.DropPods, true, true, true, false);
		}

		// Token: 0x06007AB8 RID: 31416 RVA: 0x000527DB File Offset: 0x000509DB
		public override void PostMapGenerate(Map map)
		{
			if (Find.GameInitData == null)
			{
				return;
			}
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				PawnUtility.GiveAllStartingPlayerPawnsThought(ThoughtDefOf.CrashedTogether);
			}
		}

		// Token: 0x0400506A RID: 20586
		private PlayerPawnsArriveMethod method;
	}
}
