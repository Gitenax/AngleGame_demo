using System;

/*
 * Конструкция сделана так, чтобы иметь возможность
 * отрисовки поля занятых ячеек в инспекторе
 */
namespace PlayArea
{
    [Serializable]
    public struct LogicArrayLayout
    {
        public RowData[] Rows;

        public LogicArrayLayout(int width, int height)
        {
            Rows = new RowData[height];
            
            for (int i = 0; i < Rows.Length; i++)
                Rows[i].Columns = new bool[width];
        }
        
        [Serializable]
        public struct RowData
        {
            public bool[] Columns;
        }
    }
}