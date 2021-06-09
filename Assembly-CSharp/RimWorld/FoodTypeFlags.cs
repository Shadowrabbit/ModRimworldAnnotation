using System;

namespace RimWorld
{
	// Token: 0x02000F33 RID: 3891
	[Flags]
	public enum FoodTypeFlags
	{
		// Token: 0x040036EC RID: 14060
		None = 0,
		// Token: 0x040036ED RID: 14061
		VegetableOrFruit = 1,
		// Token: 0x040036EE RID: 14062
		Meat = 2,
		// Token: 0x040036EF RID: 14063
		Fluid = 4,
		// Token: 0x040036F0 RID: 14064
		Corpse = 8,
		// Token: 0x040036F1 RID: 14065
		Seed = 16,
		// Token: 0x040036F2 RID: 14066
		AnimalProduct = 32,
		// Token: 0x040036F3 RID: 14067
		Plant = 64,
		// Token: 0x040036F4 RID: 14068
		Tree = 128,
		// Token: 0x040036F5 RID: 14069
		Meal = 256,
		// Token: 0x040036F6 RID: 14070
		Processed = 512,
		// Token: 0x040036F7 RID: 14071
		Liquor = 1024,
		// Token: 0x040036F8 RID: 14072
		Kibble = 2048,
		// Token: 0x040036F9 RID: 14073
		VegetarianAnimal = 3857,
		// Token: 0x040036FA RID: 14074
		VegetarianRoughAnimal = 3921,
		// Token: 0x040036FB RID: 14075
		CarnivoreAnimal = 2826,
		// Token: 0x040036FC RID: 14076
		CarnivoreAnimalStrict = 10,
		// Token: 0x040036FD RID: 14077
		OmnivoreAnimal = 3867,
		// Token: 0x040036FE RID: 14078
		OmnivoreRoughAnimal = 3931,
		// Token: 0x040036FF RID: 14079
		DendrovoreAnimal = 2705,
		// Token: 0x04003700 RID: 14080
		OvivoreAnimal = 2848,
		// Token: 0x04003701 RID: 14081
		OmnivoreHuman = 3903
	}
}
