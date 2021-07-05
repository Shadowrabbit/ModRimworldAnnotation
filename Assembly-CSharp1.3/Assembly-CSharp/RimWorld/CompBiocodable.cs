using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02001104 RID: 4356
	public class CompBiocodable : ThingComp
	{
		// Token: 0x170011DB RID: 4571
		// (get) Token: 0x06006891 RID: 26769 RVA: 0x00234EEC File Offset: 0x002330EC
		public bool Biocoded
		{
			get
			{
				return this.biocoded;
			}
		}

		// Token: 0x170011DC RID: 4572
		// (get) Token: 0x06006892 RID: 26770 RVA: 0x00234EF4 File Offset: 0x002330F4
		public Pawn CodedPawn
		{
			get
			{
				return this.codedPawn;
			}
		}

		// Token: 0x170011DD RID: 4573
		// (get) Token: 0x06006893 RID: 26771 RVA: 0x00234EFC File Offset: 0x002330FC
		public string CodedPawnLabel
		{
			get
			{
				return this.codedPawnLabel;
			}
		}

		// Token: 0x170011DE RID: 4574
		// (get) Token: 0x06006894 RID: 26772 RVA: 0x00234F04 File Offset: 0x00233104
		public CompProperties_Biocodable Props
		{
			get
			{
				return (CompProperties_Biocodable)this.props;
			}
		}

		// Token: 0x170011DF RID: 4575
		// (get) Token: 0x06006895 RID: 26773 RVA: 0x000126F5 File Offset: 0x000108F5
		public virtual bool Biocodable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x06006896 RID: 26774 RVA: 0x00234F14 File Offset: 0x00233114
		public static bool IsBiocoded(Thing thing)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.Biocoded;
		}

		// Token: 0x06006897 RID: 26775 RVA: 0x00234F34 File Offset: 0x00233134
		public static bool IsBiocodedFor(Thing thing, Pawn pawn)
		{
			CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
			return compBiocodable != null && compBiocodable.CodedPawn == pawn;
		}

		// Token: 0x06006898 RID: 26776 RVA: 0x00234F56 File Offset: 0x00233156
		public virtual void CodeFor(Pawn p)
		{
			if (!this.Biocodable)
			{
				return;
			}
			this.biocoded = true;
			this.codedPawn = p;
			this.codedPawnLabel = p.Name.ToStringFull;
			this.OnCodedFor(p);
		}

		// Token: 0x06006899 RID: 26777 RVA: 0x0000313F File Offset: 0x0000133F
		protected virtual void OnCodedFor(Pawn p)
		{
		}

		// Token: 0x0600689A RID: 26778 RVA: 0x00234F87 File Offset: 0x00233187
		public virtual void UnCode()
		{
			this.biocoded = false;
			this.codedPawn = null;
			this.codedPawnLabel = null;
		}

		// Token: 0x0600689B RID: 26779 RVA: 0x00234F9E File Offset: 0x0023319E
		public override void Notify_Equipped(Pawn pawn)
		{
			if (this.Biocodable && this.Props.biocodeOnEquip)
			{
				this.CodeFor(pawn);
			}
		}

		// Token: 0x0600689C RID: 26780 RVA: 0x00234FBC File Offset: 0x002331BC
		public override string TransformLabel(string label)
		{
			if (!this.biocoded)
			{
				return label;
			}
			return "Biocoded".Translate(label, this.parent.def).Resolve();
		}

		// Token: 0x0600689D RID: 26781 RVA: 0x00234FFC File Offset: 0x002331FC
		public override string CompInspectStringExtra()
		{
			if (!this.biocoded)
			{
				return string.Empty;
			}
			return "CodedFor".Translate(this.codedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
		}

		// Token: 0x0600689E RID: 26782 RVA: 0x0023503B File Offset: 0x0023323B
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			if (this.biocoded)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawn, "Stat_Thing_Biocoded_Name".Translate(), this.codedPawnLabel, "Stat_Thing_Biocoded_Desc".Translate(), 1104, null, null, false);
			}
			yield break;
		}

		// Token: 0x0600689F RID: 26783 RVA: 0x0023504B File Offset: 0x0023324B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.biocoded, "biocoded", false, false);
			Scribe_Values.Look<string>(ref this.codedPawnLabel, "biocodedPawnLabel", null, false);
			Scribe_References.Look<Pawn>(ref this.codedPawn, "codedPawn", true);
		}

		// Token: 0x04003A99 RID: 15001
		protected bool biocoded;

		// Token: 0x04003A9A RID: 15002
		protected string codedPawnLabel;

		// Token: 0x04003A9B RID: 15003
		protected Pawn codedPawn;
	}
}
