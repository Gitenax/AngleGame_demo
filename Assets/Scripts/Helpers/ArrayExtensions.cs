namespace Gitenax.AngleCheckers.Helpers
{
	public static class ArrayExtensions
	{
		public static T[] ToOneDimensionArray<T>(this T[,] array)
		{
			T[] newArray = new T[array.Length];
			int index = 0;

			for (int i = 0; i < array.GetLength(0); i++)
			{
				for (int j = 0; j < array.GetLength(1); j++)
				{
					newArray[index] = array[i, j];
					index++;
				}
			}

			return newArray;
		}
	}
}