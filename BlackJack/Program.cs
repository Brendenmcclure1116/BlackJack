using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    internal class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to Blackjack!");

            // Initialize deck and players
            Deck deck = new Deck();
            Player player = new Player();
            Player dealer = new Player();

            // Deal initial cards
            player.AddCard(deck.DrawCard());
            dealer.AddCard(deck.DrawCard());
            player.AddCard(deck.DrawCard());
            dealer.AddCard(deck.DrawCard());

            // Display initial hands
            Console.WriteLine($"Your hand: {player.ShowHand()}, Total: {player.GetTotal()}");
            Console.WriteLine($"Dealer's hand: {dealer.ShowPartialHand()}");

            // Player's turn
            while (true)
            {
                Console.Write("Do you want to hit or stand? ");
                string choice = Console.ReadLine().ToLower();

                if (choice == "hit")
                {
                    player.AddCard(deck.DrawCard());
                    Console.WriteLine($"Your hand: {player.ShowHand()}, Total: {player.GetTotal()}");

                    if (player.GetTotal() > 21)
                    {
                        Console.WriteLine("Bust! You lose.");
                        return;
                    }
                }
                else if (choice == "stand")
                {
                    break;
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter 'hit' or 'stand'.");
                }
            }

            // Dealer's turn
            while (dealer.GetTotal() < 17)
            {
                dealer.AddCard(deck.DrawCard());
            }

            Console.WriteLine($"Your hand: {player.ShowHand()}, Total: {player.GetTotal()}");
            Console.WriteLine($"Dealer's hand: {dealer.ShowHand()}, Total: {dealer.GetTotal()}");

            // Determine the winner
            if (dealer.GetTotal() > 21 || player.GetTotal() > dealer.GetTotal())
            {
                Console.WriteLine("You win!");
            }
            else if (player.GetTotal() == dealer.GetTotal())
            {
                Console.WriteLine("It's a tie!");
            }
            else
            {
                Console.WriteLine("You lose!");
              
                Console.ReadLine();
            }
            Console.ReadLine();


        }
    }

    public class Card
    {
        public string Suit { get; set; }
        public string Rank { get; set; }
    }

    public class Deck
    {
        private Card[] cards;
        private int currentCardIndex;

        public Deck()
        {
            string[] suits = { "Hearts", "Diamonds", "Clubs", "Spades" };
            string[] ranks = { "2", "3", "4", "5", "6", "7", "8", "9", "10", "Jack", "Queen", "King", "Ace" };

            cards = new Card[suits.Length * ranks.Length];
            currentCardIndex = 0;

            int i = 0;
            foreach (string suit in suits)
            {
                foreach (string rank in ranks)
                {
                    cards[i] = new Card { Suit = suit, Rank = rank };
                    i++;
                }
            }

            // Shuffle the deck
            Random random = new Random();
            cards = cards.OrderBy(card => random.Next()).ToArray();
        }

        public Card DrawCard()
        {
            Card card = cards[currentCardIndex];
            currentCardIndex++;
            return card;
        }
    }

    public class Player
    {
        private readonly List<Card> hand;

        public Player()
        {
            hand = new List<Card>();
        }

        public void AddCard(Card card)
        {
            hand.Add(card);
        }

        public string ShowHand()
        {
            return string.Join(", ", hand.Select(card => $"{card.Rank} of {card.Suit}"));
        }

        public string ShowPartialHand()
        {
            return $"{hand[0].Rank} of {hand[0].Suit}, Hidden";
        }

        public int GetTotal()
        {
            int total = 0;
            int numAces = 0;

            foreach (Card card in hand)
            {
                if (card.Rank == "Ace")
                {
                    numAces++;
                    total += 11;
                }
                else if (card.Rank == "King" || card.Rank == "Queen" || card.Rank == "Jack")
                {
                    total += 10;
                }
                else
                {
                    total += int.Parse(card.Rank);
                }
            }

            // Adjust for aces
            while (total > 21 && numAces > 0)
            {
                total -= 10;
                numAces--;
            }

            return total;

           
        }
     
    } 
    
}
