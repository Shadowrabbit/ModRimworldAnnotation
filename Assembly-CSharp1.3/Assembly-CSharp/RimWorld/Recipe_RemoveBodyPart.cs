using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D9C RID: 3484
	public class Recipe_RemoveBodyPart : Recipe_Surgery
	{
		// Token: 0x17000DFF RID: 3583
		// (get) Token: 0x060050C8 RID: 20680 RVA: 0x000126F5 File Offset: 0x000108F5
		protected virtual bool SpawnPartsWhenRemoved
		{
			get
			{
				return true;
			}
		}

		// Token: 0x060050C9 RID: 20681 RVA: 0x001B07B1 File Offset: 0x001AE9B1
		public override IEnumerable<BodyPartRecord> GetPartsToApplyOn(Pawn pawn, RecipeDef recipe)
		{
			IEnumerable<BodyPartRecord> notMissingParts = pawn.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			using (IEnumerator<BodyPartRecord> enumerator = notMissingParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					BodyPartRecord part = enumerator.Current;
					if (pawn.health.hediffSet.HasDirectlyAddedPartFor(part))
					{
						yield return part;
					}
					else if (MedicalRecipesUtility.IsCleanAndDroppable(pawn, part))
					{
						yield return part;
					}
					else if (part != pawn.RaceProps.body.corePart && part.def.canSuggestAmputation && pawn.health.hediffSet.hediffs.Any((Hediff d) => !(d is Hediff_Injury) && d.def.isBad && d.Visible && d.Part == part))
					{
						yield return part;
					}
				}
			}
			IEnumerator<BodyPartRecord> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060050CA RID: 20682 RVA: 0x001B07C1 File Offset: 0x001AE9C1
		public override bool IsViolationOnPawn(Pawn pawn, BodyPartRecord part, Faction billDoerFaction)
		{
			return ((pawn.Faction != billDoerFaction && pawn.Faction != null) || pawn.IsQuestLodger()) && HealthUtility.PartRemovalIntent(pawn, part) == BodyPartRemovalIntent.Harvest;
		}

		// Token: 0x060050CB RID: 20683 RVA: 0x001B07EC File Offset: 0x001AE9EC
		public override void ApplyOnPawn(Pawn pawn, BodyPartRecord part, Pawn billDoer, List<Thing> ingredients, Bill bill)
		{
			bool flag = MedicalRecipesUtility.IsClean(pawn, part);
			bool flag2 = this.IsViolationOnPawn(pawn, part, Faction.OfPlayer);
			if (billDoer != null)
			{
				if (base.CheckSurgeryFail(billDoer, pawn, ingredients, part, bill))
				{
					return;
				}
				TaleRecorder.RecordTale(TaleDefOf.DidSurgery, new object[]
				{
					billDoer,
					pawn
				});
				if (this.SpawnPartsWhenRemoved)
				{
					MedicalRecipesUtility.SpawnNaturalPartIfClean(pawn, part, billDoer.Position, billDoer.Map);
					MedicalRecipesUtility.SpawnThingsFromHediffs(pawn, part, billDoer.Position, billDoer.Map);
				}
			}
			this.DamagePart(pawn, part);
			if (flag)
			{
				this.ApplyThoughts(pawn, billDoer);
			}
			if (flag2)
			{
				base.ReportViolation(pawn, billDoer, pawn.HomeFaction, -70);
			}
		}

		// Token: 0x060050CC RID: 20684 RVA: 0x001B0890 File Offset: 0x001AEA90
		public virtual void DamagePart(Pawn pawn, BodyPartRecord part)
		{
			pawn.TakeDamage(new DamageInfo(DamageDefOf.SurgicalCut, 99999f, 999f, -1f, null, part, null, DamageInfo.SourceCategory.ThingOrUnknown, null, true, true));
		}

		// Token: 0x060050CD RID: 20685 RVA: 0x001B08C4 File Offset: 0x001AEAC4
		public virtual void ApplyThoughts(Pawn pawn, Pawn billDoer)
		{
			if (pawn.Dead)
			{
				ThoughtUtility.GiveThoughtsForPawnExecuted(pawn, billDoer, PawnExecutionKind.OrganHarvesting);
				return;
			}
			ThoughtUtility.GiveThoughtsForPawnOrganHarvested(pawn, billDoer);
		}

		// Token: 0x060050CE RID: 20686 RVA: 0x001B08E0 File Offset: 0x001AEAE0
		public override string GetLabelWhenUsedOn(Pawn pawn, BodyPartRecord part)
		{
			if (pawn.RaceProps.IsMechanoid || pawn.health.hediffSet.PartOrAnyAncestorHasDirectlyAddedParts(part))
			{
				return RecipeDefOf.RemoveBodyPart.label;
			}
			BodyPartRemovalIntent bodyPartRemovalIntent = HealthUtility.PartRemovalIntent(pawn, part);
			if (bodyPartRemovalIntent == BodyPartRemovalIntent.Harvest)
			{
				return "HarvestOrgan".Translate();
			}
			if (bodyPartRemovalIntent != BodyPartRemovalIntent.Amputate)
			{
				throw new InvalidOperationException();
			}
			if (part.depth == BodyPartDepth.Inside || part.def.socketed)
			{
				return "RemoveOrgan".Translate();
			}
			return "Amputate".Translate();
		}
	}
}
