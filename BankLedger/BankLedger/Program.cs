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
            NPC eBank = new NPC();
            eBank.runLedger();
        }
    }
}
