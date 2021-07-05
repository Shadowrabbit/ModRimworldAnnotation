using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001339 RID: 4921
	public class ChoiceLetter_RansomDemand : ChoiceLetter
	{
		// Token: 0x170014D8 RID: 5336
		// (get) Token: 0x06007713 RID: 30483 RVA: 0x0029D135 File Offset: 0x0029B335
		public override IEnumerable<DiaOption> Choices
		{
			get
			{
				if (base.ArchivedOnly)
				{
					yield return base.Option_Close;
					yield break;
				}
				DiaOption diaOption = new DiaOption("RansomDemand_Accept".Translate());
				diaOption.action = delegate()
				{
					this.faction.kidnapped.RemoveKidnappedPawn(this.kidnapped);
					Find.WorldPawns.RemovePawn(this.kidnapped);
					IntVec3 intVec;
					if (this.faction.def.techLevel < TechLevel.Industrial)
					{
						if (!CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map) && this.map.reachability.CanReachColony(c), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec) && !CellFinder.TryFindRandomEdgeCellWith((IntVec3 c) => c.Standable(this.map), this.map, CellFinder.EdgeRoadChance_Friendly, out intVec))
						{
							Log.Warning("Could not find any edge cell.");
							intVec = DropCellFinder.TradeDropSpot(this.map);
						}
						GenSpawn.Spawn(this.kidnapped, intVec, this.map, WipeMode.Vanish);
					}
					else
					{
						intVec = DropCellFinder.TradeDropSpot(this.map);
						TradeUtility.SpawnDropPod(intVec, this.map, this.kidnapped);
					}
					CameraJumper.TryJump(intVec, this.map);
					TradeUtility.LaunchSilver(this.map, this.fee);
					Find.LetterStack.RemoveLetter(this);
				};
				diaOption.resolveTree = true;
				if (!TradeUtility.ColonyHasEnoughSilver(this.map, this.fee))
				{
					diaOption.Disable("NeedSilverLaunchable".Translate(this.fee.ToString()));
				}
				yield return diaOption;
				yield return base.Option_Reject;
				yield return base.Option_Postpone;
				yield break;
			}
		}

		// Token: 0x170014D9 RID: 5337
		// (get) Token: 0x06007714 RID: 30484 RVA: 0x0029D145 File Offset: 0x0029B345
		public override bool CanShowInLetterStack
		{
			get
			{
				return base.CanShowInLetterStack && Find.Maps.Contains(this.map) && this.faction.kidnapped.KidnappedPawnsListForReading.Contains(this.kidnapped);
			}
		}

		// Token: 0x06007715 RID: 30485 RVA: 0x0029D180 File Offset: 0x0029B380
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Map>(ref this.map, "map", false);
			Scribe_References.Look<Faction>(ref this.faction, "faction", false);
			Scribe_References.Look<Pawn>(ref this.kidnapped, "kidnapped", false);
			Scribe_Values.Look<int>(ref this.fee, "fee", 0, false);
		}

		// Token: 0x0400422E RID: 16942
		public Map map;

		// Token: 0x0400422F RID: 16943
		public Faction faction;

		// Token: 0x04004230 RID: 16944
		public Pawn kidnapped;

		// Token: 0x04004231 RID: 16945
		public int fee;
	}
}
