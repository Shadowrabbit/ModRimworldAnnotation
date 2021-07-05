using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F38 RID: 3896
	public class RitualObligationTargetWorker_Altar : RitualObligationTargetFilter
	{
		// Token: 0x06005CA7 RID: 23719 RVA: 0x001FE22F File Offset: 0x001FC42F
		public RitualObligationTargetWorker_Altar()
		{
		}

		// Token: 0x06005CA8 RID: 23720 RVA: 0x001FE237 File Offset: 0x001FC437
		public RitualObligationTargetWorker_Altar(RitualObligationTargetFilterDef def) : base(def)
		{
		}

		// Token: 0x06005CA9 RID: 23721 RVA: 0x001FE4FE File Offset: 0x001FC6FE
		public override IEnumerable<TargetInfo> GetTargets(RitualObligation obligation, Map map)
		{
			if (!ModLister.CheckIdeology("Altar target"))
			{
				yield break;
			}
			Ideo ideo = this.parent.ideo;
			foreach (TargetInfo targetInfo in RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, map, ideo))
			{
				yield return targetInfo;
			}
			IEnumerator<TargetInfo> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005CAA RID: 23722 RVA: 0x001FE51C File Offset: 0x001FC71C
		public static IEnumerable<TargetInfo> GetTargetsWorker(RitualObligation obligation, Map map, Ideo ideo)
		{
			int num;
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
			{
				Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.isAltar)
				{
					foreach (Thing t in precept_Building.presenceDemand.AllBuildings(map))
					{
						yield return t;
					}
					IEnumerator<Thing> enumerator = null;
				}
				num = i;
			}
			yield break;
			yield break;
		}

		// Token: 0x06005CAB RID: 23723 RVA: 0x001FE533 File Offset: 0x001FC733
		protected override RitualTargetUseReport CanUseTargetInternal(TargetInfo target, RitualObligation obligation)
		{
			return RitualObligationTargetWorker_Altar.CanUseTargetWorker(target, obligation, this.parent.ideo);
		}

		// Token: 0x06005CAC RID: 23724 RVA: 0x001FE54C File Offset: 0x001FC74C
		public static bool CanUseTargetWorker(TargetInfo target, RitualObligation obligation, Ideo ideo)
		{
			Building building = target.Thing as Building;
			return building != null && building.Faction != null && building.Faction.IsPlayer && RitualObligationTargetWorker_Altar.GetTargetsWorker(obligation, building.Map, ideo).Contains(building);
		}

		// Token: 0x06005CAD RID: 23725 RVA: 0x001FE59D File Offset: 0x001FC79D
		public override IEnumerable<string> GetTargetInfos(RitualObligation obligation)
		{
			foreach (string text in RitualObligationTargetWorker_Altar.GetTargetInfosWorker(this.parent.ideo))
			{
				yield return text;
			}
			IEnumerator<string> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06005CAE RID: 23726 RVA: 0x001FE5AD File Offset: 0x001FC7AD
		public static IEnumerable<string> GetTargetInfosWorker(Ideo ideo)
		{
			int num;
			for (int i = 0; i < ideo.PreceptsListForReading.Count; i = num + 1)
			{
				Precept_Building precept_Building = ideo.PreceptsListForReading[i] as Precept_Building;
				if (precept_Building != null && precept_Building.ThingDef.isAltar)
				{
					yield return precept_Building.LabelCap;
				}
				num = i;
			}
			yield break;
		}

		// Token: 0x06005CAF RID: 23727 RVA: 0x001FE5BD File Offset: 0x001FC7BD
		public override List<string> MissingTargetBuilding(Ideo ideo)
		{
			if (!this.GetTargetInfos(null).Any<string>())
			{
				return new List<string>
				{
					"Altar".Translate()
				};
			}
			return null;
		}
	}
}
