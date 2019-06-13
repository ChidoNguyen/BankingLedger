using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLedger.User
{
    public class Checkbook
    {
        //Member Variables//
        private string userID;
        private string userPASS;
        private double balance;
        private List<Tuple<string,double>> txHistory = new List<Tuple<string,double>>();


        public string USERID {
            get { return this.userID; }
            //set { this.userID = value; }
        }

        // Create the user //
        public Checkbook(string user, string pass, double cash)
        {
            this.userID = user;
            this.userPASS = pass; // ideally we want to hash this prior to creating the user
            this.balance = cash;
        }

        // Users abilities //
        // Withdraw , Deposit , Transaction History , check balance //

        /*
         Function : withdrawTransaction
         Job : lets user track/update withdrawals
         Params: takes a string description, and a float amount
             */

        public void withdrawTransaction(string description, double amount)
        {
            //Update Tx Log
            string updatedDes = "Withdrawal: " + description;
            var txPair = new Tuple<string, double>(updatedDes, amount);
            updateTransactionHistory(txPair);
        }
        /*
         Function : depositTransaction
         Job : lets user track/update deposits
         Params: takes a string description, and a float amount
             */
        public void depositTransaction(string description, double amount)
        {
            string updatedDes = "Deposit: " + description;
            var txPair = new Tuple<string, double>(updatedDes, amount);
            updateTransactionHistory(txPair);
        }

        /*Function: checkBalance
         * Job: allows user to view balance
         * Param: none
         * Returns: balance value
         */
         public double checkBalance()
        {
            return this.balance;
        }

        /*Function: checkHistory
         * Job: allows user to have access to previous transactions
         * Param: None
         * Returns: list of tuples (strings and floats)
         * */

         public List<Tuple<string, double>> checkHistory()
        {
            return this.txHistory;
        }



        // Utilities //
        /*
         Function: updateBalance
         Job: Update the users running balance
         Param: Takes a float value
         Returns: Nothing
             */
        public void updateBalance(double amount)
        {
            this.balance += amount;
        }

        /*
         Function: updateTransactionHistory
         Job: Update the users tx History
         Param: Takes a tuple ( or pair) of string and float value representing description + amount
         Returns: Nothing
             */
        private void updateTransactionHistory(Tuple<string, double> memo)
        {
            this.txHistory.Add(memo);
        }

        /* Function : checkPassword
         * Job: a basic comparison to see if user input password matches login ID password
         * Param: String for password
         * Returns: boolean
         * */
        public bool checkPassword(string pass)
        {
            if (this.userPASS == pass)
                return true;
            else
                return false;
        }
    }
}
