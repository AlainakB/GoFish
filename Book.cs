using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal class Book
    {
        private Card[] book = new Card[4];
        private int rank;

        public int Rank
        {
            get { return rank; }
        }

        public Book(int rank)
        {
            this.rank = rank;        }

        public void fillBook(Card card)
        {
            for (int i = 0; i < book.Length; i++)
            {
                if (book[i] == null)
                {
                    book[i] = card;
                }
            }
        }

    }
}
