using System;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Threading;

namespace PermutationCalculator2
{
    public class Db_Repository
    {
        public static void AddCardsRange_Async(char[][] result)
        {
            try
            {
                using (var db = new AdoEntities())
                {
                    foreach (var r in result)
                    {
                        var concatedCard = string.Join("", r);
                        if (!db.Halborn_Cards.Any(x => x.cards == concatedCard))
                        {

                            db.Halborn_Cards.Add(new Halborn_Cards { cards = concatedCard });
                        }

                        db.SaveChanges();
                    }
                }
            }
            catch (Exception ex)
            {
                if (!ex.ToString().Contains(" Cannot insert duplicate key"))
                    throw ex;

            }
        }

        public static void Add_Domain_Mirror_Cards_Async(string newDigitDomain, int nrCifreInDomeniu, int maxDigit = -1)
        {
            using (var db = new AdoEntities())
            {
                // check if domain in DB
                if (!db.Halborn_Domains.Any(x => x.domains == newDigitDomain))
                {
                    db.Halborn_Domains.Add(new Halborn_Domains { domains = newDigitDomain });
                    db.SaveChanges();

                    DoPermutations_Async(newDigitDomain);
                }

                // make digits domain mirror
                var mirrorDigitDomain = new StringBuilder();
                foreach (var digit in newDigitDomain.ToCharArray())
                    mirrorDigitDomain.Append((maxDigit == -1 ? nrCifreInDomeniu : maxDigit) - 1 -
                                             int.Parse(digit.ToString()));

                // check if domain in DB
                if (!db.Halborn_Domains.Any(x => x.domains == newDigitDomain))
                {
                    db.Halborn_Domains.Add(new Halborn_Domains { domains = mirrorDigitDomain.ToString() });
                    db.SaveChanges();

                    DoPermutations_Async(mirrorDigitDomain.ToString());
                }
            }
        }

        private static void DoPermutations_Async(string newDigitDomain)
        {
            new Thread(() =>
            {
                // add cards for this domain
                var permutationCalculator = new PermutationCalculator();
                var result = permutationCalculator.Calculate(newDigitDomain.ToCharArray());
                AddCardsRange_Async(result);
            }).Start();
        }
    }
}