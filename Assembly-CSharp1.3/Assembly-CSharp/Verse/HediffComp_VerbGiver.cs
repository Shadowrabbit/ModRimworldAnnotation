using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020002BC RID: 700
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x170003B9 RID: 953
		// (get) Token: 0x060012ED RID: 4845 RVA: 0x0006C385 File Offset: 0x0006A585
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x170003BA RID: 954
		// (get) Token: 0x060012EE RID: 4846 RVA: 0x0006C392 File Offset: 0x0006A592
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x170003BB RID: 955
		// (get) Token: 0x060012EF RID: 4847 RVA: 0x0006C39A File Offset: 0x0006A59A
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x170003BC RID: 956
		// (get) Token: 0x060012F0 RID: 4848 RVA: 0x0006C3A7 File Offset: 0x0006A5A7
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x170003BD RID: 957
		// (get) Token: 0x060012F1 RID: 4849 RVA: 0x0006C3B4 File Offset: 0x0006A5B4
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return base.Pawn;
			}
		}

		// Token: 0x170003BE RID: 958
		// (get) Token: 0x060012F2 RID: 4850 RVA: 0x0006C3BC File Offset: 0x0006A5BC
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Hediff;
			}
		}

		// Token: 0x060012F3 RID: 4851 RVA: 0x0006C3C3 File Offset: 0x0006A5C3
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x060012F4 RID: 4852 RVA: 0x0006C3D7 File Offset: 0x0006A5D7
		public override void CompExposeData()
		{
			base.CompExposeData();
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.verbTracker == null)
			{
				this.verbTracker = new VerbTracker(this);
			}
		}

		// Token: 0x060012F5 RID: 4853 RVA: 0x0006C415 File Offset: 0x0006A615
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060012F6 RID: 4854 RVA: 0x0006C429 File Offset: 0x0006A629
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID() + "_" + this.parent.comps.IndexOf(this);
		}

		// Token: 0x060012F7 RID: 4855 RVA: 0x0006C456 File Offset: 0x0006A656
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p.health.hediffSet.hediffs.Contains(this.parent);
		}

		// Token: 0x04000E41 RID: 3649
		public VerbTracker verbTracker;
	}
}
