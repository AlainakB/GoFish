using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal class Card
    {
        private int rank;
        private string suit;
        

        public Card(int rank, string suit)
        {
            this.rank = rank;
            this.suit = suit;
        }

        public int Rank
        {
            get { return rank; }
        }

        public string Suit
        {
            get { return suit; }
        }

        public static string convertToString(int number)
        {
            if (number == 1)
            {
                return "Ace";
            }
            else if (number == 2)
            {
                return "Two";
            }
            else if (number == 3)
            {
                return "Three";
            }
            else if (number == 4)
            {
                return "Four";
            }
            else if (number == 5)
            {
                return "Five";
            }
            else if (number == 6)
            {
                return "Six";
            }
            else if (number == 7)
            {
                return "Seven";
            }
            else if (number == 8)
            {
                return "Eight";
            }
            else if (number == 9)
            {
                return "Nine";
            }
            else if (number == 10)
            {
                return "Ten";
            }
            else if (number == 11)
            {
                return "Jack";
            }
            else if (number == 12)
            {
                return "Queen";
            }
            else
            {
                return "King";
            }   
        }

        public override bool Equals(object obj) //provided by chatGPT
        {
            if (obj == null || GetType() != obj.GetType()) //checks if object is null or not of type card
            {
                return false;
            }

            Card otherCard = (Card)obj;
            return this.Rank == otherCard.Rank && this.Suit == otherCard.Suit;
        }

        public override int GetHashCode() //provided by chatGPT
        {
            unchecked // Overflow is fine, just wrap
            {
                int hash = 17;
                hash = hash * 23 + Rank.GetHashCode();
                hash = hash * 23 + Suit.GetHashCode();
                return hash;
            }
        }

        public override string ToString()
        {
            return String.Format("{0} of {1}", convertToString(this.rank), this.suit);
        }


    }

}
