using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F40 RID: 3904
	public class RitualObligationTargetWorker_LightBall : RitualObligationTargetWorker_ThingDef
	{
		// Token: 0x06005CD1 RID: 23761 RVA: 0x001FE9E5 File Offset: 0x001FCBE5
		public RitualObligationTargetWorker_LightBall()
		{
		}

		// Token: 0x06005CD2 RID: 23762 RVA: 0x001FE9ED File Offset: 0x001FCBED
		public RitualObligationTargetWorker_LightBall(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CD3 RID: 23763 RVA: 0x001FE9F8 File Offset: 0x001FCBF8
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			if (!ModLister.CheckIdeology("Lightball target"))
			{
				return false;
			}
			if (!base.CanUseTargetInternal(target, obligation).canUse)
			{
				return false;
			}
			Thing thing = target.Thing;
			CompPowerTrader compPowerTrader = thing.TryGetComp<CompPowerTrader>();
			if (compPowerTrader != null)
			{
				if (compPowerTrader.PowerNet == null || !compPowerTrader.PowerNet.HasActivePowerSource)
				{
					return "RitualTargetLightBallIsNotPowered".Translate();
				}
				List<Thing> forCell = target.Map.listerBuldingOfDefInProximity.GetForCell(target.Cell, (float)this.def.maxSpeakerDistance, ThingDefOf.Loudspeaker, null);
				bool flag = false;
				foreach (Thing thing2 in forCell)
				{
					CompPowerTrader compPowerTrader2 = thing2.TryGetComp<CompPowerTrader>();
					if (thing2.GetRoom(RegionType.Set_All) == thing.GetRoom(RegionType.Set_All) && compPowerTrader2.PowerNet != null && compPowerTrader2.PowerNet.HasActivePowerSource)
					{
						flag = true;
						break;
					}
				}
				if (!flag)
				{
					return "RitualTargetNoPoweredSpeakers".Translate();
				}
			}
			return true;
		}

		// Token: 0x06005CD4 RID: 23764 RVA: 0x001FEB1C File Offset: 0x001FCD1C
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			yield return "RitualTargetLightBallInfo".Translate();
			yield break;
		}
	}
}
