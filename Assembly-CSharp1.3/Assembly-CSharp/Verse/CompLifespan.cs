using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000388 RID: 904
	public class CompLifespan : ThingComp
	{
		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001A79 RID: 6777 RVA: 0x00099AF4 File Offset: 0x00097CF4
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x06001A7A RID: 6778 RVA: 0x00099B01 File Offset: 0x00097D01
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x06001A7B RID: 6779 RVA: 0x00099B1B File Offset: 0x00097D1B
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06001A7C RID: 6780 RVA: 0x00099B44 File Offset: 0x00097D44
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x06001A7D RID: 6781 RVA: 0x00099B74 File Offset: 0x00097D74
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			string result = "";
			int num = this.Props.lifespanTicks - this.age;
			if (num > 0)
			{
				result = "LifespanExpiry".Translate() + " " + num.ToStringTicksToPeriod(true, false, true, true).Colorize(ColoredText.DateTimeColor);
				if (!text.NullOrEmpty())
				{
					result = "\n" + text;
				}
			}
			return result;
		}

		// Token: 0x06001A7E RID: 6782 RVA: 0x00099BF0 File Offset: 0x00097DF0
		protected void Expire()
		{
			if (!this.parent.Spawned)
			{
				return;
			}
			if (this.Props.expireEffect != null)
			{
				this.Props.expireEffect.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			}
			if (this.Props.plantDefToSpawn != null && this.Props.plantDefToSpawn.CanNowPlantAt(this.parent.Position, this.parent.Map, false))
			{
				GenSpawn.Spawn(this.Props.plantDefToSpawn, this.parent.Position, this.parent.Map, WipeMode.Vanish);
			}
			this.parent.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x04001137 RID: 4407
		public int age = -1;
	}
}
