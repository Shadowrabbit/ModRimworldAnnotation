using System;
using System.Collections.Generic;

namespace Verse
{
	// Token: 0x02000196 RID: 406
	public sealed class FleckManager : IExposable
	{
		// Token: 0x06000B75 RID: 2933 RVA: 0x0003E2D4 File Offset: 0x0003C4D4
		public FleckManager()
		{
			foreach (FleckDef fleckDef in DefDatabase<FleckDef>.AllDefsListForReading)
			{
				FleckSystem fleckSystem;
				if (!this.systems.TryGetValue(fleckDef.fleckSystemClass, out fleckSystem))
				{
					fleckSystem = (FleckSystem)Activator.CreateInstance(fleckDef.fleckSystemClass);
					fleckSystem.parent = this;
					this.systems.Add(fleckDef.fleckSystemClass, fleckSystem);
				}
				fleckSystem.handledDefs.Add(fleckDef);
			}
		}

		// Token: 0x06000B76 RID: 2934 RVA: 0x0003E388 File Offset: 0x0003C588
		public FleckManager(Map parent) : this()
		{
			this.parent = parent;
		}

		// Token: 0x06000B77 RID: 2935 RVA: 0x0003E398 File Offset: 0x0003C598
		public void CreateFleck(FleckCreationData fleckData)
		{
			FleckSystem fleckSystem;
			if (!this.systems.TryGetValue(fleckData.def.fleckSystemClass, out fleckSystem))
			{
				throw new Exception("No system to handle MoteDef " + fleckData.def + " found!?");
			}
			fleckData.spawnPosition.y = fleckData.def.altitudeLayer.AltitudeFor(fleckData.def.altitudeLayerIncOffset);
			fleckSystem.CreateFleck(fleckData);
		}

		// Token: 0x06000B78 RID: 2936 RVA: 0x0003E408 File Offset: 0x0003C608
		public void FleckManagerUpdate()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Update();
			}
		}

		// Token: 0x06000B79 RID: 2937 RVA: 0x0003E460 File Offset: 0x0003C660
		public void FleckManagerTick()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.Tick();
			}
		}

		// Token: 0x06000B7A RID: 2938 RVA: 0x0003E4B8 File Offset: 0x0003C6B8
		public void FleckManagerDraw()
		{
			try
			{
				foreach (FleckSystem fleckSystem in this.systems.Values)
				{
					fleckSystem.Draw(this.drawBatch);
				}
			}
			finally
			{
				this.drawBatch.Flush(true);
			}
		}

		// Token: 0x06000B7B RID: 2939 RVA: 0x0003E52C File Offset: 0x0003C72C
		public void FleckManagerOnGUI()
		{
			foreach (FleckSystem fleckSystem in this.systems.Values)
			{
				fleckSystem.OnGUI();
			}
		}

		// Token: 0x06000B7C RID: 2940 RVA: 0x0000313F File Offset: 0x0000133F
		public void ExposeData()
		{
		}

		// Token: 0x0400098E RID: 2446
		public readonly Map parent;

		// Token: 0x0400098F RID: 2447
		private Dictionary<Type, FleckSystem> systems = new Dictionary<Type, FleckSystem>();

		// Token: 0x04000990 RID: 2448
		private DrawBatch drawBatch = new DrawBatch();
	}
}
