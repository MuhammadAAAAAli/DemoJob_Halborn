using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PermutationCalculator2
{
    internal class Program
    {
        private static readonly int nrCifreInDomeniu = 6;

        private static void Main()
        {
            for (var cifraInFocus = 0; cifraInFocus < nrCifreInDomeniu; cifraInFocus++)
            {
                var ciclu = 0;

                var newDomain = new StringBuilder();
                while (ciclu < nrCifreInDomeniu)
                {
                    newDomain = new StringBuilder();
                    for (var cifra = 0; cifra < nrCifreInDomeniu; cifra++)
                    {
                        if (newDomain.Length == nrCifreInDomeniu)
                        {
                            Db_Repository.Add_Domain_Mirror_Cards_Async(newDomain.ToString(), nrCifreInDomeniu);
                            break;
                        }

                        newDomain.Append(cifra);
                        if (cifra == cifraInFocus)
                        {
                            ciclu++;
                            var ciclu_temp = nrCifreInDomeniu - newDomain.Length < ciclu
                                ? nrCifreInDomeniu - newDomain.Length
                                : ciclu;
                            while (ciclu_temp > 0)
                            {
                                ciclu_temp--;
                                newDomain.Append(cifra);
                            }
                        }
                    }
                }

                // only for last
                Db_Repository.Add_Domain_Mirror_Cards_Async(newDomain.ToString(),nrCifreInDomeniu);
            }


            for (var replaceThis = 0; replaceThis < nrCifreInDomeniu; replaceThis++)
                for (var withThis = 0; withThis < nrCifreInDomeniu; withThis++)
                    if (replaceThis != withThis)
                        DoOneDigitReplacement_Async(replaceThis.ToString(), withThis.ToString());

            // add extra domain digit
            for (var replaceThis = 0; replaceThis < nrCifreInDomeniu; replaceThis++)
                DoOneDigitReplacement_Async(replaceThis.ToString(), "6", nrCifreInDomeniu + 1);

            // add extra domain digit
            for (var replaceThis = 0; replaceThis < nrCifreInDomeniu + 1; replaceThis++)
                DoOneDigitReplacement_Async(replaceThis.ToString(), "7", nrCifreInDomeniu + 2);

            // add extra extra domain digit
            for (var replaceThis = 0; replaceThis < nrCifreInDomeniu + 2; replaceThis++)
                DoOneDigitReplacement_Async(replaceThis.ToString(), "8", nrCifreInDomeniu + 3);

            // add extra extra domain digit
            for (var replaceThis = 0; replaceThis < nrCifreInDomeniu + 3; replaceThis++)
                DoOneDigitReplacement_Async(replaceThis.ToString(), "9", nrCifreInDomeniu + 4);

            // print when done
            using (var db = new AdoEntities())
            {
                Console.WriteLine("Domains : " + db.Halborn_Domains.Count());
                Console.WriteLine("Card : " + db.Halborn_Cards.Count());
            }
        }

        private static void DoOneDigitReplacement_Async(string replaceThis, string withThis, int maxDigit = -1)
        {
            var domains_temp = new HashSet<string>();

            using (var db = new AdoEntities())
            {
                foreach (var domain in db.Halborn_Domains)
                {
                    var temp = domain.domains.Replace(replaceThis, withThis);
                    domains_temp.Add(temp);
                }

                foreach (var dom_temp in domains_temp)
                    Db_Repository.Add_Domain_Mirror_Cards_Async(dom_temp, maxDigit);
            }
        }
    }
}