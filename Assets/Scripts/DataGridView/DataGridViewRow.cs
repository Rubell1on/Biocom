using System.Collections.Generic;
using System.Linq;

namespace CatchyClick
{ 
    public class DataGridViewRow
    {
        public List<DataGridViewCell> cells;
        public DataGridViewRow(IEnumerable<DataGridViewCell> cells)
        {
            this.cells = cells.ToList();
        }
    }
}
