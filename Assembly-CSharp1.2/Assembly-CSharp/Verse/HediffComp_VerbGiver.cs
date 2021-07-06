using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x020003F9 RID: 1017
	public class HediffComp_VerbGiver : HediffComp, IVerbOwner
	{
		// Token: 0x17000492 RID: 1170
		// (get) Token: 0x060018BF RID: 6335 RVA: 0x00017853 File Offset: 0x00015A53
		public HediffCompProperties_VerbGiver Props
		{
			get
			{
				return (HediffCompProperties_VerbGiver)this.props;
			}
		}

		// Token: 0x17000493 RID: 1171
		// (get) Token: 0x060018C0 RID: 6336 RVA: 0x00017860 File Offset: 0x00015A60
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		// Token: 0x17000494 RID: 1172
		// (get) Token: 0x060018C1 RID: 6337 RVA: 0x00017868 File Offset: 0x00015A68
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.Props.verbs;
			}
		}

		// Token: 0x17000495 RID: 1173
		// (get) Token: 0x060018C2 RID: 6338 RVA: 0x00017875 File Offset: 0x00015A75
		public List<Tool> Tools
		{
			get
			{
				return this.Props.tools;
			}
		}

		// Token: 0x17000496 RID: 1174
		// (get) Token: 0x060018C3 RID: 6339 RVA: 0x00017882 File Offset: 0x00015A82
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return base.Pawn;
			}
		}

		// Token: 0x17000497 RID: 1175
		// (get) Token: 0x060018C4 RID: 6340 RVA: 0x0001788A File Offset: 0x00015A8A
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Hediff;
			}
		}

		// Token: 0x060018C5 RID: 6341 RVA: 0x00017891 File Offset: 0x00015A91
		public HediffComp_VerbGiver()
		{
			this.verbTracker = new VerbTracker(this);
		}

		// Token: 0x060018C6 RID: 6342 RVA: 0x000178A5 File Offset: 0x00015AA5
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

		// Token: 0x060018C7 RID: 6343 RVA: 0x000178E3 File Offset: 0x00015AE3
		public override void CompPostTick(ref float severityAdjustment)
		{
			base.CompPostTick(ref severityAdjustment);
			this.verbTracker.VerbsTick();
		}

		// Token: 0x060018C8 RID: 6344 RVA: 0x000178F7 File Offset: 0x00015AF7
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return this.parent.GetUniqueLoadID() + "_" + this.parent.comps.IndexOf(this);
		}

		// Token: 0x060018C9 RID: 6345 RVA: 0x00017924 File Offset: 0x00015B24
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p.health.hediffSet.hediffs.Contains(this.parent);
		}

		// Token: 0x040012B1 RID: 4785
		public VerbTracker verbTracker;
	}
}
