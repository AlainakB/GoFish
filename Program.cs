using System.Runtime.InteropServices;

namespace GoFish_
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string[] CPUnames = new string[] { "Bob", "Sarah", "Kate", "Robert" };
            //Console.WriteLine("How many CPUs do you want?");
            int answer;
            /*bool success = int.TryParse(Console.ReadLine(), out answer);
            while(!success && answer >= 1 && answer < 5)
            {
                Console.WriteLine("Sorry, that's not a valid answer. Please enter a between 1 && 4");
                success = int.TryParse(Console.ReadLine(), out answer);
            }
            Console.WriteLine("What is your name?");
            string names = Console.ReadLine();
            for(int i = 0;  i < answer; i++)
            {
                names += "," + CPUnames[i];
            }
            answer += 1;*/
            answer = 4;
            string names = "Alaina";
            for (int i = 0; i < answer; i++)
            {
                names += "," + CPUnames[i];
            }

            Game newGame = new Game(answer, names);
            newGame.getReady();
            newGame.startGame();
        }
    }
}
