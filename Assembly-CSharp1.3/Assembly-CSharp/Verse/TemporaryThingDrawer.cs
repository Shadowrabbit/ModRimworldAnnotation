using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000222 RID: 546
	public class TemporaryThingDrawer : IExposable
	{
		// Token: 0x06000F8A RID: 3978 RVA: 0x000584F0 File Offset: 0x000566F0
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

		// Token: 0x06000F8B RID: 3979 RVA: 0x00058550 File Offset: 0x00056750
		public void Draw()
		{
			foreach (TemporaryThingDrawable temporaryThingDrawable in this.drawables)
			{
				temporaryThingDrawable.thing.DrawAt(temporaryThingDrawable.position, false);
			}
		}

		// Token: 0x06000F8C RID: 3980 RVA: 0x000585B0 File Offset: 0x000567B0
		public void AddThing(Thing t, Vector3 position, int ticks)
		{
			this.drawables.Add(new TemporaryThingDrawable
			{
				thing = t,
				position = position,
				ticksLeft = ticks
			});
		}

		// Token: 0x06000F8D RID: 3981 RVA: 0x000585D7 File Offset: 0x000567D7
		public void ExposeData()
		{
			Scribe_Collections.Look<TemporaryThingDrawable>(ref this.drawables, "drawables", LookMode.Undefined, Array.Empty<object>());
		}

		// Token: 0x04000C43 RID: 3139
		private List<TemporaryThingDrawable> drawables = new List<TemporaryThingDrawable>();
	}
}
