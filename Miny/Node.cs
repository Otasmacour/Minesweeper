using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Text;
using System.Threading.Tasks;

namespace Miny
{
    class Node
    {
        public Coordinates coordinates;
        public bool mine;
        public bool exposed;
        public bool marked;
        public int numberOfMinesAround = 0;
        public Label label;
    }
}
