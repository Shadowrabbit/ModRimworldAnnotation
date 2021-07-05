using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001010 RID: 4112
	public class ScenPart_PlayerPawnsArriveMethod : ScenPart
	{
		// Token: 0x060060EB RID: 24811 RVA: 0x0020F1CD File Offset: 0x0020D3CD
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<PlayerPawnsArriveMethod>(ref this.method, "method", PlayerPawnsArriveMethod.Standing, false);
		}

		// Token: 0x060060EC RID: 24812 RVA: 0x0020F1E8 File Offset: 0x0020D3E8
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
					}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0));
				}
				Find.WindowStack.Add(new FloatMenu(list));
			}
		}

		// Token: 0x060060ED RID: 24813 RVA: 0x0020F2C4 File Offset: 0x0020D4C4
		public override string Summary(Scenario scen)
		{
			if (this.method == PlayerPawnsArriveMethod.DropPods)
			{
				return "ScenPart_ArriveInDropPods".Translate();
			}
			return null;
		}

		// Token: 0x060060EE RID: 24814 RVA: 0x0020F2E0 File Offset: 0x0020D4E0
		public override void Randomize()
		{
			this.method = ((Rand.Value < 0.5f) ? PlayerPawnsArriveMethod.DropPods : PlayerPawnsArriveMethod.Standing);
		}

		// Token: 0x060060EF RID: 24815 RVA: 0x0020F2F8 File Offset: 0x0020D4F8
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
			DropPodUtility.DropThingGroupsNear(MapGenerator.PlayerStartSpot, map, list, 110, Find.GameInitData.QuickStarted || this.method != PlayerPawnsArriveMethod.DropPods, true, true, true, false, false);
		}

		// Token: 0x060060F0 RID: 24816 RVA: 0x0020F460 File Offset: 0x0020D660
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

		// Token: 0x04003756 RID: 14166
		private PlayerPawnsArriveMethod method;
	}
}
