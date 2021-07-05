using System;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB4 RID: 4020
	public abstract class CompRitualEffect_Constant : RitualVisualEffectComp
	{
		// Token: 0x06005EE9 RID: 24297 RVA: 0x00207DCF File Offset: 0x00205FCF
		public override void OnSetup(RitualVisualEffect parent, LordJob_Ritual ritual, bool loading)
		{
			base.OnSetup(parent, ritual, loading);
			this.Spawn(ritual);
		}

		// Token: 0x06005EEA RID: 24298 RVA: 0x00207DE4 File Offset: 0x00205FE4
		protected virtual void Spawn(LordJob_Ritual ritual)
		{
			Mote mote = this.SpawnMote(ritual, null);
			if (mote != null)
			{
				this.parent.AddMoteToMaintain(mote);
				if (this.props.colorOverride != null)
				{
					mote.instanceColor = this.props.colorOverride.Value;
				}
				else
				{
					mote.instanceColor = this.parent.def.tintColor;
				}
				this.spawned = true;
			}
		}

		// Token: 0x06005EEB RID: 24299 RVA: 0x00207E58 File Offset: 0x00206058
		public override void Tick()
		{
			base.Tick();
			if (!this.spawned)
			{
				this.Spawn(this.parent.ritual);
			}
		}

		// Token: 0x040036B5 RID: 14005
		protected bool spawned;
	}
}
