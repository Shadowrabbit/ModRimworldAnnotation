using System;

namespace Verse
{
	// Token: 0x02000292 RID: 658
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000395 RID: 917
		// (get) Token: 0x06001264 RID: 4708 RVA: 0x0006A281 File Offset: 0x00068481
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x06001265 RID: 4709 RVA: 0x0006A290 File Offset: 0x00068490
		public EffecterDef CurrentStateEffecter()
		{
			if (this.parent.CurStageIndex >= this.Props.severityIndices.min && (this.Props.severityIndices.max < 0 || this.parent.CurStageIndex <= this.Props.severityIndices.max))
			{
				return this.Props.stateEffecter;
			}
			return null;
		}
	}
}
