using System;
using Verse;

namespace RimWorld
{
	// Token: 0x020012BE RID: 4798
	public abstract class GenStep_Ambush : GenStep
	{
		// Token: 0x06006811 RID: 26641 RVA: 0x00201DCC File Offset: 0x001FFFCC
		public override void Generate(Map map, GenStepParams parms)
		{
			CellRect rectToDefend;
			IntVec3 root;
			if (!SiteGenStepUtility.TryFindRootToSpawnAroundRectOfInterest(out rectToDefend, out root, map))
			{
				return;
			}
			this.SpawnTrigger(rectToDefend, root, map, parms);
		}

		// Token: 0x06006812 RID: 26642 RVA: 0x00201DF0 File Offset: 0x001FFFF0
		private void SpawnTrigger(CellRect rectToDefend, IntVec3 root, Map map, GenStepParams parms)
		{
			int nextSignalTagID = Find.UniqueIDsManager.GetNextSignalTagID();
			string signalTag = "ambushActivated-" + nextSignalTagID;
			CellRect rect;
			if (root.IsValid)
			{
				rect = CellRect.CenteredOn(root, 17);
			}
			else
			{
				rect = rectToDefend.ExpandedBy(12);
			}
			SignalAction_Ambush signalAction_Ambush = this.MakeAmbushSignalAction(rectToDefend, root, parms);
			signalAction_Ambush.signalTag = signalTag;
			GenSpawn.Spawn(signalAction_Ambush, rect.CenterCell, map, WipeMode.Vanish);
			RectTrigger rectTrigger = this.MakeRectTrigger();
			rectTrigger.signalTag = signalTag;
			rectTrigger.Rect = rect;
			GenSpawn.Spawn(rectTrigger, rect.CenterCell, map, WipeMode.Vanish);
			TriggerUnfogged triggerUnfogged = (TriggerUnfogged)ThingMaker.MakeThing(ThingDefOf.TriggerUnfogged, null);
			triggerUnfogged.signalTag = signalTag;
			GenSpawn.Spawn(triggerUnfogged, rect.CenterCell, map, WipeMode.Vanish);
		}

		// Token: 0x06006813 RID: 26643 RVA: 0x00046E2A File Offset: 0x0004502A
		protected virtual RectTrigger MakeRectTrigger()
		{
			return (RectTrigger)ThingMaker.MakeThing(ThingDefOf.RectTrigger, null);
		}

		// Token: 0x06006814 RID: 26644 RVA: 0x00201EA0 File Offset: 0x002000A0
		protected virtual SignalAction_Ambush MakeAmbushSignalAction(CellRect rectToDefend, IntVec3 root, GenStepParams parms)
		{
			SignalAction_Ambush signalAction_Ambush = (SignalAction_Ambush)ThingMaker.MakeThing(ThingDefOf.SignalAction_Ambush, null);
			if (parms.sitePart != null)
			{
				signalAction_Ambush.points = parms.sitePart.parms.threatPoints;
			}
			else
			{
				signalAction_Ambush.points = this.defaultPointsRange.RandomInRange;
			}
			int num = Rand.RangeInclusive(0, 2);
			if (num == 0)
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Manhunters;
			}
			else if (num == 1 && PawnGroupMakerUtility.CanGenerateAnyNormalGroup(Faction.OfMechanoids, signalAction_Ambush.points))
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Mechanoids;
			}
			else
			{
				signalAction_Ambush.ambushType = SignalActionAmbushType.Normal;
			}
			return signalAction_Ambush;
		}

		// Token: 0x04004553 RID: 17747
		public FloatRange defaultPointsRange = new FloatRange(180f, 340f);
	}
}
