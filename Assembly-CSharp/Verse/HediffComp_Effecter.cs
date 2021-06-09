using System;

namespace Verse
{
	// Token: 0x020003D5 RID: 981
	public class HediffComp_Effecter : HediffComp
	{
		// Token: 0x17000472 RID: 1138
		// (get) Token: 0x0600183A RID: 6202 RVA: 0x000170C9 File Offset: 0x000152C9
		public HediffCompProperties_Effecter Props
		{
			get
			{
				return (HediffCompProperties_Effecter)this.props;
			}
		}

		// Token: 0x0600183B RID: 6203 RVA: 0x000DEB78 File Offset: 0x000DCD78
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
