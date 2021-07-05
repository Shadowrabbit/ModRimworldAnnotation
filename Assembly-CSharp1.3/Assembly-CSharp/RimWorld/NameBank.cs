using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DB7 RID: 3511
	public class NameBank
	{
		// Token: 0x17000E02 RID: 3586
		// (get) Token: 0x0600514A RID: 20810 RVA: 0x001B51FD File Offset: 0x001B33FD
		private IEnumerable<List<string>> AllNameLists
		{
			get
			{
				int num;
				for (int i = 0; i < NameBank.numGenders; i = num + 1)
				{
					for (int j = 0; j < NameBank.numSlots; j = num + 1)
					{
						yield return this.names[i, j];
						num = j;
					}
					num = i;
				}
				yield break;
			}
		}

		// Token: 0x0600514B RID: 20811 RVA: 0x001B5210 File Offset: 0x001B3410
		public NameBank(PawnNameCategory ID)
		{
			this.nameType = ID;
			this.names = new List<string>[NameBank.numGenders, NameBank.numSlots];
			for (int i = 0; i < NameBank.numGenders; i++)
			{
				for (int j = 0; j < NameBank.numSlots; j++)
				{
					this.names[i, j] = new List<string>();
				}
			}
		}

		// Token: 0x0600514C RID: 20812 RVA: 0x001B5274 File Offset: 0x001B3474
		public void ErrorCheck()
		{
			foreach (List<string> list in this.AllNameLists)
			{
				foreach (string str in (from x in list
				group x by x into g
				where g.Count<string>() > 1
				select g.Key).ToList<string>())
				{
					Log.Error("Duplicated name: " + str);
				}
				foreach (string text in list)
				{
					if (text.Trim() != text)
					{
						Log.Error("Trimmable whitespace on name: [" + text + "]");
					}
				}
			}
		}

		// Token: 0x0600514D RID: 20813 RVA: 0x001B53F8 File Offset: 0x001B35F8
		private List<string> NamesFor(PawnNameSlot slot, Gender gender)
		{
			return this.names[(int)gender, (int)slot];
		}

		// Token: 0x0600514E RID: 20814 RVA: 0x001B5408 File Offset: 0x001B3608
		public void AddNames(PawnNameSlot slot, Gender gender, IEnumerable<string> namesToAdd)
		{
			foreach (string item in namesToAdd)
			{
				this.NamesFor(slot, gender).Add(item);
			}
		}

		// Token: 0x0600514F RID: 20815 RVA: 0x001B5458 File Offset: 0x001B3658
		public void AddNamesFromFile(PawnNameSlot slot, Gender gender, string fileName)
		{
			this.AddNames(slot, gender, GenFile.LinesFromFile("Names/" + fileName));
		}

		// Token: 0x06005150 RID: 20816 RVA: 0x001B5474 File Offset: 0x001B3674
		public string GetName(PawnNameSlot slot, Gender gender = Gender.None, bool checkIfAlreadyUsed = true)
		{
			List<string> list = this.NamesFor(slot, gender);
			int num = 0;
			if (list.Count == 0)
			{
				Log.Error(string.Concat(new object[]
				{
					"Name list for gender=",
					gender,
					" slot=",
					slot,
					" is empty."
				}));
				return "Errorname";
			}
			string text;
			for (;;)
			{
				text = list.RandomElement<string>();
				if (checkIfAlreadyUsed && !NameUseChecker.NameWordIsUsed(text))
				{
					break;
				}
				num++;
				if (num > 50)
				{
					return text;
				}
			}
			return text;
		}

		// Token: 0x04003022 RID: 12322
		public PawnNameCategory nameType;

		// Token: 0x04003023 RID: 12323
		private List<string>[,] names;

		// Token: 0x04003024 RID: 12324
		private static readonly int numGenders = Enum.GetValues(typeof(Gender)).Length;

		// Token: 0x04003025 RID: 12325
		private static readonly int numSlots = Enum.GetValues(typeof(PawnNameSlot)).Length;
	}
}
