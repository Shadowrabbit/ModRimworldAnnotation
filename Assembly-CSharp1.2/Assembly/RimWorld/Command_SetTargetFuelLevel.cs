﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200181E RID: 6174
	[StaticConstructorOnStartup]
	public class Command_SetTargetFuelLevel : Command
	{
		// Token: 0x060088BD RID: 35005 RVA: 0x00280120 File Offset: 0x0027E320
		public override void ProcessInput(Event ev)
		{
			base.ProcessInput(ev);
			if (this.refuelables == null)
			{
				this.refuelables = new List<CompRefuelable>();
			}
			if (!this.refuelables.Contains(this.refuelable))
			{
				this.refuelables.Add(this.refuelable);
			}
			int num = int.MaxValue;
			for (int i = 0; i < this.refuelables.Count; i++)
			{
				if ((int)this.refuelables[i].Props.fuelCapacity < num)
				{
					num = (int)this.refuelables[i].Props.fuelCapacity;
				}
			}
			int startingValue = num / 2;
			for (int j = 0; j < this.refuelables.Count; j++)
			{
				if ((int)this.refuelables[j].TargetFuelLevel <= num)
				{
					startingValue = (int)this.refuelables[j].TargetFuelLevel;
					break;
				}
			}
			Func<int, string> textGetter;
			if (this.refuelable.parent.def.building.hasFuelingPort)
			{
				textGetter = ((int x) => "SetPodLauncherTargetFuelLevel".Translate(x, CompLaunchable.MaxLaunchDistanceAtFuelLevel((float)x)));
			}
			else
			{
				textGetter = ((int x) => "SetTargetFuelLevel".Translate(x));
			}
			Dialog_Slider window = new Dialog_Slider(textGetter, 0, num, delegate(int value)
			{
				for (int k = 0; k < this.refuelables.Count; k++)
				{
					this.refuelables[k].TargetFuelLevel = (float)value;
				}
			}, startingValue);
			Find.WindowStack.Add(window);
		}

		// Token: 0x060088BE RID: 35006 RVA: 0x0005BD22 File Offset: 0x00059F22
		public override bool InheritInteractionsFrom(Gizmo other)
		{
			if (this.refuelables == null)
			{
				this.refuelables = new List<CompRefuelable>();
			}
			this.refuelables.Add(((Command_SetTargetFuelLevel)other).refuelable);
			return false;
		}

		// Token: 0x040057CB RID: 22475
		public CompRefuelable refuelable;

		// Token: 0x040057CC RID: 22476
		private List<CompRefuelable> refuelables;
	}
}
