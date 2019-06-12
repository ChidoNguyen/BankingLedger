using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BankLedger.User;
using BankLedger.Ledger;

namespace BankLedger
{
    class Program
    {
        static void Main(string[] args)
        {
            //string name = "Chido";
            //string pass = "TestPass";
            //double amount = 1000000;

            //Checkbook tmp = new Checkbook(name, pass, amount);

            //string tmp2 = "making with";
            //double tmp3 = 5000.50;
            //tmp.withdraw(tmp2, tmp3);

            //double tmp4 = tmp.checkBalance();
            //List<Tuple<string, double>> tmp5 = tmp.checkHistory();

            //Console.WriteLine("Test Balance {0}", tmp4);
            //foreach (Tuple<string,double> item in tmp5)
            //{
            //    Console.WriteLine("TXHISTORY TEST {0}", item);
            //}

            //tmp.deposit("making depo", 5000000);
            //tmp4 = tmp.checkBalance();
            //tmp5 = tmp.checkHistory();

            //Console.WriteLine("Test Balance {0}", tmp4);
            //foreach (Tuple<string, double> item in tmp5)
            //{
            //    Console.WriteLine("TXHISTORY TEST {0}", item);
            //}
            NPC eBank = new NPC();
            eBank.runLedger();
        }
    }
}
