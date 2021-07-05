using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Steamworks;
using Verse;
using Verse.Steam;

namespace RimWorld
{
	// Token: 0x0200101C RID: 4124
	public class Scenario : IExposable, WorkshopUploadable
	{
		// Token: 0x1700108D RID: 4237
		// (get) Token: 0x06006134 RID: 24884 RVA: 0x00210654 File Offset: 0x0020E854
		public IEnumerable<System.Version> SupportedVersions
		{
			get
			{
				yield return new System.Version(VersionControl.CurrentMajor, VersionControl.CurrentMinor);
				yield break;
			}
		}

		// Token: 0x1700108E RID: 4238
		// (get) Token: 0x06006135 RID: 24885 RVA: 0x0021065D File Offset: 0x0020E85D
		public FileInfo File
		{
			get
			{
				return new FileInfo(GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x1700108F RID: 4239
		// (get) Token: 0x06006136 RID: 24886 RVA: 0x0021066F File Offset: 0x0020E86F
		public IEnumerable<ScenPart> AllParts
		{
			get
			{
				yield return this.playerFaction;
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					yield return this.parts[i];
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x17001090 RID: 4240
		// (get) Token: 0x06006137 RID: 24887 RVA: 0x0021067F File Offset: 0x0020E87F
		// (set) Token: 0x06006138 RID: 24888 RVA: 0x0021069F File Offset: 0x0020E89F
		public ScenarioCategory Category
		{
			get
			{
				if (this.categoryInt == ScenarioCategory.Undefined)
				{
					Log.Error("Category is Undefined on Scenario " + this);
				}
				return this.categoryInt;
			}
			set
			{
				this.categoryInt = value;
			}
		}

		// Token: 0x0600613A RID: 24890 RVA: 0x002106D4 File Offset: 0x0020E8D4
		public void ExposeData()
		{
			Scribe_Values.Look<string>(ref this.name, "name", null, false);
			Scribe_Values.Look<string>(ref this.summary, "summary", null, false);
			Scribe_Values.Look<string>(ref this.description, "description", null, false);
			Scribe_Values.Look<PublishedFileId_t>(ref this.publishedFileIdInt, "publishedFileId", PublishedFileId_t.Invalid, false);
			Scribe_Deep.Look<ScenPart_PlayerFaction>(ref this.playerFaction, "playerFaction", Array.Empty<object>());
			Scribe_Collections.Look<ScenPart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.parts.RemoveAll((ScenPart x) => x == null) != 0)
				{
					Log.Warning("Some ScenParts were null after loading.");
				}
				if (this.parts.RemoveAll((ScenPart x) => x.HasNullDefs()) != 0)
				{
					Log.Warning("Some ScenParts had null defs.");
				}
			}
		}

		// Token: 0x0600613B RID: 24891 RVA: 0x002107CC File Offset: 0x0020E9CC
		public IEnumerable<string> ConfigErrors()
		{
			if (this.name.NullOrEmpty())
			{
				yield return "no title";
			}
			if (this.parts.NullOrEmpty<ScenPart>())
			{
				yield return "no parts";
			}
			if (this.playerFaction == null)
			{
				yield return "no playerFaction";
			}
			foreach (ScenPart scenPart in this.AllParts)
			{
				foreach (string text in scenPart.ConfigErrors())
				{
					yield return text;
				}
				IEnumerator<string> enumerator2 = null;
			}
			IEnumerator<ScenPart> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x0600613C RID: 24892 RVA: 0x002107DC File Offset: 0x0020E9DC
		public string GetFullInformationText()
		{
			string result;
			try
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine(this.description);
				stringBuilder.AppendLine();
				foreach (ScenPart scenPart in this.AllParts)
				{
					scenPart.summarized = false;
				}
				foreach (ScenPart scenPart2 in from p in this.AllParts
				orderby p.def.summaryPriority descending, p.def.defName
				where p.visible
				select p)
				{
					string text = scenPart2.Summary(this);
					if (!text.NullOrEmpty())
					{
						stringBuilder.AppendLine(text);
					}
				}
				result = stringBuilder.ToString().TrimEndNewlines();
			}
			catch (Exception ex)
			{
				Log.ErrorOnce("Exception in Scenario.GetFullInformationText():\n" + ex.ToString(), 10395878);
				result = "Cannot read data.";
			}
			return result;
		}

		// Token: 0x0600613D RID: 24893 RVA: 0x0021095C File Offset: 0x0020EB5C
		public string GetSummary()
		{
			return this.summary;
		}

		// Token: 0x0600613E RID: 24894 RVA: 0x00210964 File Offset: 0x0020EB64
		public Scenario CopyForEditing()
		{
			Scenario scenario = new Scenario();
			scenario.name = this.name;
			scenario.summary = this.summary;
			scenario.description = this.description;
			scenario.playerFaction = (ScenPart_PlayerFaction)this.playerFaction.CopyForEditing();
			scenario.parts.AddRange(from p in this.parts
			select p.CopyForEditing());
			scenario.categoryInt = ScenarioCategory.CustomLocal;
			return scenario;
		}

		// Token: 0x0600613F RID: 24895 RVA: 0x002109EC File Offset: 0x0020EBEC
		public void PreConfigure()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreConfigure();
			}
		}

		// Token: 0x06006140 RID: 24896 RVA: 0x00210A38 File Offset: 0x0020EC38
		public Page GetFirstConfigPage()
		{
			List<Page> list = new List<Page>();
			list.Add(new Page_SelectStoryteller());
			list.Add(new Page_CreateWorldParams());
			list.Add(new Page_SelectStartingSite());
			if (ModsConfig.IdeologyActive)
			{
				list.Add(new Page_ConfigureIdeo());
			}
			foreach (Page item in this.parts.SelectMany((ScenPart p) => p.GetConfigPages()))
			{
				list.Add(item);
			}
			Page page = PageUtility.StitchedPages(list);
			if (page != null)
			{
				Page page2 = page;
				while (page2.next != null)
				{
					page2 = page2.next;
				}
				page2.nextAct = delegate()
				{
					PageUtility.InitGameStart();
				};
			}
			return page;
		}

		// Token: 0x06006141 RID: 24897 RVA: 0x00210B2C File Offset: 0x0020ED2C
		public bool AllowPlayerStartingPawn(Pawn pawn, bool tryingToRedress, PawnGenerationRequest req)
		{
			using (IEnumerator<ScenPart> enumerator = this.AllParts.GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					if (!enumerator.Current.AllowPlayerStartingPawn(pawn, tryingToRedress, req))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x06006142 RID: 24898 RVA: 0x00210B84 File Offset: 0x0020ED84
		public void Notify_NewPawnGenerating(Pawn pawn, PawnGenerationContext context)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_NewPawnGenerating(pawn, context);
			}
		}

		// Token: 0x06006143 RID: 24899 RVA: 0x00210BD0 File Offset: 0x0020EDD0
		public void Notify_PawnGenerated(Pawn pawn, PawnGenerationContext context, bool redressed)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.Notify_PawnGenerated(pawn, context, redressed);
			}
		}

