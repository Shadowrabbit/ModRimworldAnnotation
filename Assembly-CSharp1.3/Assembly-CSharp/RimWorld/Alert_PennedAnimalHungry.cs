using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02001285 RID: 4741
	public class Alert_PennedAnimalHungry : Alert
	{
		// Token: 0x170013C4 RID: 5060
		// (get) Token: 0x06007145 RID: 28997 RVA: 0x0025C07A File Offset: 0x0025A27A
		private List<GlobalTargetInfo> Targets
		{
			get
			{
				this.CalculateTargets();
				return this.targets;
			}
		}

		// Token: 0x06007146 RID: 28998 RVA: 0x0025C088 File Offset: 0x0025A288
		public Alert_PennedAnimalHungry()
		{
			this.defaultLabel = "AlertPennedAnimalHungry".Translate();
		}

		// Token: 0x06007147 RID: 28999 RVA: 0x0025C0BC File Offset: 0x0025A2BC
		private void CalculateTargets()
		{
			this.targets.Clear();
			this.pawnNames.Clear();
			foreach (Pawn pawn in PawnsFinder.AllMaps_SpawnedPawnsInFaction(Faction.OfPlayer))
			{
				if (AnimalPenUtility.NeedsToBeManagedByRope(pawn) && pawn.needs.food.TicksStarving > 2500 && AnimalPenUtility.GetCurrentPenOf(pawn, false) != null)
				{
					this.targets.Add(pawn);
					this.pawnNames.Add(pawn.NameShortColored.Resolve());
				}
			}
		}

		// Token: 0x06007148 RID: 29000 RVA: 0x0025C174 File Offset: 0x0025A374
		public override TaggedString GetExplanation()
		{
			return "AlertPennedAnimalHungryDesc".Translate(this.pawnNames.ToLineList("  - "));
		}

		// Token: 0x06007149 RID: 29001 RVA: 0x0025C195 File Offset: 0x0025A395
		public override AlertReport GetReport()
		{
			return AlertReport.CulpritsAre(this.Targets);
		}

		// Token: 0x04003E4C RID: 15948
		private List<GlobalTargetInfo> targets = new List<GlobalTargetInfo>();

		// Token: 0x04003E4D RID: 15949
		private List<string> pawnNames = new List<string>();
	}
}
