using System;
using System.Text;
using RimWorld;

namespace Verse
{
	// Token: 0x020002C6 RID: 710
	public class Hediff_AddedPart : Hediff_Implant
	{
		// Token: 0x170003D4 RID: 980
		// (get) Token: 0x06001338 RID: 4920 RVA: 0x0006D2F0 File Offset: 0x0006B4F0
		public override string TipStringExtra
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.TipStringExtra);
				stringBuilder.AppendLine("Efficiency".Translate() + ": " + this.def.addedPartProps.partEfficiency.ToStringPercent());
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06001339 RID: 4921 RVA: 0x0006D350 File Offset: 0x0006B550
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			this.pawn.health.RestorePart(base.Part, this, false);
			for (int i = 0; i < base.Part.parts.Count; i++)
			{
				Hediff_MissingPart hediff_MissingPart = (Hediff_MissingPart)HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this.pawn, null);
				hediff_MissingPart.IsFresh = true;
				hediff_MissingPart.lastInjury = HediffDefOf.SurgicalCut;
				hediff_MissingPart.Part = base.Part.parts[i];
				this.pawn.health.hediffSet.AddDirect(hediff_MissingPart, null, null);
			}
		}
	}
}
