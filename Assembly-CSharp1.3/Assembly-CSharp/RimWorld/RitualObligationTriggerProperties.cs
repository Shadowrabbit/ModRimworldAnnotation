using System;

namespace RimWorld
{
	// Token: 0x02000F45 RID: 3909
	public abstract class RitualObligationTriggerProperties
	{
		// Token: 0x06005CE7 RID: 23783 RVA: 0x001FEF1F File Offset: 0x001FD11F
		public virtual RitualObligationTrigger GetInstance(Precept_Ritual parent)
		{
			RitualObligationTrigger ritualObligationTrigger = (RitualObligationTrigger)Activator.CreateInstance(this.triggerClass);
			ritualObligationTrigger.ritual = parent;
			return ritualObligationTrigger;
		}

		// Token: 0x040035DB RID: 13787
		public Type triggerClass;

		// Token: 0x040035DC RID: 13788
		public bool mustBePlayerIdeo;
	}
}
