using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000735 RID: 1845
	public sealed class LetterStack : IExposable
	{
		// Token: 0x17000704 RID: 1796
		// (get) Token: 0x06002E72 RID: 11890 RVA: 0x000246E7 File Offset: 0x000228E7
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x17000705 RID: 1797
		// (get) Token: 0x06002E73 RID: 11891 RVA: 0x000246EF File Offset: 0x000228EF
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x06002E74 RID: 11892 RVA: 0x001377D4 File Offset: 0x001359D4
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction, quest, hyperlinkThingDefs);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06002E75 RID: 11893 RVA: 0x001377FC File Offset: 0x001359FC
		public void ReceiveLetter(string label, string text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, null, null);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06002E76 RID: 11894 RVA: 0x00137828 File Offset: 0x00135A28
		public void ReceiveLetter(Letter let, string debugInfo = null)
		{
			if (!let.CanShowInLetterStack)
			{
				return;
			}
			let.def.arriveSound.PlayOneShotOnCamera(null);
			if (Prefs.AutomaticPauseMode >= let.def.pauseMode)
			{
				Find.TickManager.Pause();
			}
			else if (let.def.forcedSlowdown)
			{
				Find.TickManager.slower.SignalForceNormalSpeedShort();
			}
			let.arrivalTime = Time.time;
			let.arrivalTick = Find.TickManager.TicksGame;
			let.debugInfo = debugInfo;
			this.letters.Add(let);
			Find.Archive.Add(let);
			let.Received();
		}

		// Token: 0x06002E77 RID: 11895 RVA: 0x000246F7 File Offset: 0x000228F7
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x06002E78 RID: 11896 RVA: 0x001378CC File Offset: 0x00135ACC
		public void LettersOnGUI(float baseY)
		{
			float num = baseY;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				num -= 30f;
				this.letters[i].DrawButtonAt(num);
				num -= 12f;
			}
			this.lastTopYInt = num;
			if (Event.current.type == EventType.Repaint)
			{
				num = baseY;
				for (int j = this.letters.Count - 1; j >= 0; j--)
				{
					num -= 30f;
					this.letters[j].CheckForMouseOverTextAt(num);
					num -= 12f;
				}
			}
		}

		// Token: 0x06002E79 RID: 11897 RVA: 0x00137968 File Offset: 0x00135B68
		public void LetterStackTick()
		{
			int num = Find.TickManager.TicksGame + 1;
			for (int i = 0; i < this.letters.Count; i++)
			{
				LetterWithTimeout letterWithTimeout = this.letters[i] as LetterWithTimeout;
				if (letterWithTimeout != null && letterWithTimeout.TimeoutActive && letterWithTimeout.disappearAtTick == num)
				{
					letterWithTimeout.OpenLetter();
					return;
				}
			}
		}

		// Token: 0x06002E7A RID: 11898 RVA: 0x001379C8 File Offset: 0x00135BC8
		public void LetterStackUpdate()
		{
			if (this.mouseoverLetterIndex >= 0 && this.mouseoverLetterIndex < this.letters.Count)
			{
				this.letters[this.mouseoverLetterIndex].lookTargets.TryHighlight(true, true, false);
			}
			this.mouseoverLetterIndex = -1;
			for (int i = this.letters.Count - 1; i >= 0; i--)
			{
				if (!this.letters[i].CanShowInLetterStack)
				{
					this.RemoveLetter(this.letters[i]);
				}
			}
		}

		// Token: 0x06002E7B RID: 11899 RVA: 0x0002470C File Offset: 0x0002290C
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x06002E7C RID: 11900 RVA: 0x00137A54 File Offset: 0x00135C54
		public void Notify_FactionRemoved(Faction faction)
		{
			for (int i = 0; i < this.letters.Count; i++)
			{
				if (this.letters[i].relatedFaction == faction)
				{
					this.letters[i].relatedFaction = null;
				}
			}
		}

		// Token: 0x06002E7D RID: 11901 RVA: 0x00137AA0 File Offset: 0x00135CA0
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}

		// Token: 0x04001FA8 RID: 8104
		private List<Letter> letters = new List<Letter>();

		// Token: 0x04001FA9 RID: 8105
		private int mouseoverLetterIndex = -1;

		// Token: 0x04001FAA RID: 8106
		private float lastTopYInt;

		// Token: 0x04001FAB RID: 8107
		private const float LettersBottomY = 350f;

		// Token: 0x04001FAC RID: 8108
		public const float LetterSpacing = 12f;
	}
}
