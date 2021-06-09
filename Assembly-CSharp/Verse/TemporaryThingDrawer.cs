using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000313 RID: 787
	public class TemporaryThingDrawer : IExposable
	{
		// Token: 0x06001410 RID: 5136 RVA: 0x000CD21C File Offset: 0x000CB41C
		public void Tick()
		{
			for (int i = this.drawables.Count - 1; i >= 0; i--)
			{
				TemporaryThingDrawable temporaryThingDrawable = this.drawables[i];
				if (temporaryThingDrawable.ticksLeft >= 0 && temporaryThingDrawable.thing != null)
				{
					temporaryThingDrawable.ticksLeft--;
				}
				else
				{
					this.drawables.RemoveAt(i);
				}
			}
		}

		// Token: 0x06001411 RID: 5137 RVA: 0x000CD27C File Offset: 0x000CB47C
		public void Draw()
		{
			foreach (TemporaryThingDrawable temporaryThingDrawable in this.drawables)
			{
				temporaryThingDrawable.thing.DrawAt(temporaryThingDrawable.position, false);
			}
		}

		// Token: 0x06001412 RID: 5138 RVA: 0x000146B0 File Offset: 0x000128B0
		public void AddThing(Thing t, Vector3 position, int ticks)
		{
			this.drawables.Add(new TemporaryThingDrawable
			{
				thing = t,
				position = position,
				ticksLeft = ticks
			});
		}

		// Token: 0x06001413 RID: 5139 RVA: 0x000146D7 File Offset: 0x000128D7
		public void ExposeData()
		{
			Scribe_Collections.Look<TemporaryThingDrawable>(ref this.drawables, "drawables", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04000FCE RID: 4046
		private List<TemporaryThingDrawable> drawables = new List<TemporaryThingDrawable>();
	}
}
