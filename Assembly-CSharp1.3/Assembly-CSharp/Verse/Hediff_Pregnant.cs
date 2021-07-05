using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002CD RID: 717
	public class Hediff_Pregnant : HediffWithComps
	{
		// Token: 0x170003ED RID: 1005
		// (get) Token: 0x0600136F RID: 4975 RVA: 0x0006E1C0 File Offset: 0x0006C3C0
		// (set) Token: 0x06001370 RID: 4976 RVA: 0x0006E1C8 File Offset: 0x0006C3C8
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

		// Token: 0x170003EE RID: 1006
		// (get) Token: 0x06001371 RID: 4977 RVA: 0x0006E1D4 File Offset: 0x0006C3D4
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

		// Token: 0x06001372 RID: 4978 RVA: 0x0006E2AC File Offset: 0x0006C4AC
		public override void Tick()
		{
			this.ageTicks++;
			if (this.pawn.IsHashIntervalTick(1000))
			{
				if (this.pawn.needs.food != null && this.pawn.needs.food.CurCategory == HungerCategory.Starving)
				{
					Hediff firstHediffOfDef = this.pawn.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Malnutrition, false);
					if (firstHediffOfDef != null && firstHediffOfDef.Severity > 0.1f && Rand.MTBEventOccurs(2f, 60000f, 1000f))
					{
						if (this.Visible && PawnUtility.ShouldSendNotificationAbout(this.pawn))
						{
							string value = this.pawn.Name.Numerical ? this.pawn.LabelShort : (this.pawn.LabelShort + " (" + this.pawn.kindDef.label + ")");
							Messages.Message("MessageMiscarriedStarvation".Translate(value, this.pawn), this.pawn, MessageTypeDefOf.NegativeHealthEvent, true);
						}
						this.Miscarry();
						return;
					}
				}
				if (this.IsSeverelyWounded && Rand.MTBEventOccurs(2f, 60000f, 1000f))
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
			float num = PawnUtility.BodyResourceGrowthSpeed(this.pawn) / (this.pawn.RaceProps.gestationPeriodDays * 60000f);
			this.GestationProgress += num;
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

		// Token: 0x06001373 RID: 4979 RVA: 0x0006E564 File Offset: 0x0006C764
		private void Miscarry()
		{
			this.pawn.health.RemoveHediff(this);
		}

		// Token: 0x06001374 RID: 4980 RVA: 0x0006E578 File Offset: 0x0006C778
		public static void DoBirthSpawn(Pawn mother, Pawn father)
		{
			int num = (mother.RaceProps.litterSizeCurve != null) ? Mathf.RoundToInt(Rand.ByCurve(mother.RaceProps.litterSizeCurve)) : 1;
			if (num < 1)
			{
				num = 1;
			}
			PawnGenerationRequest request = new PawnGenerationRequest(mother.kindDef, mother.Faction, PawnGenerationContext.NonPlayer, -1, false, true, false, false, true, false, 1f, false, true, true, true, false, false, false, false, 0f, 0f, null, 1f, null, null, null, null, null, null, null, null, null, null, null, null, null, false, false);
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

		// Token: 0x06001375 RID: 4981 RVA: 0x0006E715 File Offset: 0x0006C915
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_References.Look<Pawn>(ref this.father, "father", false);
		}

		// Token: 0x06001376 RID: 4982 RVA: 0x0006E730 File Offset: 0x0006C930
		public override string DebugString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.DebugString());
			stringBuilder.AppendLine("Gestation progress: " + this.GestationProgress.ToStringPercent());
			stringBuilder.AppendLine("Time left: " + ((int)((1f - this.GestationProgress) * this.pawn.RaceProps.gestationPeriodDays * 60000f)).ToStringTicksToPeriod(true, false, true, true));
			return stringBuilder.ToString();
		}

		// Token: 0x04000E56 RID: 3670
		public Pawn father;

		// Token: 0x04000E57 RID: 3671
		private const int MiscarryCheckInterval = 1000;

		// Token: 0x04000E58 RID: 3672
		private const float MTBMiscarryStarvingDays = 2f;

		// Token: 0x04000E59 RID: 3673
		private const float MTBMiscarryWoundedDays = 2f;

		// Token: 0x04000E5A RID: 3674
		private const float MalnutritionMinSeverityForMiscarry = 0.1f;
	}
}
