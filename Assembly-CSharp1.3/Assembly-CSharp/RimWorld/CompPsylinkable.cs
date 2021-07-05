using System;
using System.Collections.Generic;
using System.Linq;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02001176 RID: 4470
	public class CompPsylinkable : ThingComp
	{
		// Token: 0x1700127D RID: 4733
		// (get) Token: 0x06006B66 RID: 27494 RVA: 0x00240F11 File Offset: 0x0023F111
		public CompProperties_Psylinkable Props
		{
			get
			{
				return (CompProperties_Psylinkable)this.props;
			}
		}

		// Token: 0x1700127E RID: 4734
		// (get) Token: 0x06006B67 RID: 27495 RVA: 0x00240F1E File Offset: 0x0023F11E
		public CompSpawnSubplant CompSubplant
		{
			get
			{
				return this.parent.TryGetComp<CompSpawnSubplant>();
			}
		}

		// Token: 0x06006B68 RID: 27496 RVA: 0x00240F2C File Offset: 0x0023F12C
		private IEnumerable<Pawn> GetPawnsThatCanPsylink(int level = -1)
		{
			return from p in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive_Colonists
			where this.Props.requiredFocus.CanPawnUse(p) && this.GetRequiredPlantCount(p) <= this.CompSubplant.SubplantsForReading.Count && (level == -1 || p.GetPsylinkLevel() == level)
			select p;
		}

		// Token: 0x06006B69 RID: 27497 RVA: 0x00240F63 File Offset: 0x0023F163
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			this.CompSubplant.onGrassGrown = new Action(this.OnGrassGrown);
		}

		// Token: 0x06006B6A RID: 27498 RVA: 0x00240F7C File Offset: 0x0023F17C
		private void OnGrassGrown()
		{
			bool flag = false;
			foreach (Pawn item in this.GetPawnsThatCanPsylink(-1))
			{
				if (!this.pawnsThatCanPsylinkLastGrassGrow.Contains(item))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
				string text = "";
				for (int i = 0; i < this.Props.requiredSubplantCountPerPsylinkLevel.Count; i++)
				{
					IEnumerable<string> enumerable = from p in this.GetPawnsThatCanPsylink(i)
					select p.LabelShort;
					if (enumerable.Count<string>() > 0)
					{
						text = string.Concat(new object[]
						{
							text,
							"- " + "Level".Translate().CapitalizeFirst() + " ",
							i + 1,
							": ",
							this.Props.requiredSubplantCountPerPsylinkLevel[i],
							" ",
							compSpawnSubplant.Props.subplant.label,
							" (",
							enumerable.ToCommaList(false, false),
							")\n"
						});
					}
				}
				Find.LetterStack.ReceiveLetter(this.Props.enoughPlantsLetterLabel, this.Props.enoughPlantsLetterText.Formatted(compSpawnSubplant.SubplantsForReading.Count, text.TrimEndNewlines()), LetterDefOf.NeutralEvent, new LookTargets(this.GetPawnsThatCanPsylink(-1)), null, null, null, null);
			}
			this.pawnsThatCanPsylinkLastGrassGrow.Clear();
			this.pawnsThatCanPsylinkLastGrassGrow.AddRange(this.GetPawnsThatCanPsylink(-1));
		}

		// Token: 0x06006B6B RID: 27499 RVA: 0x00241170 File Offset: 0x0023F370
		public int GetRequiredPlantCount(Pawn pawn)
		{
			int psylinkLevel = pawn.GetPsylinkLevel();
			if (this.parent.TryGetComp<CompSpawnSubplant>() == null)
			{
				Log.Warning("CompPsylinkable with requiredSubplantCountPerPsylinkLevel set on a Thing without CompSpawnSubplant!");
				return -1;
			}
			int result;
			if (this.Props.requiredSubplantCountPerPsylinkLevel.Count <= psylinkLevel)
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel.Last<int>();
			}
			else
			{
				result = this.Props.requiredSubplantCountPerPsylinkLevel[psylinkLevel];
			}
			return result;
		}

		// Token: 0x06006B6C RID: 27500 RVA: 0x002411D8 File Offset: 0x0023F3D8
		public AcceptanceReport CanPsylink(Pawn pawn, LocalTargetInfo? knownSpot = null)
		{
			if (pawn.Dead || pawn.Faction != Faction.OfPlayer)
			{
				return false;
			}
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int requiredPlantCount = this.GetRequiredPlantCount(pawn);
			if (requiredPlantCount == -1)
			{
				return false;
			}
			if (!this.Props.requiredFocus.CanPawnUse(pawn))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedFocus".Translate(this.Props.requiredFocus.label));
			}
			if (pawn.GetPsylinkLevel() >= pawn.GetMaxPsylinkLevel())
			{
				return new AcceptanceReport("BeginLinkingRitualMaxPsylinkLevel".Translate());
			}
			if (!pawn.Map.reservationManager.CanReserve(pawn, this.parent, 1, -1, null, false))
			{
				Pawn pawn2 = pawn.Map.reservationManager.FirstRespectedReserver(this.parent, pawn);
				return new AcceptanceReport((pawn2 == null) ? "Reserved".Translate() : "ReservedBy".Translate(pawn.LabelShort, pawn2));
			}
			if (compSpawnSubplant.SubplantsForReading.Count < requiredPlantCount)
			{
				return new AcceptanceReport("BeginLinkingRitualNeedSubplants".Translate(requiredPlantCount.ToString(), compSpawnSubplant.Props.subplant.label, compSpawnSubplant.SubplantsForReading.Count.ToString()));
			}
			LocalTargetInfo localTargetInfo;
			if (knownSpot != null)
			{
				if (!this.CanUseSpot(pawn, knownSpot.Value))
				{
					return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
				}
			}
			else if (!this.TryFindLinkSpot(pawn, out localTargetInfo))
			{
				return new AcceptanceReport("BeginLinkingRitualNeedLinkSpot".Translate());
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06006B6D RID: 27501 RVA: 0x002413A4 File Offset: 0x0023F5A4
		public bool TryFindLinkSpot(Pawn pawn, out LocalTargetInfo spot)
		{
			spot = MeditationUtility.FindMeditationSpot(pawn).spot;
			if (this.CanUseSpot(pawn, spot))
			{
				return true;
			}
			int num = GenRadial.NumCellsInRadius(2.9f);
			int num2 = GenRadial.NumCellsInRadius(3.9f);
			for (int i = num; i < num2; i++)
			{
				IntVec3 c = this.parent.Position + GenRadial.RadialPattern[i];
				if (this.CanUseSpot(pawn, c))
				{
					spot = c;
					return true;
				}
			}
			spot = IntVec3.Zero;
			return false;
		}

		// Token: 0x06006B6E RID: 27502 RVA: 0x00241440 File Offset: 0x0023F640
		private bool CanUseSpot(Pawn pawn, LocalTargetInfo spot)
		{
			IntVec3 cell = spot.Cell;
			return cell.DistanceTo(this.parent.Position) <= 3.9f && cell.Standable(this.parent.Map) && GenSight.LineOfSight(cell, this.parent.Position, this.parent.Map, false, null, 0, 0) && pawn.CanReach(spot, PathEndMode.OnCell, Danger.Deadly, false, false, TraverseMode.ByPawn);
		}

		// Token: 0x06006B6F RID: 27503 RVA: 0x002414B9 File Offset: 0x0023F6B9
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn pawn)
		{
			if (pawn.Dead || pawn.Drafted)
			{
				yield break;
			}
			string text = "BeginLinkingRitualFloatMenu".Translate();
			AcceptanceReport acceptanceReport = this.CanPsylink(pawn, null);
			if (!acceptanceReport.Accepted && !string.IsNullOrWhiteSpace(acceptanceReport.Reason))
			{
				text = text + ": " + acceptanceReport.Reason;
			}
			yield return new FloatMenuOption(text, delegate()
			{
				Precept_Ritual precept_Ritual = null;
				for (int i = 0; i < pawn.Ideo.PreceptsListForReading.Count; i++)
				{
					if (pawn.Ideo.PreceptsListForReading[i].def == PreceptDefOf.AnimaTreeLinking)
					{
						precept_Ritual = (Precept_Ritual)pawn.Ideo.PreceptsListForReading[i];
						break;
					}
				}
				if (precept_Ritual != null)
				{
					Find.WindowStack.Add(precept_Ritual.GetRitualBeginWindow(this.parent, null, null, pawn, null, null));
				}
			}, MenuOptionPriority.Default, null, null, 0f, null, null, true, 0)
			{
				Disabled = !acceptanceReport.Accepted
			};
			yield break;
		}

		// Token: 0x06006B70 RID: 27504 RVA: 0x002414D0 File Offset: 0x0023F6D0
		public void FinishLinkingRitual(Pawn pawn, int plantsToKeep)
		{
			if (!ModLister.CheckRoyalty("Psylinkable"))
			{
				return;
			}
			FleckMaker.Static(this.parent.Position, pawn.Map, FleckDefOf.PsycastAreaEffect, 10f);
			SoundDefOf.PsycastPsychicPulse.PlayOneShot(new TargetInfo(this.parent));
			CompSpawnSubplant compSpawnSubplant = this.parent.TryGetComp<CompSpawnSubplant>();
			int num = this.GetRequiredPlantCount(pawn) - plantsToKeep;
			List<Thing> list = (from p in compSpawnSubplant.SubplantsForReading
			orderby p.Position.DistanceTo(this.parent.Position) descending
			select p).ToList<Thing>();
			int num2 = 0;
			while (num2 < num && num2 < list.Count)
			{
				list[num2].Destroy(DestroyMode.Vanish);
				num2++;
			}
			compSpawnSubplant.Cleanup();
			pawn.ChangePsylinkLevel(1, true);
			Find.History.Notify_PsylinkAvailable();
		}

		// Token: 0x06006B71 RID: 27505 RVA: 0x00241594 File Offset: 0x0023F794
		public override void PostExposeData()
		{
			Scribe_Collections.Look<Pawn>(ref this.pawnsThatCanPsylinkLastGrassGrow, "pawnsThatCanPsylinkLastGrassGrow", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.pawnsThatCanPsylinkLastGrassGrow.RemoveAll((Pawn x) => x == null);
			}
		}

		// Token: 0x04003BC1 RID: 15297
		private List<Pawn> pawnsThatCanPsylinkLastGrassGrow = new List<Pawn>();

		// Token: 0x04003BC2 RID: 15298
		public const float MaxDistance = 3.9f;
	}
}