		// Token: 0x06006144 RID: 24900 RVA: 0x00210C20 File Offset: 0x0020EE20
		public void Notify_PawnDied(Corpse corpse)
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Notify_PawnDied(corpse);
			}
		}

		// Token: 0x06006145 RID: 24901 RVA: 0x00210C58 File Offset: 0x0020EE58
		public void PostWorldGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostWorldGenerate();
			}
		}

		// Token: 0x06006146 RID: 24902 RVA: 0x00210CA4 File Offset: 0x0020EEA4
		public void PostIdeoChosen()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostIdeoChosen();
			}
		}

		// Token: 0x06006147 RID: 24903 RVA: 0x00210CF0 File Offset: 0x0020EEF0
		public void PreMapGenerate()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PreMapGenerate();
			}
		}

		// Token: 0x06006148 RID: 24904 RVA: 0x00210D3C File Offset: 0x0020EF3C
		public void GenerateIntoMap(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.GenerateIntoMap(map);
			}
		}

		// Token: 0x06006149 RID: 24905 RVA: 0x00210D88 File Offset: 0x0020EF88
		public void PostMapGenerate(Map map)
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostMapGenerate(map);
			}
		}

		// Token: 0x0600614A RID: 24906 RVA: 0x00210DD4 File Offset: 0x0020EFD4
		public void PostGameStart()
		{
			foreach (ScenPart scenPart in this.AllParts)
			{
				scenPart.PostGameStart();
			}
		}

		// Token: 0x0600614B RID: 24907 RVA: 0x00210E20 File Offset: 0x0020F020
		public float GetStatFactor(StatDef stat)
		{
			float num = 1f;
			for (int i = 0; i < this.parts.Count; i++)
			{
				ScenPart_StatFactor scenPart_StatFactor = this.parts[i] as ScenPart_StatFactor;
				if (scenPart_StatFactor != null)
				{
					num *= scenPart_StatFactor.GetStatFactor(stat);
				}
			}
			return num;
		}

		// Token: 0x0600614C RID: 24908 RVA: 0x00210E6C File Offset: 0x0020F06C
		public void TickScenario()
		{
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].Tick();
			}
		}

		// Token: 0x0600614D RID: 24909 RVA: 0x00210EA0 File Offset: 0x0020F0A0
		public void RemovePart(ScenPart part)
		{
			if (!this.parts.Contains(part))
			{
				Log.Error("Cannot remove: " + part);
			}
			this.parts.Remove(part);
		}

		// Token: 0x0600614E RID: 24910 RVA: 0x00210ED0 File Offset: 0x0020F0D0
		public bool CanReorder(ScenPart part, ReorderDirection dir)
		{
			if (!part.def.PlayerAddRemovable)
			{
				return false;
			}
			int num = this.parts.IndexOf(part);
			if (dir == ReorderDirection.Up)
			{
				return num != 0 && (num <= 0 || this.parts[num - 1].def.PlayerAddRemovable);
			}
			if (dir == ReorderDirection.Down)
			{
				return num != this.parts.Count - 1;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600614F RID: 24911 RVA: 0x00210F40 File Offset: 0x0020F140
		public void Reorder(ScenPart part, ReorderDirection dir)
		{
			int num = this.parts.IndexOf(part);
			this.parts.RemoveAt(num);
			if (dir == ReorderDirection.Up)
			{
				this.parts.Insert(num - 1, part);
			}
			if (dir == ReorderDirection.Down)
			{
				this.parts.Insert(num + 1, part);
			}
		}

		// Token: 0x06006150 RID: 24912 RVA: 0x00210F8C File Offset: 0x0020F18C
		public bool CanToUploadToWorkshop()
		{
			return this.Category != ScenarioCategory.FromDef && this.TryUploadReport().Accepted && !this.GetWorkshopItemHook().MayHaveAuthorNotCurrentUser;
		}

		// Token: 0x06006151 RID: 24913 RVA: 0x00210FC8 File Offset: 0x0020F1C8
		public void PrepareForWorkshopUpload()
		{
			string path = this.name + Rand.RangeInclusive(100, 999).ToString();
			this.tempUploadDir = Path.Combine(GenFilePaths.TempFolderPath, path);
			DirectoryInfo directoryInfo = new DirectoryInfo(this.tempUploadDir);
			if (directoryInfo.Exists)
			{
				directoryInfo.Delete();
			}
			directoryInfo.Create();
			string text = Path.Combine(this.tempUploadDir, this.name);
			text += ".rsc";
			GameDataSaveLoader.SaveScenario(this, text);
		}

		// Token: 0x06006152 RID: 24914 RVA: 0x0021104C File Offset: 0x0020F24C
		public AcceptanceReport TryUploadReport()
		{
			if (this.name == null || this.name.Length < 3 || this.summary == null || this.summary.Length < 3 || this.description == null || this.description.Length < 3)
			{
				return "TextFieldsMustBeFilled".TranslateSimple();
			}
			return AcceptanceReport.WasAccepted;
		}

		// Token: 0x06006153 RID: 24915 RVA: 0x002110B0 File Offset: 0x0020F2B0
		public PublishedFileId_t GetPublishedFileId()
		{
			return this.publishedFileIdInt;
		}

		// Token: 0x06006154 RID: 24916 RVA: 0x002110B8 File Offset: 0x0020F2B8
		public void SetPublishedFileId(PublishedFileId_t newPfid)
		{
			this.publishedFileIdInt = newPfid;
			if (this.Category == ScenarioCategory.CustomLocal && !this.fileName.NullOrEmpty())
			{
				GameDataSaveLoader.SaveScenario(this, GenFilePaths.AbsPathForScenario(this.fileName));
			}
		}

		// Token: 0x06006155 RID: 24917 RVA: 0x002110E8 File Offset: 0x0020F2E8
		public string GetWorkshopName()
		{
			return this.name;
		}

		// Token: 0x06006156 RID: 24918 RVA: 0x002110F0 File Offset: 0x0020F2F0
		public string GetWorkshopDescription()
		{
			return this.GetFullInformationText();
		}

		// Token: 0x06006157 RID: 24919 RVA: 0x002110F8 File Offset: 0x0020F2F8
		public string GetWorkshopPreviewImagePath()
		{
			return GenFilePaths.ScenarioPreviewImagePath;
		}

		// Token: 0x06006158 RID: 24920 RVA: 0x002110FF File Offset: 0x0020F2FF
		public IList<string> GetWorkshopTags()
		{
			return new List<string>
			{
				"Scenario"
			};
		}

		// Token: 0x06006159 RID: 24921 RVA: 0x00211111 File Offset: 0x0020F311
		public DirectoryInfo GetWorkshopUploadDirectory()
		{
			return new DirectoryInfo(this.tempUploadDir);
		}

		// Token: 0x0600615A RID: 24922 RVA: 0x0021111E File Offset: 0x0020F31E
		public WorkshopItemHook GetWorkshopItemHook()
		{
			if (this.workshopHookInt == null)
			{
				this.workshopHookInt = new WorkshopItemHook(this);
			}
			return this.workshopHookInt;
		}

		// Token: 0x0600615B RID: 24923 RVA: 0x0021113A File Offset: 0x0020F33A
		public override string ToString()
		{
			if (this.name.NullOrEmpty())
			{
				return "LabellessScenario";
			}
			return this.name;
		}

		// Token: 0x0600615C RID: 24924 RVA: 0x00211158 File Offset: 0x0020F358
		public override int GetHashCode()
		{
			int num = 6126121;
			if (this.name != null)
			{
				num ^= this.name.GetHashCode();
			}
			if (this.summary != null)
			{
				num ^= this.summary.GetHashCode();
			}
			if (this.description != null)
			{
				num ^= this.description.GetHashCode();
			}
			return num ^ this.publishedFileIdInt.GetHashCode();
		}

		// Token: 0x0400376E RID: 14190
		[MustTranslate]
		public string name;

		// Token: 0x0400376F RID: 14191
		[MustTranslate]
		public string summary;

		// Token: 0x04003770 RID: 14192
		[MustTranslate]
		public string description;

		// Token: 0x04003771 RID: 14193
		internal ScenPart_PlayerFaction playerFaction;

		// Token: 0x04003772 RID: 14194
		internal List<ScenPart> parts = new List<ScenPart>();

		// Token: 0x04003773 RID: 14195
		private PublishedFileId_t publishedFileIdInt = PublishedFileId_t.Invalid;

		// Token: 0x04003774 RID: 14196
		private ScenarioCategory categoryInt;

		// Token: 0x04003775 RID: 14197
		[NoTranslate]
		public string fileName;

		// Token: 0x04003776 RID: 14198
		private WorkshopItemHook workshopHookInt;

		// Token: 0x04003777 RID: 14199
		[NoTranslate]
		private string tempUploadDir;

		// Token: 0x04003778 RID: 14200
		public bool enabled = true;

		// Token: 0x04003779 RID: 14201
		public bool showInUI = true;

		// Token: 0x0400377A RID: 14202
		public const int NameMaxLength = 55;

		// Token: 0x0400377B RID: 14203
		public const int SummaryMaxLength = 300;

		// Token: 0x0400377C RID: 14204
		public const int DescriptionMaxLength = 1000;
	}
}
