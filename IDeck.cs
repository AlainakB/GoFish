using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoFish_
{
    internal interface IDeck
    {
        public void addCard(Card card);

        public Card pullCard();
    }
}
