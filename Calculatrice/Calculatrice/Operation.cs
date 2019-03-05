using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Calculatrice
{
    public class Operation : BaseNotifyPropertyChanged
    {
        public string Entree
        {
            get { return (string)GetField(); }
            set { SetField(value); }
        }

        public double Resultat
        {
            get { return (double)GetField(); }
            set { SetField(value); }
        }
        public string prettyResultat
        {
            get { return (string)GetField(); }
            set { SetField(value); }
        }

        public List<string> Variable
        {
            get { return (List<string>)GetField(); }
            set { SetField(value); }
        }


        public override string ToString()
        {
            return Entree + "\n" + Resultat;
        }

        public Operation(string entre)
        {
            Entree = entre;
            Variable = new List<string>();
            effectuerCalcul();
            beautifyResult();
        }

        /// <summary>
        /// debut des fonctions
        /// </summary>

        private void beautifyResult()
        {
            string strResult = Resultat.ToString();
            string[] entiereDecimale = strResult.Split(',');
            string partieEntiere = entiereDecimale[0];
            string strEntiere = "";
            int n = 0;
            for (int i = partieEntiere.Length - 1; i >= 0; i--)
            {

                strEntiere = partieEntiere.ElementAt(i) + strEntiere;
                n++;
                if (n == 3)
                {
                    strEntiere = " " + strEntiere;
                    n = 0;
                }
            }
            prettyResultat = strEntiere;
            if (entiereDecimale.Length > 1)
            {
                prettyResultat += "," + entiereDecimale[1];
            }
        }

        public bool isANumber(char c)
        {
            if ('0' <= c && c <= '9')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public bool isAOperator(char c)
        {
            if (c == '*' || c == '/' || c == '+' || c == '-')
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private void power(ref char[] tab)
        {
            string varTemp1 = "";
            string varTemp2 = "";
            int x = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] == '^')
                {
                    if (tab[i - 1] == ')')
                    {
                        int nbParenthese = 0;
                        int k = 0;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (tab[j] == ')')
                            {
                                nbParenthese++;
                            }
                            if (tab[j] == '(' && nbParenthese == 1)
                            {
                                k = j;
                                break;
                            }
                            if (tab[j] == '(' && nbParenthese != 1)
                            {
                                nbParenthese--;
                            }
                        }
                        char[] temp = splitTab(tab, k, i - 1);
                        varTemp1 = evaluerParenthese(temp);
                    }
                    else
                    {
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (isANumber(tab[j]) || tab[j] == ',')
                            {
                                varTemp1 = string.Concat(tab[j], varTemp1);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    if (isANumber(tab[i + 1]))
                    {
                        for (int j = i + 1; j < tab.Length; j++)
                        {
                            if (isANumber(tab[j]))
                            {
                                varTemp2 = string.Concat(varTemp2, tab[j]);
                                x = j;
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        x = parenthese(ref tab, i + 1);
                        char[] temp = splitTab(tab, i + 1, x);
                        varTemp2 = evaluerParenthese(temp);
                    }
                    string value = Math.Pow(double.Parse(varTemp1), double.Parse(varTemp2)).ToString();
                    if (i - (varTemp1.Length) >= 0)
                    {
                        tab = concatTab(tab, value, i - (varTemp1.Length), x);
                    }
                    else
                    {
                        tab = concatTab(tab, value, 0, x);
                    }

                }
            }

        }

        long Factoriel(long n)
        {
            if (n == 1 || n == 0)
            {
                return 1;
            }
            return n * Factoriel(n - 1);
        }

        private void facto(ref char[] tab)
        {
            string varTemp1 = "";
            int x = 0;
            for (int i = 0; i < tab.Length; i++)
            {
                if (tab[i] == '!')
                {
                    x = i;
                    if (tab[i - 1] == ')')
                    {
                        int nbParenthese = 0;
                        int k = 0;
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (tab[j] == ')')
                            {
                                nbParenthese++;
                            }
                            if (tab[j] == '(' && nbParenthese == 1)
                            {
                                k = j;
                                break;
                            }
                            if (tab[j] == '(' && nbParenthese != 1)
                            {
                                nbParenthese--;
                            }
                        }
                        char[] temp = splitTab(tab, k, i - 1);
                        varTemp1 = evaluerParenthese(temp);
                    }
                    else
                    {
                        for (int j = i - 1; j >= 0; j--)
                        {
                            if (isANumber(tab[j]))
                            {
                                varTemp1 = string.Concat(tab[j], varTemp1);
                            }
                            else
                            {
                                break;
                            }
                        }
                    }
                    Console.WriteLine(varTemp1);
                    string value = Factoriel(long.Parse(varTemp1)).ToString();
                    tab = concatTab(tab, value, i - (varTemp1.Length), x);
                }
            }

        }

        private void sqrt(ref char[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {

                if (tab[i] == '√')
                {

                    int x = parenthese(ref tab, i + 1);
                    char[] temp = splitTab(tab, i + 1, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Sqrt(double.Parse(varTemp)).ToString();
                    tab = concatTab(tab, value, i, x);
                }

            }

        }

        private void tan(ref char[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {

                if (tab[i] == 't')
                {

                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Tan(DegreeToRadian(double.Parse(varTemp))).ToString();
                    tab = concatTab(tab, value, i, x);
                }

            }

        }

        private void exp(ref char[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {

                if (tab[i] == 'e')
                {

                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Exp(double.Parse(varTemp)).ToString();
                    tab = concatTab(tab, value, i, x);
                }

            }

        }

        private double DegreeToRadian(double angle)
        {
            return Math.PI * angle / 180.0;
        }

        private void sin(ref char[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {

                if (tab[i] == 's' && tab[i + 1] == 'i')
                {

                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    Console.WriteLine(Math.Sin(2));
                    string value = Math.Sin(DegreeToRadian(double.Parse(varTemp))).ToString();
                    tab = concatTab(tab, value, i, x);
                }

            }

        }

        private void cos(ref char[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {

                if (tab[i] == 'c')
                {

                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    Console.WriteLine(Math.Cos(2));
                    string value = Math.Cos(DegreeToRadian(double.Parse(varTemp))).ToString();
                    tab = concatTab(tab, value, i, x);
                }

            }

        }

        private void ln(ref char[] tab)
        {

            for (int i = 0; i < tab.Length; i++)
            {

                if (tab[i] == 'l')
                {

                    int x = parenthese(ref tab, i + 2);
                    char[] temp = splitTab(tab, i + 2, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Log10(double.Parse(varTemp)).ToString();
                    tab = concatTab(tab, value, i, x);
                }

            }

        }

        private double calculerVariable(List<string> temp)
        {
            while (temp.Count > 1)
            {
                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i] == "*" || temp[i] == "/")
                    {
                        double res = 0;
                        if (temp[i] == "*")
                        {
                            res = double.Parse(temp[i - 1]) * double.Parse(temp[i + 1]);
                        }
                        if (temp[i] == "/")
                        {
                            res = double.Parse(temp[i - 1]) / double.Parse(temp[i + 1]);
                        }
                        temp[i] = res.ToString();
                        temp.RemoveAt(i - 1);
                        temp.RemoveAt(i);
                        i--;
                    }
                }

                for (int i = 0; i < temp.Count; i++)
                {
                    if (temp[i] == "+" || temp[i] == "-")
                    {
                        double res = 0;
                        if (temp[i] == "+")
                        {
                            res = double.Parse(temp[i - 1]) + double.Parse(temp[i + 1]);
                        }
                        if (temp[i] == "-")
                        {
                            res = double.Parse(temp[i - 1]) - double.Parse(temp[i + 1]);
                        }
                        temp[i] = res.ToString();
                        temp.RemoveAt(i - 1);
                        temp.RemoveAt(i);
                        i--;
                    }
                }

            }

            return double.Parse(temp[0]);
        }

        private string evaluerParenthese(char[] var)
        {
            char[] tab = splitTab(var, 1, var.Count() - 1);
            string varTemp = "";

            sqrt(ref tab);

            exp(ref tab);

            cos(ref tab);

            sin(ref tab);

            tan(ref tab);

            facto(ref tab);

            power(ref tab);

            List<string> temp = new List<string>();

            if (tab[0] != '-')
            {
                for (int i = 0; i < tab.Count(); i++)
                {
                    if (isANumber(tab[i]) || tab[i] == ',')
                    {
                        varTemp = string.Concat(varTemp, tab[i].ToString());
                    }
                    else if (tab[i] == '\0')
                    {
                        temp.Add(varTemp);
                        varTemp = "";
                    }
                    else if (tab[i] == '-' && (tab[i - 1] == '+' || tab[i - 1] == '-' || tab[i - 1] == '*' || tab[i - 1] == '/'))
                    {
                        varTemp += "-";
                    }
                    else
                    {
                        temp.Add(varTemp);
                        temp.Add(tab[i].ToString());
                        varTemp = "";
                    }
                }
            }
            else
            {
                varTemp += "-";
                for (int i = 1; i < tab.Count(); i++)
                {
                    if (isANumber(tab[i]) || tab[i] == ',')
                    {
                        varTemp = string.Concat(varTemp, tab[i].ToString());
                    }
                    else if (tab[i] == '\0')
                    {
                        temp.Add(varTemp);
                        varTemp = "";
                    }
                    else
                    {
                        temp.Add(varTemp);
                        temp.Add(tab[i].ToString());
                        varTemp = "";
                    }
                }
            }
            if (varTemp != "")
            {
                temp.Add(varTemp);
            }
            return calculerVariable(temp).ToString();

        }

        private char[] splitTab(char[] tab, int debut, int fin)
        {
            System.Console.WriteLine(debut + " || " + fin + " || " + ((fin - debut) - 1));

            char[] newTab = new char[fin - debut + 1];

            for (int i = debut; i < fin; i++)
            {
                newTab[i - debut] = tab[i];
            }

            return newTab;
        }

        private char[] concatTab(char[] tab, string val, int d, int f)
        {

            char[] temp1 = new char[d + 1];
            char[] temp2 = new char[tab.Count() - f];
            for (int i = 0; i < tab.Count(); i++)
            {
                if (i < d)
                {
                    temp1[i] = tab[i];
                }
                else if (i > f)
                {
                    temp2[i - (f + 1)] = tab[i];
                }
            }

            string s = string.Concat(new string(temp1).Replace("\0", ""), val, new string(temp2).Replace("\0", ""));

            return s.ToArray();

        }

        private int parenthese(ref char[] tab, int index)
        {
            bool autreParenthese = false;
            for (int i = index + 1; i < tab.Count(); i++)
            {
                if (tab[i] == '(')
                {
                    //autreParenthese = true;
                    int x = parenthese(ref tab, i);
                    char[] temp = splitTab(tab, i, x);
                    string s = evaluerParenthese(temp);
                    tab = concatTab(tab, s, i, x);
                }
                else if (tab[i] == 'c')
                {
                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    Console.WriteLine(Math.Cos(2));
                    string value = Math.Cos(DegreeToRadian(double.Parse(varTemp))).ToString();
                    tab = concatTab(tab, value, i, x);
                }
                else if (tab[i] == 't')
                {
                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Tan(DegreeToRadian(double.Parse(varTemp))).ToString();
                    tab = concatTab(tab, value, i, x);
                }
                else if (tab[i] == 's')
                {
                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    Console.WriteLine(Math.Sin(2));
                    string value = Math.Sin(DegreeToRadian(double.Parse(varTemp))).ToString();
                    tab = concatTab(tab, value, i, x);
                }
                else if (tab[i] == 'e')
                {
                    int x = parenthese(ref tab, i + 3);
                    char[] temp = splitTab(tab, i + 3, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Exp(double.Parse(varTemp)).ToString();
                    tab = concatTab(tab, value, i, x);
                }
                else if (tab[i] == 'l')
                {
                    int x = parenthese(ref tab, i + 2);
                    char[] temp = splitTab(tab, i + 2, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Log10(double.Parse(varTemp)).ToString();
                    tab = concatTab(tab, value, i, x);
                }
                else if (tab[i] == '√')
                {
                    int x = parenthese(ref tab, i + 1);
                    char[] temp = splitTab(tab, i + 1, x);
                    string varTemp = evaluerParenthese(temp);
                    string value = Math.Sqrt(double.Parse(varTemp)).ToString();
                    tab = concatTab(tab, value, i, x);
                }
                else if (tab[i] == ')' && autreParenthese == false)
                {
                    return i;
                }
                else if (tab[i] == ')' && autreParenthese == true)
                {
                    autreParenthese = false;
                }
            }
            return -1;
        }

        public void effectuerCalcul()
        {

            char[] tableau = Entree.ToArray();

            sqrt(ref tableau);

            exp(ref tableau);

            sin(ref tableau);

            cos(ref tableau);

            tan(ref tableau);

            facto(ref tableau);

            power(ref tableau);

            string varTemp = "";

            Console.WriteLine(Factoriel(10));

            for (int i = 0; i < tableau.Count(); i++)
            {
                if (i == 0 && tableau[i] == '-')
                {
                    varTemp += "-";
                }
                if (isANumber(tableau[i]) || tableau[i] == ',')
                {
                    varTemp = string.Concat(varTemp, tableau[i].ToString());
                }
                else if (isAOperator(tableau[i]) && i != 0)
                {
                    if (varTemp != "")
                    {
                        Variable.Add(varTemp);
                    }
                    Variable.Add(tableau[i].ToString());
                    varTemp = "";
                }
                else if (tableau[i] == '(')
                {
                    int index = parenthese(ref tableau, i);
                    char[] temp = splitTab(tableau, i, index);
                    Variable.Add(evaluerParenthese(temp));
                    i = index;
                }
            }
            if (varTemp != "")
            {
                Variable.Add(varTemp);
            }
            Resultat = calculerVariable(Variable);

        }
    }
}