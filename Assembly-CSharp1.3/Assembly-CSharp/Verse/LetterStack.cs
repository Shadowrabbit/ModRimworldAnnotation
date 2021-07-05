using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x02000404 RID: 1028
	public sealed class LetterStack : IExposable
	{
		// Token: 0x170005D8 RID: 1496
		// (get) Token: 0x06001EC0 RID: 7872 RVA: 0x000C04BD File Offset: 0x000BE6BD
		public List<Letter> LettersListForReading
		{
			get
			{
				return this.letters;
			}
		}

		// Token: 0x170005D9 RID: 1497
		// (get) Token: 0x06001EC1 RID: 7873 RVA: 0x000C04C5 File Offset: 0x000BE6C5
		public float LastTopY
		{
			get
			{
				return this.lastTopYInt;
			}
		}

		// Token: 0x06001EC2 RID: 7874 RVA: 0x000C04D0 File Offset: 0x000BE6D0
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, LookTargets lookTargets, Faction relatedFaction = null, Quest quest = null, List<ThingDef> hyperlinkThingDefs = null, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, lookTargets, relatedFaction, quest, hyperlinkThingDefs);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06001EC3 RID: 7875 RVA: 0x000C04F8 File Offset: 0x000BE6F8
		public void ReceiveLetter(TaggedString label, TaggedString text, LetterDef textLetterDef, string debugInfo = null)
		{
			ChoiceLetter let = LetterMaker.MakeLetter(label, text, textLetterDef, null, null);
			this.ReceiveLetter(let, debugInfo);
		}

		// Token: 0x06001EC4 RID: 7876 RVA: 0x000C051C File Offset: 0x000BE71C
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

		// Token: 0x06001EC5 RID: 7877 RVA: 0x000C05BD File Offset: 0x000BE7BD
		public void RemoveLetter(Letter let)
		{
			this.letters.Remove(let);
			let.Removed();
		}

		// Token: 0x06001EC6 RID: 7878 RVA: 0x000C05D4 File Offset: 0x000BE7D4
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

		// Token: 0x06001EC7 RID: 7879 RVA: 0x000C0670 File Offset: 0x000BE870
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

		// Token: 0x06001EC8 RID: 7880 RVA: 0x000C06D0 File Offset: 0x000BE8D0
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

		// Token: 0x06001EC9 RID: 7881 RVA: 0x000C075B File Offset: 0x000BE95B
		public void Notify_LetterMouseover(Letter let)
		{
			this.mouseoverLetterIndex = this.letters.IndexOf(let);
		}

		// Token: 0x06001ECA RID: 7882 RVA: 0x000C0770 File Offset: 0x000BE970
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

		// Token: 0x06001ECB RID: 7883 RVA: 0x000C07BC File Offset: 0x000BE9BC
		public void ExposeData()
		{
			Scribe_Collections.Look<Letter>(ref this.letters, "letters", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.letters.RemoveAll((Letter x) => x == null);
			}
		}

		// Token: 0x040012D1 RID: 4817
		private List<Letter> letters = new List<Letter>();

		// Token: 0x040012D2 RID: 4818
		private int mouseoverLetterIndex = -1;

		// Token: 0x040012D3 RID: 4819
		private float lastTopYInt;

		// Token: 0x040012D4 RID: 4820
		private const float LettersBottomY = 350f;

		// Token: 0x040012D5 RID: 4821
		public const float LetterSpacing = 12f;
	}
}
