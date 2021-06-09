using System;
using UnityEngine;

namespace Verse
{
	// Token: 0x020003B3 RID: 947
	public class Hediff_ImplantWithLevel : Hediff_Implant
	{
		// Token: 0x1700043F RID: 1087
		// (get) Token: 0x0600177E RID: 6014 RVA: 0x000DC724 File Offset: 0x000DA924
		public override string Label
		{
			get
			{
				return this.def.label + " (" + "Level".Translate().ToLower() + " " + this.level + ")";
			}
		}

		// Token: 0x17000440 RID: 1088
		// (get) Token: 0x0600177F RID: 6015 RVA: 0x00016896 File Offset: 0x00014A96
		public override bool ShouldRemove
		{
			get
			{
				return this.level == 0;
			}
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x000168A1 File Offset: 0x00014AA1
		public override void Tick()
		{
			base.Tick();
			this.Severity = (float)this.level;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x000168B6 File Offset: 0x00014AB6
		public virtual void ChangeLevel(int levelOffset)
		{
			this.level = (int)Mathf.Clamp((float)(this.level + levelOffset), this.def.minSeverity, this.def.maxSeverity);
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x000168E3 File Offset: 0x00014AE3
		public virtual void SetLevelTo(int targetLevel)
		{
			if (targetLevel != this.level)
			{
				this.ChangeLevel(targetLevel - this.level);
			}
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x000168FC File Offset: 0x00014AFC
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.level, "level", 0, false);
		}

		// Token: 0x04001214 RID: 4628
		public int level = 1;
	}
}
