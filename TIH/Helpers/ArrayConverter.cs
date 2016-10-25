namespace TIH.Helpers
{
    public static class ArrayConverter
    {
        public static T[][] ToJaggedArray<T>(this T[,] mArray)
        {
            var cols = mArray.GetLength(0);
            var rows = mArray.GetLength(1);
            var jArray = new T[cols][];
            for (int i = 0; i < cols; i++)
            {
                jArray[i] = new T[rows];
                for (int j = 0; j < rows; j++)
                {
                    jArray[i][j] = mArray[i, j];
                }
            }
            return jArray;
        }
    }
}
