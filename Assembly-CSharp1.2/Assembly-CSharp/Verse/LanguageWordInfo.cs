using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using RimWorld.IO;

namespace Verse
{
	// Token: 0x0200020A RID: 522
	public class LanguageWordInfo
	{
		// Token: 0x06000D80 RID: 3456 RVA: 0x000ADB84 File Offset: 0x000ABD84
		public void LoadFrom(Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			VirtualDirectory directory = dir.Item1.GetDirectory("WordInfo").GetDirectory("Gender");
			this.TryLoadFromFile(directory.GetFile("Male.txt"), Gender.Male, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Female.txt"), Gender.Female, dir, lang);
			this.TryLoadFromFile(directory.GetFile("Neuter.txt"), Gender.None, dir, lang);
		}

		// Token: 0x06000D81 RID: 3457 RVA: 0x000ADBE8 File Offset: 0x000ABDE8
		public Gender ResolveGender(string str, string fallback = null)
		{
			Gender result;
			if (!this.TryResolveGender(str, out result) && fallback != null)
			{
				this.TryResolveGender(str, out result);
			}
			return result;
		}

		// Token: 0x06000D82 RID: 3458 RVA: 0x000ADC10 File Offset: 0x000ABE10
		private bool TryResolveGender(string str, out Gender gender)
		{
			LanguageWordInfo.tmpLowercase.Length = 0;
			for (int i = 0; i < str.Length; i++)
			{
				LanguageWordInfo.tmpLowercase.Append(char.ToLower(str[i]));
			}
			string key = LanguageWordInfo.tmpLowercase.ToString();
			if (this.genders.TryGetValue(key, out gender))
			{
				return true;
			}
			gender = Gender.Male;
			return false;
		}

		// Token: 0x06000D83 RID: 3459 RVA: 0x000ADC70 File Offset: 0x000ABE70
		private void TryLoadFromFile(VirtualFile file, Gender gender, Tuple<VirtualDirectory, ModContentPack, string> dir, LoadedLanguage lang)
		{
			string[] array;
			try
			{
				array = file.ReadAllLines();
			}
			catch (DirectoryNotFoundException)
			{
				return;
			}
			catch (FileNotFoundException)
			{
				return;
			}
			if (!lang.TryRegisterFileIfNew(dir, file.FullPath))
			{
				return;
			}
			for (int i = 0; i < array.Length; i++)
			{
				if (!array[i].NullOrEmpty() && !this.genders.ContainsKey(array[i]))
				{
					this.genders.Add(array[i], gender);
				}
			}
		}

		// Token: 0x04000B6A RID: 2922
		private Dictionary<string, Gender> genders = new Dictionary<string, Gender>();

		// Token: 0x04000B6B RID: 2923
		private const string FolderName = "WordInfo";

		// Token: 0x04000B6C RID: 2924
		private const string GendersFolderName = "Gender";

		// Token: 0x04000B6D RID: 2925
		private const string MaleFileName = "Male.txt";

		// Token: 0x04000B6E RID: 2926
		private const string FemaleFileName = "Female.txt";

		// Token: 0x04000B6F RID: 2927
		private const string NeuterFileName = "Neuter.txt";

		// Token: 0x04000B70 RID: 2928
		private static StringBuilder tmpLowercase = new StringBuilder();
	}
}
