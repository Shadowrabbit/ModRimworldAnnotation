using System;
using RimWorld;

namespace Verse
{
	// Token: 0x02000529 RID: 1321
	public class CompLifespan : ThingComp
	{
		// Token: 0x1700066D RID: 1645
		// (get) Token: 0x060021DF RID: 8671 RVA: 0x0001D507 File Offset: 0x0001B707
		public CompProperties_Lifespan Props
		{
			get
			{
				return (CompProperties_Lifespan)this.props;
			}
		}

		// Token: 0x060021E0 RID: 8672 RVA: 0x0001D514 File Offset: 0x0001B714
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<int>(ref this.age, "age", 0, false);
		}

		// Token: 0x060021E1 RID: 8673 RVA: 0x0001D52E File Offset: 0x0001B72E
		public override void CompTick()
		{
			this.age++;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x060021E2 RID: 8674 RVA: 0x0001D557 File Offset: 0x0001B757
		public override void CompTickRare()
		{
			this.age += 250;
			if (this.age >= this.Props.lifespanTicks)
			{
				this.Expire();
			}
		}

		// Token: 0x060021E3 RID: 8675 RVA: 0x00107C14 File Offset: 0x00105E14
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			string result = "";
			int num = this.Props.lifespanTicks - this.age;
			if (num > 0)
			{
				result = "LifespanExpiry".Translate() + " " + num.ToStringTicksToPeriod(true, false, true, true);
				if (!text.NullOrEmpty())
				{
					result = "\n" + text;
				}
			}
			return result;
		}

		// Token: 0x060021E4 RID: 8676 RVA: 0x00107C84 File Offset: 0x00105E84
		protected void Expire()
		{
			if (this.Props.expireEffect != null)
			{
				this.Props.expireEffect.Spawn(this.parent.Position, this.parent.Map, 1f).Cleanup();
			}
			this.parent.Destroy(DestroyMode.KillFinalize);
		}

		// Token: 0x040016FF RID: 5887
		public int age = -1;
	}
}
