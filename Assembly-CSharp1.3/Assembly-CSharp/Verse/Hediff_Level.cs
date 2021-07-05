using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020002CB RID: 715
	public class Hediff_Level : Hediff
	{
		// Token: 0x170003E1 RID: 993
		// (get) Token: 0x06001356 RID: 4950 RVA: 0x0006DB34 File Offset: 0x0006BD34
		public override string Label
		{
			get
			{
				return this.def.label + " (" + "Level".Translate() + " " + this.level + ")";
			}
		}

		// Token: 0x170003E2 RID: 994
		// (get) Token: 0x06001357 RID: 4951 RVA: 0x0006DB89 File Offset: 0x0006BD89
		public override bool ShouldRemove
		{
			get
			{
				return this.level == 0;
			}
		}

		// Token: 0x06001358 RID: 4952 RVA: 0x0006DB94 File Offset: 0x0006BD94
		public override void PostAdd(DamageInfo? dinfo)
		{
			base.PostAdd(dinfo);
			if (base.Part == null)
			{
				Log.Error(this.def.defName + " has null Part. It should be set before PostAdd.");
			}
		}

		// Token: 0x06001359 RID: 4953 RVA: 0x0006DBBF File Offset: 0x0006BDBF
		public override void Tick()
		{
			base.Tick();
			this.Severity = (float)this.level;
		}

		// Token: 0x0600135A RID: 4954 RVA: 0x0006DBD4 File Offset: 0x0006BDD4
		public virtual void ChangeLevel(int levelOffset)
		{
			this.level = (int)Mathf.Clamp((float)(this.level + levelOffset), this.def.minSeverity, this.def.maxSeverity);
		}

		// Token: 0x0600135B RID: 4955 RVA: 0x0006DC01 File Offset: 0x0006BE01
		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != this.level)
			{
				this.ChangeLevel(targetLevel - this.level);
			}
		}

		// Token: 0x0600135C RID: 4956 RVA: 0x0006DC1C File Offset: 0x0006BE1C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && base.Part == null)
			{
				Log.Error(base.GetType().Name + " has null part after loading.");
				this.pawn.health.hediffSet.hediffs.Remove(this);
			}
		}

		// Token: 0x04000E53 RID: 3667
		public int level = 1;
	}
}
