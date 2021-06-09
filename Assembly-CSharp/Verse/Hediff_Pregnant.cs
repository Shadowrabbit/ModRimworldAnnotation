using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B6 RID: 950
	public class Hediff_Pregnant : HediffWithComps
	{
		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x060017A7 RID: 6055 RVA: 0x00016A33 File Offset: 0x00014C33
		// (set) Token: 0x060017A8 RID: 6056 RVA: 0x00016A3B File Offset: 0x00014C3B
		public float GestationProgress
		{
			get
			{
				return this.Severity;
			}
			private set
			{
				this.Severity = value;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x060017A9 RID: 6057 RVA: 0x000DD12C File Offset: 0x000DB32C
		private bool IsSeverelyWounded
		{
			get
			{
				float num = 0f;
				List<Hediff> hediffs = this.pawn.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					if (hediffs[i] is Hediff_Injury && !hediffs[i].IsPermanent())
					{
						num += hediffs[i].Severity;
					}
				}
				List<Hediff_MissingPart> missingPartsCommonAncestors = this.pawn.health.hediffSet.GetMissingPartsCommonAncestors();
				for (int j = 0; j < missingPartsCommonAncestors.Count; j++)
				{
					if (missingPartsCommonAncestors[j].IsFreshNonSolidExtremity)
					{
						num += missingPartsCommonAncestors[j].Part.def.GetMaxHealth(this.pawn);
					}
				}
				return num > 38f * this.pawn.RaceProps.baseHealthScale;
			}
		}

		// Token: 0x060017AA RID: 6058 RVA: 0x000DD204 File Offset: 0x000DB404
		public override void Tick()
		{
			this.ageTicks++;
			if (this.pawn.IsHashIntervalTick(1000))
			{
				if (this.pawn.needs.food != null && this.pawn.needs.food.CurCategory == HungerCategory.Starving && this.pawn.health.hediffSet.HasHediff(HediffDefOf.Malnutrition, false) && this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false).Severity > 0.25f && Rand.MTBEventOccurs(0.5f, 60000f, 1000f))
				{
					if (this.Visible && PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						string value = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
						Messages.Message("MessageMiscarriedStarvation".Translate(value, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					}
					this.Miscarry();
					return;
				}
				if (this.IsSeverelyWounded && Rand.MTBEventOccurs(0.5f, 60000f, 1000f))
				{
					if (this.Visible && PawnUtility.ShouldSendNotificationAbout(this.pawn))
					{
						string value2 = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
						Messages.Message("MessageMiscarriedPoorHealth".Translate(value2, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
					}
					this.Miscarry();
					return;
				}
			}
			this.GestationProgress += 1f / (this.pawn.RaceProps.gestationPeriodDays * 60000f);
			if (this.GestationProgress >= 1f)
			{
				if (this.Visible && PawnUtility.ShouldSendNotificationAbout(this.pawn))
				{
					Messages.Message("MessageGaveBirth".Translate(this.pawn), this.pawn, MessageTypeDefOf.PositiveEvent, true);
				}
				Hediff_Pregnant.DoBirthSpawn(this.pawn, this.father);
				this.pawn.health.RemoveHediff(this);
			}
		}

		// Token: 0x060017AB RID: 6059 RVA: 0x00016A44 File Offset: 0x00014C44
		private void Miscarry()
		{
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x060017AC RID: 6060 RVA: 0x000DD4CC File Offset: 0x000DB6CC
		public static void DoBirthSpawn(Pawn mother, Pawn father)
		{
			int num = (mother.RaceProps.litterSizeCurve != null) ? Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve)) : 1;
			if (num < 1)
			{
				num = 1;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null);
			Pawn pawn = null;
			for (int i = 0; i < num; i++)
			{
				pawn = PawnGenerator.GeneratePawn(request);
				if (PawnUtility.TrySpawnHatchedOrBornPawn(pawn, mother))
				{
					if (pawn.playerSettings != null && mother.playerSettings != null)
					{
						pawn.playerSettings.AreaRestriction = mother.playerSettings.AreaRestriction;
					}
					if (pawn.RaceProps.IsFlesh)
					{
						pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, mother);
						if (father != null)
						{
							pawn.relations.AddDirectRelation(PawnRelationDefOf.Parent, father);
						}
					}
				}
				else
				{
					Find.WorldPawns.PassToWorld(pawn, PawnDiscardDecideMode.Discard);
				}
				TaleRecorder.RecordTale(TaleDefOf.GaveBirth, new object[]
				{
					mother,
					pawn
				});
			}
			if (mother.Spawned)
			{
				FilthMaker.TryMakeFilth(mother.Position, mother.Map, ThingDefOf.Filth_AmnioticFluid, mother.LabelIndefinite(), 5, FilthSourceFlags.None);
				if (mother.caller != null)
				{
					mother.caller.DoCall();
				}
				if (pawn.caller != null)
				{
					pawn.caller.DoCall();
				}
			}
		}

		// Token: 0x060017AD RID: 6061 RVA: 0x00016A57 File Offset: 0x00014C57
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", false);
		}

		// Token: 0x060017AE RID: 6062 RVA: 0x000DD664 File Offset: 0x000DB864
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1f - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod(true, false, true, true));
			return stringBuilder.ToString();
		}

		// Token: 0x04001218 RID: 4632
		public Pawn father;

		// Token: 0x04001219 RID: 4633
		private const int MiscarryCheckInterval = 1000;

		// Token: 0x0400121A RID: 4634
		private const float MTBMiscarryStarvingDays = 0.5f;

		// Token: 0x0400121B RID: 4635
		private const float MTBMiscarryWoundedDays = 0.5f;
	}
}
