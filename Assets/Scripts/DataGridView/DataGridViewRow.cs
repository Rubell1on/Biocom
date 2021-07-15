using System.Collections.Generic;
using System.Linq;
using System;

namespace CatchyClick
{
    [Serializable]
    public class DataGridViewRow
    {
        public List<DataGridViewCell> cells;
        public DataGridViewRow(IEnumerable<DataGridViewCell> cells)
        {
            this.cells = cells.ToList();
        }
    }
}
