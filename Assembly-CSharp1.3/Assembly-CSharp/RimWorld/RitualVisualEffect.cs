using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000FB1 RID: 4017
	public class RitualVisualEffect : IExposable
	{
		// Token: 0x06005ED2 RID: 24274 RVA: 0x00207570 File Offset: 0x00205770
		public void Setup(LordJob_Ritual r, bool loading)
		{
			this.ritual = r;
			if (!loading)
			{
				using (List<CompProperties_RitualVisualEffect>.Enumerator enumerator = this.def.comps.GetEnumerator())
				{
					while (enumerator.MoveNext())
					{
						CompProperties_RitualVisualEffect compProperties_RitualVisualEffect = enumerator.Current;
						RitualVisualEffectComp instance = compProperties_RitualVisualEffect.GetInstance();
						instance.props = compProperties_RitualVisualEffect;
						instance.OnSetup(this, this.ritual, false);
						this.comps.Add(instance);
					}
					return;
				}
			}
			for (int i = 0; i < this.comps.Count; i++)
			{
				RitualVisualEffectComp ritualVisualEffectComp = this.comps[i];
				ritualVisualEffectComp.props = this.def.comps[i];
				ritualVisualEffectComp.OnSetup(this, this.ritual, true);
			}
		}

		// Token: 0x06005ED3 RID: 24275 RVA: 0x0020763C File Offset: 0x0020583C
		public void Tick()
		{
			foreach (RitualVisualEffectComp ritualVisualEffectComp in this.comps)
			{
				ritualVisualEffectComp.Tick();
			}
			foreach (Mote mote in this.maintainedMotes)
			{
				mote.Maintain();
			}
			foreach (Pair<Effecter, TargetInfo> pair in this.maintainedEffecters)
			{
				pair.First.EffectTick(pair.Second, pair.Second);
			}
		}

		// Token: 0x06005ED4 RID: 24276 RVA: 0x00207720 File Offset: 0x00205920
		public void AddMoteToMaintain(Mote mote)
		{
			this.maintainedMotes.Add(mote);
		}

		// Token: 0x06005ED5 RID: 24277 RVA: 0x0020772E File Offset: 0x0020592E
		public void AddEffecterToMaintain(TargetInfo target, Effecter eff)
		{
			this.maintainedEffecters.Add(new Pair<Effecter, TargetInfo>(eff, target));
		}

		// Token: 0x06005ED6 RID: 24278 RVA: 0x00207744 File Offset: 0x00205944
		public void Cleanup()
		{
			foreach (Pair<Effecter, TargetInfo> pair in this.maintainedEffecters)
			{
				pair.First.Cleanup();
			}
		}

		// Token: 0x06005ED7 RID: 24279 RVA: 0x0020779C File Offset: 0x0020599C
		public void ExposeData()
		{
			Scribe_Defs.Look<RitualVisualEffectDef>(ref this.def, "def");
			Scribe_Collections.Look<RitualVisualEffectComp>(ref this.comps, "comps", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x0400369C RID: 13980
		public LordJob_Ritual ritual;

		// Token: 0x0400369D RID: 13981
		public RitualVisualEffectDef def;

		// Token: 0x0400369E RID: 13982
		public List<RitualVisualEffectComp> comps = new List<RitualVisualEffectComp>();

		// Token: 0x0400369F RID: 13983
		private List<Mote> maintainedMotes = new List<Mote>();

		// Token: 0x040036A0 RID: 13984
		private List<Pair<Effecter, TargetInfo>> maintainedEffecters = new List<Pair<Effecter, TargetInfo>>();
	}
}
