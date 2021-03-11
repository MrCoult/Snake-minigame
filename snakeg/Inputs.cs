using System;
using System.Collections;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace snakeg
{
    internal class Inputs
    {
        // Listă cu butoanele de tastatură disponibile
        private static Hashtable keyTable = new Hashtable();

        public static bool KeyPressed(Keys key)
        {
            if (keyTable[key] == null)
            {
                return false;
            }
            return (bool)keyTable[key];
        }

        // Detectează dacă o tastă e apăsată

        public static void ChangeState(Keys key, bool state)
        {
            keyTable[key] = state;
        }
    }
}
