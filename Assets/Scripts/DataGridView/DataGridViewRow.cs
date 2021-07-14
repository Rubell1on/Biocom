using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;

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
