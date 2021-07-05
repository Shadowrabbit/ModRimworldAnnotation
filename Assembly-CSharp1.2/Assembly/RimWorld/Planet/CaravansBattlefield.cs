using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x02002129 RID: 8489
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x17001A8B RID: 6795
		// (get) Token: 0x0600B453 RID: 46163 RVA: 0x000751C0 File Offset: 0x000733C0
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x0600B454 RID: 46164 RVA: 0x000751C8 File Offset: 0x000733C8
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x0600B455 RID: 46165 RVA: 0x000751E2 File Offset: 0x000733E2
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				alsoRemoveWorldObject = true;
				return true;
			}
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x0600B456 RID: 46166 RVA: 0x000751FF File Offset: 0x000733FF
		public override void Tick()
		{
			base.Tick();
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x0600B457 RID: 46167 RVA: 0x00075215 File Offset: 0x00073415
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(240000, -1);
		}

		// Token: 0x0600B458 RID: 46168 RVA: 0x0034543C File Offset: 0x0034363C
		private void CheckWonBattle()
		{
			if (this.wonBattle)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false))
			{
				return;
			}
			TimedDetectionRaids component = base.GetComponent<TimedDetectionRaids>();
			component.SetNotifiedSilently();
			string detectionCountdownTimeLeftString = component.DetectionCountdownTimeLeftString;
			Find.LetterStack.ReceiveLetter("LetterLabelCaravansBattlefieldVictory".Translate(), "LetterCaravansBattlefieldVictory".Translate(detectionCountdownTimeLeftString), LetterDefOf.PositiveEvent, this, null, null, null, null);
			TaleRecorder.RecordTale(TaleDefOf.CaravanAmbushDefeated, new object[]
			{
				base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
			});
			this.wonBattle = true;
		}

		// Token: 0x04007BD7 RID: 31703
		private bool wonBattle;
	}
}
