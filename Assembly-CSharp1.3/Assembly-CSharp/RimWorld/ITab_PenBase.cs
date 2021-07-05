using System;
using Verse;

namespace RimWorld
{
	// Token: 0x0200134F RID: 4943
	public abstract class ITab_PenBase : ITab
	{
		// Token: 0x17001507 RID: 5383
		// (get) Token: 0x060077AA RID: 30634 RVA: 0x002A261B File Offset: 0x002A081B
		public CompAnimalPenMarker SelectedCompAnimalPenMarker
		{
			get
			{
				Thing selThing = base.SelThing;
				if (selThing == null)
				{
					return null;
				}
				return selThing.TryGetComp<CompAnimalPenMarker>();
			}
		}

		// Token: 0x17001508 RID: 5384
		// (get) Token: 0x060077AB RID: 30635 RVA: 0x002A262E File Offset: 0x002A082E
		public override bool IsVisible
		{
			get
			{
				return this.SelectedCompAnimalPenMarker != null;
			}
		}
	}
}
