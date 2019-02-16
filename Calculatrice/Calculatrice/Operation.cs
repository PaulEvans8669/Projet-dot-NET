using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Calculatrice
{
    public class Operation
    {
        private string _entree;

        public string Entree
        {
            get { return _entree; }
            set { _entree = value; }
        }

        private float _resultat;

        public float Resultat
        {
            get { return _resultat; }
            set { _resultat = value; }
        }

        public override string ToString()
        {
            return Entree + "\n" + Resultat;
        }

        public Operation(string entre)
        {
            Entree = entre;
            Resultat = 0;
        }
    }
}
