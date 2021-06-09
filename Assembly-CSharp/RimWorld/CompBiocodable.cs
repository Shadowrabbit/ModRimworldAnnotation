using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200179A RID: 6042
	public class CompBiocodable : ThingComp
	{
		// Token: 0x170014A6 RID: 5286
		// (get) Token: 0x06008581 RID: 34177 RVA: 0x00059824 File Offset: 0x00057A24
		public bool Biocoded
		{
			get
			{
				return this.biocoded;
			}
		}

		// Token: 0x170014A7 RID: 5287
		// (get) Token: 0x06008582 RID: 34178 RVA: 0x0005982C File Offset: 0x00057A2C
		public Pawn CodedPawn
		{
			get
			{
				return this.codedPawn;
			}
		}

		// Token: 0x170014A8 RID: 5288
		// (get) Token: 0x06008583 RID: 34179 RVA: 0x00059834 File Offset: 0x00057A34
		public string CodedPawnLabel
		{
			get
			{
				return this.codedPawnLabel;
			}
		}

		// Token: 0x06008584 RID: 34180 RVA: 0x0005983C File Offset: 0x00057A3C
		public void CodeFor(Pawn p)
		{
			this.biocoded = true;
			this.codedPawn = p;
			this.codedPawnLabel = p.Name.ToStringFull;
		}

		// Token: 0x06008585 RID: 34181 RVA: 0x002763DC File Offset: 0x002745DC
		public override string TransformLabel(string label)
		{
			if (!this.biocoded)
			{
				return label;
			}
			return "Biocoded".Translate(label, this.parent.def).Resolve();
		}

		// Token: 0x06008586 RID: 34182 RVA: 0x0027641C File Offset: 0x0027461C
		public override string CompInspectStringExtra()
		{
			if (!this.biocoded)
			{
				return string.Empty;
			}
			return "CodedFor".Translate(this.codedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
		}

		// Token: 0x06008587 RID: 34183 RVA: 0x0005985D File Offset: 0x00057A5D
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.biocoded, "biocoded", false, false);
			Scribe_Values.Look<string>(ref this.codedPawnLabel, "biocodedPawnLabel", null, false);
			Scribe_References.Look<Pawn>(ref this.codedPawn, "codedPawn", true);
		}

		// Token: 0x0400562C RID: 22060
		protected bool biocoded;

		// Token: 0x0400562D RID: 22061
		protected string codedPawnLabel;

		// Token: 0x0400562E RID: 22062
		protected Pawn codedPawn;
	}
}
