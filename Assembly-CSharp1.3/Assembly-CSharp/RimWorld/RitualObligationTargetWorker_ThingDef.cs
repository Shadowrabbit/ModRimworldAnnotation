using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F33 RID: 3891
	public class RitualObligationTargetWorker_ThingDef : RitualObligationTargetFilter
	{
		// Token: 0x06005C8A RID: 23690 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_ThingDef()
		{
		}

		// Token: 0x06005C8B RID: 23691 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_ThingDef(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005C8C RID: 23692 RVA: 0x001FE240 File Offset: 0x001FC440
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (this.def.thingDefs.NullOrEmpty<ThingDef>())
			{
				yield break;
			}
			int num;
			for (int i = 0; i < this.def.thingDefs.Count; i = num + 1)
			{
				ThingDef def = this.def.thingDefs[i];
				List<Thing> things = map.listerThings.ThingsOfDef(def);
				for (int j = 0; j < things.Count; j = num + 1)
				{
					if (base.CanUseTarget(things[j], obligation).canUse)
					{
						yield return things[j];
					}
					num = j;
				}
				things = null;
				num = i;
			}
			yield break;
		}

		// Token: 0x06005C8D RID: 23693 RVA: 0x001FE260 File Offset: 0x001FC460
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return target.HasThing && this.def.thingDefs.Contains(target.Thing.def) && (!this.def.colonistThingsOnly || (target.Thing.Faction != null && target.Thing.Faction.IsPlayer));
		}

		// Token: 0x06005C8E RID: 23694 RVA: 0x001FE2CE File Offset: 0x001FC4CE
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			int num;
			for (int i = 0; i < this.def.thingDefs.Count; i = num + 1)
			{
				yield return "RitualTargetThingDefsInfo".Translate(this.def.thingDefs[i].label);
				num = i;
			}
			yield break;
		}
	}
}
