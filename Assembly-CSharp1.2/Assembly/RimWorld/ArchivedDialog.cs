using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x0200101E RID: 4126
	public class ArchivedDialog : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x17000DEC RID: 3564
		// (get) Token: 0x06005A07 RID: 23047 RVA: 0x0000C32E File Offset: 0x0000A52E
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x17000DED RID: 3565
		// (get) Token: 0x06005A08 RID: 23048 RVA: 0x0000BBC0 File Offset: 0x00009DC0
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x17000DEE RID: 3566
		// (get) Token: 0x06005A09 RID: 23049 RVA: 0x0003E873 File Offset: 0x0003CA73
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x17000DEF RID: 3567
		// (get) Token: 0x06005A0A RID: 23050 RVA: 0x0003E880 File Offset: 0x0003CA80
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x17000DF0 RID: 3568
		// (get) Token: 0x06005A0B RID: 23051 RVA: 0x0003E888 File Offset: 0x0003CA88
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x17000DF1 RID: 3569
		// (get) Token: 0x06005A0C RID: 23052 RVA: 0x0000A2A7 File Offset: 0x000084A7
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000DF2 RID: 3570
		// (get) Token: 0x06005A0D RID: 23053 RVA: 0x0000C32E File Offset: 0x0000A52E
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x06005A0E RID: 23054 RVA: 0x00006B8B File Offset: 0x00004D8B
		public ArchivedDialog()
		{
		}

		// Token: 0x06005A0F RID: 23055 RVA: 0x001D4358 File Offset: 0x001D2558
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextArchivedDialogID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x06005A10 RID: 23056 RVA: 0x001D43B0 File Offset: 0x001D25B0
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, false, this.title));
		}

		// Token: 0x06005A11 RID: 23057 RVA: 0x001D4414 File Offset: 0x001D2614
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		// Token: 0x06005A12 RID: 23058 RVA: 0x0003E890 File Offset: 0x0003CA90
		public string GetUniqueLoadID()
		{
			return "ArchivedDialog_" + this.ID;
		}

		// Token: 0x04003C99 RID: 15513
		public int ID;

		// Token: 0x04003C9A RID: 15514
		public string text;

		// Token: 0x04003C9B RID: 15515
		public string title;

		// Token: 0x04003C9C RID: 15516
		public Faction relatedFaction;

		// Token: 0x04003C9D RID: 15517
		public int createdTick;
	}
}
