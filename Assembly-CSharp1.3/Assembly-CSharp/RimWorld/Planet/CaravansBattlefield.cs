using System;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x020017C6 RID: 6086
	public class CaravansBattlefield : MapParent
	{
		// Token: 0x170016FF RID: 5887
		// (get) Token: 0x06008D4F RID: 36175 RVA: 0x0032D97E File Offset: 0x0032BB7E
		public bool WonBattle
		{
			get
			{
				return this.wonBattle;
			}
		}

		// Token: 0x06008D50 RID: 36176 RVA: 0x0032D986 File Offset: 0x0032BB86
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<bool>(ref this.wonBattle, "wonBattle", false, false);
		}

		// Token: 0x06008D51 RID: 36177 RVA: 0x0032D9A0 File Offset: 0x0032BBA0
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

		// Token: 0x06008D52 RID: 36178 RVA: 0x0032D9BD File Offset: 0x0032BBBD
		public override void Tick()
		{
			base.Tick();
			if (base.HasMap)
			{
				this.CheckWonBattle();
			}
		}

		// Token: 0x06008D53 RID: 36179 RVA: 0x0032D9D3 File Offset: 0x0032BBD3
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			base.GetComponent<TimedDetectionRaids>().StartDetectionCountdown(240000, -1);
		}

		// Token: 0x06008D54 RID: 36180 RVA: 0x0032D9EC File Offset: 0x0032BBEC
		private void CheckWonBattle()
		{
			if (this.wonBattle)
			{
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false, true))
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

		// Token: 0x04005986 RID: 22918
		private bool wonBattle;
	}
}
